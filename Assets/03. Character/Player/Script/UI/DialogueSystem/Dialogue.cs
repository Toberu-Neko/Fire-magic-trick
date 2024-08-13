using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class Dialogue
{
    public Dialogue_Content[] contents;
}
[System.Serializable]
public class Dialogue_Content
{
    public Sprite CharacterIcon;
    public LocalizedString localizedName;
    public LocalizedString localizedContent;
    public bool playFeedback;
    [Header("Feedbacak¡]if so¡^")]
    public MMF_Player feedback;
}