using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Localization.Components;

public class DialogueUI : MonoBehaviour
{
    //Script
    [SerializeField] private Image characterIcon;
    [SerializeField] private LocalizeStringEvent nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Queue<Dialogue_Content> contents;

    //variables
    private bool isDialogueActive;
    private bool isDialogueAuto;
    private bool canNext = true;
    private void Start()
    {
        contents = new Queue<Dialogue_Content>();

        canNext = true;
    }

    private void Update()
    {
        if (isDialogueActive)
        {
            if(PlayerInputHandler.Instance.AttackInput)
            {
                PlayerInputHandler.Instance.UseAttackInput();
                if (canNext)
                {
                    DisplayNextSentence();
                }
            }
        }
    }

    public void StartDialogue(SO_Dialogue dialogue)
    {
        InitiaDialogue();

        characterIcon.sprite = dialogue.contents[0].CharacterIcon;
        nameText.StringReference = dialogue.contents[0].localizedName;


        contents.Clear();

        foreach (Dialogue_Content content in dialogue.contents)
        {
            contents.Enqueue(content);
        }

        DisplayNextSentence();
    }

    public async void StartDialogue(SO_Dialogue dialogue,float onceAutoTime)
    {
        InitialDialogueAuto();
        isDialogueAuto = true;
        nameText.StringReference = dialogue.contents[0].localizedName;
        characterIcon.sprite = dialogue.contents[0].CharacterIcon;

        contents.Clear();

        foreach (Dialogue_Content content in dialogue.contents)
        {
            contents.Enqueue(content);
        }

        DisplayNextSentence();

        for (int i = 0; i < contents.Count+2; i++)
        {
            await Task.Delay((int)(onceAutoTime*1000));
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        ToNextTimerCooling();

        if(contents.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue_Content content = contents.Dequeue();
        nameText.StringReference = content.localizedName;
        characterIcon.sprite = content.CharacterIcon;
        if(content.feedback != null) content.feedback.PlayFeedbacks();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(content.localizedContent.GetLocalizedString()));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    private void ToNextTimerCooling()
    {
        canNext = false;
        Task.Delay(250).ContinueWith(t => canNext = true);
    }

    private void InitiaDialogue()
    {
        gameObject.SetActive(true);
        isDialogueActive = true;
        contents = new();
    }

    private void InitialDialogueAuto()
    {
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
        dialogueText.text = "";
        isDialogueAuto = true;
    }

    private void EndDialogue()
    {
        InGameUIManager.Instance.DialogueEnd();

        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
        if(isDialogueAuto)
        {
            isDialogueAuto = false;
            return;
        }
        isDialogueActive = false;
    }

}
