using MoreMountains.Feedbacks;
using UnityEngine;

public class TriggerArea_DialogueTrigger : DataPersistMapObjBase
{
    [Header("Additionals")]
    [SerializeField] private MMF_Player NeedFeedbacks;

    [Header("Auto")]
    [SerializeField] private bool triggerOnce;
    [SerializeField] private bool useAuto;
    [SerializeField] private float OnceAutoTime;
    private bool canTrigger = true;
    private bool isReadyDialogue;

    //Script
    [SerializeField] private SO_Dialogue dialogueSO;
    [SerializeField] private MMF_Player[] feedbacksDuringDialogue;
    private int playedFeedbacksIndex = 0;
    public Dialogue dialogue;
    private IPlayerHandler playerHandler;


    private void Awake()
    {
        canTrigger = true;
    }

    protected override void Start()
    {
        base.Start();

        if (isActivated) 
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        InGameUIManager.Instance.OnDisplayNextSentence += HandleDisplayNextSentence;
        playedFeedbacksIndex = 0;
    }

    private void OnDisable()
    {
        InGameUIManager.Instance.OnDisplayNextSentence -= HandleDisplayNextSentence;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!isReadyDialogue)
            {
                isReadyDialogue = true;
                EventTrigger();

                if (!useAuto)
                {
                    other.TryGetComponent(out playerHandler);
                    playerHandler.GotoCantControlState();
                }
            }
        }
    }
    public void EventTrigger()
    {
        if (canTrigger)
        {
            if (useAuto)
            {
                InGameUIManager.Instance.StartDialogue(dialogueSO, OnceAutoTime);
            }
            else
            {
                InGameUIManager.Instance.StartDialogue(dialogueSO);
            }

            InGameUIManager.Instance.OnDialogueEnd += DialogueEnd;

            if (triggerOnce)
            {
                isActivated = true;
                canTrigger = false;
            }
        }
    }

    private void DialogueEnd()
    {
        InGameUIManager.Instance.OnDialogueEnd -= DialogueEnd;
        DataPersistenceManager.Instance.SaveGame();

        if (!useAuto)
        {
            playerHandler.FinishCantControlState();
        }

        if (NeedFeedbacks != null)
        {
            NeedFeedbacks.PlayFeedbacks();
        }
    }

    private void HandleDisplayNextSentence()
    {
        if (feedbacksDuringDialogue.Length > playedFeedbacksIndex)
        {
            feedbacksDuringDialogue[playedFeedbacksIndex].PlayFeedbacks();
            playedFeedbacksIndex++;
        }
    }
}
