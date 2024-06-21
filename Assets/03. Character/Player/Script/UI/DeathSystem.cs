using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class DeathSystem : MonoBehaviour
{
    [Header("Script")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Model;
    private PlayerState playerState;

    [Header("VFX")]
    [SerializeField] private MMF_Player PlayerRebirth;

    private void Start()
    {
        playerState = GameManager.Instance.Player.GetComponent<PlayerState>();
    }

    public void Death()
    {
        animator.Play("Enter");
    }
    public void Death_Fast()
    {
        animator.Play("Enterfast");
    }
    public async void Rebirth()
    {
        animator.Play("Exit");
        PlayerRebirth.PlayFeedbacks();
        playerState.SetUseCameraRotate(true);

        await Task.Delay(2000);
        playerState.SetVerticalVelocity(0);
        playerState.TakeControl();
        if (Model!=null && Model.activeSelf==false)
        {
            Model.SetActive(true);
        }
    }
}
