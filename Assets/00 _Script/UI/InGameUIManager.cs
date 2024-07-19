using System;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    [SerializeField] private CrosshairUI crosshairUI;
    [SerializeField] private DialogueUI dialogueUI;

    public event Action OnDialogueEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dialogueUI.gameObject.SetActive(false);
    }

    public void SetCrossRed()
    {
        crosshairUI.SetCrossRed();
    }

    public void SetCrossWhite()
    {
        crosshairUI.SetCrossWhite();
    }

    public void StartDialogue(SO_Dialogue dialogue)
    {
        dialogueUI.StartDialogue(dialogue);
    }

    public void DialogueEnd()
    {
        OnDialogueEnd?.Invoke();
    }

    public void CrosshairShooting()
    {
        crosshairUI.CrosshairShooting();
    }

    public void HitEnemyEffect()
    {
        crosshairUI.HitEffextOn();
    }

    public void StartDialogue(SO_Dialogue dialogue, float time)
    {
        dialogueUI.StartDialogue(dialogue, time);
    }
}
