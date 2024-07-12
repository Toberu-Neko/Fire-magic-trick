using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;

public class TriggerArea_DialogueTrigger : MonoBehaviour
{
    [Header("Additionals")]
    public MMF_Player NeedFeedbacks;

    [Header("Auto")]
    public bool useAuto;
    public float OnceAutoTime;
    //Variables
    public bool triggerOnce;
    private bool canTrigger = true;
    private bool isReadyDialogue;
    
    [Header("Input To TMP")]
    [SerializeField][TextArea(5, 10)] public string debugText;

    //Script
    public Dialogue dialogue;
    private DialogueManager dialogueManager;


    private void Start()
    {
        dialogueManager = GameManager.Instance.UISystem.GetComponent<DialogueManager>();
        canTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isReadyDialogue)
            {
                isReadyDialogue = true;
            }
        }
    }
    public async void EventTrigger()
    {
        await Task.Delay(250);
        if (canTrigger)
        {
            if (useAuto)
            {
                dialogueManager.StartDialogue(dialogue, OnceAutoTime);
                dialogueManager.OnDialogueEnd += DialogueEnd;
            }
            else
            {
                dialogueManager.StartDialogue(dialogue);
                dialogueManager.OnDialogueEnd += DialogueEnd;
            }

            if (triggerOnce)
            {
                triggerOnce = true;
                canTrigger = false;
            }
        }
    }

    private void DialogueEnd()
    {
        if(NeedFeedbacks != null)
        {
            NeedFeedbacks.PlayFeedbacks();
        }
    }

    private void OnValidate()
    {
        string debugText = "";

        for (int i = 0; i < dialogue.contents.Length; i++)
        {
            debugText += dialogue.contents[i].name;
            debugText += dialogue.contents[i].sentences;
        }
        this.debugText = debugText;
    }
}
