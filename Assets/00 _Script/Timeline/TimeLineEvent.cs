using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常用Timeline事件
/// </summary>
public class TimeLineEvent : MonoBehaviour
{
    [SerializeField] private Transform teleportTransform;
    private IPlayerHandler playerInterface;

    private void Start()
    {
        playerInterface = GameManager.Instance.Player.GetComponent<IPlayerHandler>();
    }

    public void OnStart()
    {
        // 開始播放時取消玩家控制，並取消抬頭顯示
        UIManager.Instance.DeactivateHUD();
        DisablePlayerControl();
    }

    public void OnComplete()
    {
        UIManager.Instance.ActivateHUD();
        EnablePlayerControl();
    }

    public void EnablePlayerControl()
    {
        playerInterface.FinishCantControlState();
    }

    public void DisablePlayerControl()
    {
        playerInterface.GotoCantControlState();
    }

    public void DisablePlayerModel()
    {
        playerInterface.SetModel(false);
    }

    public void EnablePlayerModel()
    {
        playerInterface.SetModel(true);
    }

    public void GotoAfterSuperDashJumpState()
    {
        playerInterface.GotoAfterSuperDashJumpState();
    }

    public void ActivateTeach(int index)
    {
        // 觸發教學並取消玩家控制
        UIManager.Instance.ActivateTeachUI(index);
        UIManager.Instance.OnTeachEnd += HandleTeachEnd;
        playerInterface.GotoCantControlState();
    }

    private void HandleTeachEnd()
    {
        UIManager.Instance.OnTeachEnd -= HandleTeachEnd;
        playerInterface.FinishCantControlState();
    }

    public void TeleportPlayerToTLTransform()
    {
        if(teleportTransform == null)
        {
            Debug.LogError("Teleport Transform is null in TLEvent");
            return;
        }
        Debug.Log("Teleporting player to " + teleportTransform.position);
        playerInterface.Teleport(teleportTransform.position);
    }

    public void PlayBGN(string bgmName)
    {
        AudioManager.Instance.PlayBGM(bgmName);
    }
}
