using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineEvent : MonoBehaviour
{
    [SerializeField] private Transform teleportTransform;
    private IPlayerHandler player;

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<IPlayerHandler>();
    }

    public void OnStart()
    {
    }

    public void OnComplete()
    {
    }

    public void EnablePlayerControl()
    {
        player.FinishCantControlState();
    }

    public void DisablePlayerControl()
    {
        player.GotoCantControlState();
    }

    public void DisablePlayerModel()
    {
        player.SetModel(false);
    }

    public void EnablePlayerModel()
    {
        player.SetModel(true);
    }

    public void GotoAfterSuperDashJumpState()
    {
        player.GotoAfterSuperDashJumpState();
    }

    public void TeleportPlayerToTLTransform()
    {
        if(teleportTransform == null)
        {
            Debug.LogError("Teleport Transform is null in TLEvent");
            return;
        }
        player.Teleport(teleportTransform.position);
    }
}
