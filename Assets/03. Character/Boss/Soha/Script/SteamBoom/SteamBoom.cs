using MoreMountains.Feedbacks;
using UnityEngine;

public class SteamBoom : MonoBehaviour
{
    [MMFReadOnly][SerializeField] private bool playerInside;
    [SerializeField] private float MaxInsideTime;
    [SerializeField] private Transform edge;
    [SerializeField] private float height;
    [SerializeField] private MMF_Player feedback;
    private ImpactReceiver impact;
    private float preferLenght;

    private void Start()
    {
        player = GameManager.singleton.Player.gameObject;
        impact = GameManager.singleton.Player.GetComponent<ImpactReceiver>();
        preferLenght = (edge.position - this.transform.position).magnitude;
    }

    private GameObject player;
    private float timer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
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
        timerSystem();
    }
    public void SteamBoomRightNow()
    {
        steamBoom();
    }
    private void initialization()
    {
        timer = 0;
    }
    private void timerSystem()
    {
        if(playerInside)
        {
            timer += Time.deltaTime;
        }
        if(timer > MaxInsideTime)
        {
            steamBoom();
        }
    }
    private void steamBoom()
    {
        Vector3 direction = (player.transform.position - this.transform.position).normalized;
        Vector3 direction2D = new Vector3(direction.x, 0, direction.z);
        float overlappingLenght = (player.transform.position - this.transform.position).magnitude;
        float forceLenght = preferLenght - overlappingLenght;
        Vector3 height = new Vector3(0, this.height, 0);
        Vector3 force = direction2D * forceLenght*5 + height;

        feedback.PlayFeedbacks();
        impact.AddImpact(force);
        initialization();
    }
}
