using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Timeline_Trigger : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool isOnce = true;
    //Script
    private PlayableDirector timeline;

    //varable
    private bool isTrigger = false;

    [HideInInspector] public bool isCompelete = true;

    private Player player;
    private void Awake()
    {
        timeline = transform.parent.GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTrigger)
            {
                other.TryGetComponent(out player);
                isTrigger = true;
                timeline.Play();
            }
        }
    }

    public void OnStart()
    {
    }

    public void OnComplete()
    {
    }

    public void EnablePlayerControl()
    {
        player?.FinishCantControlState();
    }

    public void DisablePlayerControl()
    {
        player?.GotoCantControlState();
    }

}
