using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerArea_Timeline : MonoBehaviour
{
    //Script
    public PlayableDirector timelinePlayable;
    private PlayerState playerState;

    //varable
    private bool isTrigger = false;
    private bool isReady = false;

    private void Start()
    {
        if (timelinePlayable == null)
        {
            timelinePlayable = GetComponent<PlayableDirector>();
        }
        playerState = GameManager.singleton.Player.GetComponent<PlayerState>();
    }
    private void Update()
    {
        if(isReady)
        {
            if(playerState.isGround)
            {
                if (!isTrigger)
                {
                    TriggerEvent();
                }
            }
        }
    }
    public void readyPlay()
    {
        SetIsReady(true);
    }
    private async void TriggerEvent()
    {
        await Task.Delay(250);
        isTrigger = true;
        timelinePlayable.Play();
        SetIsReady(false);
        return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!isReady)
            {
                SetIsReady(true);
            }
        }
    }
    private void SetIsReady(bool active)
    {
        isReady = active;
    }
}
