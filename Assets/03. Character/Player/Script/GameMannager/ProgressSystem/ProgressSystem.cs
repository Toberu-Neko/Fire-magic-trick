using UnityEngine;

public class ProgressSystem : MonoBehaviour
{
    //Script
    public Transform ProgressCheckPoint;
    private NGP_CameraSystem cameraSystem;
    private DeathSystem _deathSystem;
    private Transform player;

    //delegate
    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath;

    private void Start()
    {
        _deathSystem = GameManager.Instance.UISystem.GetComponent<DeathSystem>();
        player = GameManager.Instance.Player;
        cameraSystem = GameManager.Instance.Player.GetComponent<NGP_CameraSystem>();
    }
    
    public void PlayerDeath()
    {
        if(ProgressCheckPoint != null)
        {
            player.transform.position = ProgressCheckPoint.position;
            player.transform.rotation = ProgressCheckPoint.rotation;
            cameraSystem.LookForward();
            OnPlayerDeath?.Invoke();
            PlayerRebirth();
        }
    }
    public void PlayerRebirth()
    {
        _deathSystem.Rebirth();
    }
}
