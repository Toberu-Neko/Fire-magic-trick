using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>�\��: Ū�����M�L���ʵe</para>
/// <para>�ϥΤ覡:�s SceneTransition.LoadeSceneCall?.Invoke(�����W);</para>
/// <para>�ݭn�L���ʵe���ܦb�s���e�h�s�@��  SceneTransition.SetAnimOverrideCall?.Invoke(�i���ʵe,�X���ʵe);</para>
/// </summary>
public class SceneTransition : MonoBehaviour
{
    //�L���ʵe���� �ݭn���ܦA�[,�[������h�~���զn�ʵe����NOK�F
    public enum AnimTypes
    {
        FadeInAndOut,
        CircleInAndOut,
        GateInAndOut,
        SideSlideInAndOut,
    }


    //�i�I�s�\��
    /// <summary> �s�s���� </summary>
    public static System.Action<string> LoadeSceneCall { get; private set; }

    /// <summary> �]�w�i�X���ʵe</summary>
    public static System.Action<AnimTypes, AnimTypes> SetAnimOverrideCall;

    /// <summary> ��Ū���� </summary>
    public static System.Action ReloadeSceneCall;

    /// <summary> ��bŪ�߳����e�I�s�A�Q�n�b�������e���@�Ǩƥi�H���[�o�� </summary>
    [Tooltip("Call BEFORE Load new Scene")] public static event System.Action OnSceneLoadAnnounce;

    /// <summary> �A���Animator�s���A���n�b�{���s�A����ʵe�]������O�o�ɴ��ƥ�^�o�� </summary>
    public static System.Action AnimationFinishLoadeSceneCall { get; private set; }



    public static AnimTypes OverrideAnimTypeIn;
    public static AnimTypes OverrideAnimTypeOut;

    static bool IsOverride;

    bool ListenStaticAction;


    [SerializeField] List<SceneAnimationPack> Animators;
    public static string Temp_DelayLoadSceneName;

    [SerializeField] UnityEvent LoadSceneEvent;


    #region ���P�� 
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