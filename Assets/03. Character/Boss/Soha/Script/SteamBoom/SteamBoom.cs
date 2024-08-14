using MoreMountains.Feedbacks;
using UnityEngine;

public class SteamBoom : MonoBehaviour
{
    [MMFReadOnly][SerializeField] private bool playerInside;
    [SerializeField] private float MaxInsideTime;
    [SerializeField] private Transform edge;
    [SerializeField] private float height;
    [SerializeField] private MMF_Player feedback;
    private float preferLenght;

    private IKnockbackable playerKnockable;
    private float timer;

    private void Start()
    {
        playerKnockable = GameManager.Instance.Player.GetComponent<IKnockbackable>();
        preferLenght = (edge.position - this.transform.position).magnitude;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.TryGetComponent(out playerKnockable);
            playerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
    private void Update()
    {
        if (playerInside)
        {
            timer += Time.deltaTime;
        }
        if (timer > MaxInsideTime)
        {
            steamBoom();
        }
    }
    public void SteamBoomRightNow()
    {
        steamBoom();
    }
    private void steamBoom()
    {
        if (playerInside)
        {
            Vector3 direction = (GameManager.Instance.Player.position - transform.position - Vector3.down * 10f).normalized;

            playerKnockable.Knockback(direction, 60f, transform.position);
        }

        feedback.PlayFeedbacks();
        timer = 0;
    }
}
