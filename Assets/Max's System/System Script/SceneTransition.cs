using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>功用: 讀場景和過場動畫</para>
/// <para>使用方式:叫 SceneTransition.LoadeSceneCall?.Invoke(場景名);</para>
/// <para>需要過場動畫的話在叫之前多叫一行  SceneTransition.SetAnimOverrideCall?.Invoke(進場動畫,出場動畫);</para>
/// </summary>
public class SceneTransition : MonoBehaviour
{
    //過場動畫種類 需要的話再加,加完之後去外面調好動畫之後就OK了
    public enum AnimTypes
    {
        FadeInAndOut,
        CircleInAndOut,
        GateInAndOut,
        SideSlideInAndOut,
    }


    //可呼叫功能
    /// <summary> 叫新場景 </summary>
    public static System.Action<string> LoadeSceneCall { get; private set; }

    /// <summary> 設定進出場動畫</summary>
    public static System.Action<AnimTypes, AnimTypes> SetAnimOverrideCall;

    /// <summary> 重讀場景 </summary>
    public static System.Action ReloadeSceneCall;

    /// <summary> 當在讀心場景前呼叫，想要在換場景前做一些事可以附加這裡 </summary>
    [Tooltip("Call BEFORE Load new Scene")] public static event System.Action OnSceneLoadAnnounce;

    /// <summary> 再轉場Animator叫的，不要在程式叫，轉場動畫跑完之後記得補插事件回這裡 </summary>
    public static System.Action AnimationFinishLoadeSceneCall { get; private set; }



    public static AnimTypes OverrideAnimTypeIn;
    public static AnimTypes OverrideAnimTypeOut;

    static bool IsOverride;

    bool ListenStaticAction;


    [SerializeField] List<SceneAnimationPack> Animators;
    public static string Temp_DelayLoadSceneName;

    [SerializeField] UnityEvent LoadSceneEvent;


    #region 火星文 
    void Awake()
    {
        LoadeSceneCall += LoadScene;
        ReloadeSceneCall += ReloadScene;
        SetAnimOverrideCall += SetOverrideAnimtion;
        AnimationFinishLoadeSceneCall += DelayLoadScene;
        ListenStaticAction = true;
    }
    private void Start()
    {
        TryPlayIntro();
    }
    private void OnDestroy()
    {
        if (ListenStaticAction)
        {
            LoadeSceneCall -= LoadScene;
            ReloadeSceneCall -= ReloadScene;
            SetAnimOverrideCall -= SetOverrideAnimtion;
            AnimationFinishLoadeSceneCall -= DelayLoadScene;
        }
    }
    public void LoadScene(string SceneName)
    {
        LoadSceneEvent.Invoke();
        Temp_DelayLoadSceneName = SceneName;
        if (IsOverride)
        {
            if (GetAnimatorPack(OverrideAnimTypeIn) != null)
            {
                GetAnimatorPack(OverrideAnimTypeIn).Animator.SetTrigger("In");

            }
        }
        else
        {
            DelayLoadScene();
        }
    }
    public void SetOverrideAnimtion(Animator InAndOuttypes)
    {

        foreach (SceneAnimationPack pack in Animators)
        {
            if (pack.Animator == InAndOuttypes)
            {
                OverrideAnimTypeIn = pack.Type;
                OverrideAnimTypeOut = pack.Type;
                IsOverride = true;
                return;
            }
        }
    }
    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetOverrideAnimtion(AnimTypes Intypes, AnimTypes Outtypes)
    {
        OverrideAnimTypeIn = Intypes;
        OverrideAnimTypeOut = Outtypes;
        IsOverride = true;
    }
    void TryPlayIntro()
    {
        if (IsOverride)
        {

            if (GetAnimatorPack(OverrideAnimTypeOut) != null)
            {
                GetAnimatorPack(OverrideAnimTypeOut).Animator.SetTrigger("Out");

                IsOverride = false;
            }
        }
    }

    void DelayLoadScene()
    {
        OnSceneLoadAnnounce?.Invoke();  
        SceneManager.LoadScene(Temp_DelayLoadSceneName);
    }
    SceneAnimationPack GetAnimatorPack(AnimTypes Type)
    {
        foreach (SceneAnimationPack pack in Animators)
        {
            if (pack.Type == Type)
            {
                return pack;
            }
        }
        Debug.LogError("Can't Find Animatior for type " + Type.ToString());
        return null;
    }
    
    #endregion 

}
[System.Serializable]
public class SceneAnimationPack
{

    public SceneTransition.AnimTypes Type;
    public Animator Animator;
}