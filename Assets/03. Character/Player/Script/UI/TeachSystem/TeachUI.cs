using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.Video;

[System.Serializable]
public class TeachSystem_content
{
    public string title;
    public VideoClip video;
    [TextArea(3,10)]public string[] content;
}
public class TeachUI : MonoBehaviour
{
    [Header("Input To TMP")]
    [SerializeField][TextArea(5, 10)] private string debugText;
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private TeachVideo teachVideo;
    [Header("Content")]
    [SerializeField] private TeachSystem_content[] teachSystem_Content;

    //variable
    private int index = 0;
    private int contentIndex = 0;
    private bool isTeach = false;
    private bool canNext = true;

    private void Update()
    {
        if (isTeach)
        {
            if (PlayerInputHandler.Instance.AttackInput || PlayerInputHandler.Instance.JumpInput)
            {
                PlayerInputHandler.Instance.UseAttackInput();
                PlayerInputHandler.Instance.UseJumpInput();

                if (canNext)
                {
                    DisplayNextSentence();
                }
            }
        }
    }

    public void OpenTeach(int index)
    {
        isTeach = true;
        this.index = index;
        contentIndex = 0;

        //UI
        gameObject.SetActive(true);

        //title
        title.text = teachSystem_Content[index].title;

        // video
        teachVideo.ChageVideoClip(teachSystem_Content[index].video);
        teachVideo.videoPlayer.prepareCompleted += OnVideoPrepared;
        teachVideo.videoPlayer.Prepare();
        teachVideo.videoPlayer.Prepare();

        //content
        StopAllCoroutines();
        StartCoroutine(TypeWord(teachSystem_Content[index].content[contentIndex]));
    }

    public void CloseTeach()
    {
        isTeach = false;
        UIManager.Instance.OnDecativateTeachUI();
        gameObject.SetActive(false);
    }

    private void DisplayNextSentence()
    {
        if (teachSystem_Content[index].content.Length == contentIndex+1)
        {
            CloseTeach();
            return;
        }

        ToNextTimerCooling();

        contentIndex++;

        StopAllCoroutines();
        StartCoroutine(TypeWord(teachSystem_Content[index].content[contentIndex]));
    }
    
    private void ToNextTimerCooling()
    {
        canNext = false;
        Task.Delay(250).ContinueWith(t => canNext = true);
    }
    IEnumerator TypeWord(string Content)
    {
        content.text = "";
        foreach (char letter in Content.ToCharArray())
        {
            content.text += letter;
            yield return null;
        }
    }


    private void OnValidate()
    {
        string debugText = "";
        for(int i=0;i<teachSystem_Content.Length;i++)
        {
            debugText += teachSystem_Content[i].title;

            for(int i2 = 0; i2 < teachSystem_Content[i].content.Length;i2++)
            {
                debugText += teachSystem_Content[i].content[i2];
            }
        }
        this.debugText = debugText;
    }
    private void OnVideoPrepared(VideoPlayer source)
    {
        source.Play(); // 使用 source.Play() 而不是 teachVideo.videoPlayer.Play()
    }
}
