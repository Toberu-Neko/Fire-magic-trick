using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class Timeline_Trigger : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private bool isOnce = true;
    //Script
    private PlayableDirector timeline;
    private TimelineState state;

    //varable
    private bool isTrigger = false;

    private void Awake()
    {
        timeline = this.transform.parent.GetComponent<PlayableDirector>();
        state = GetComponent<TimelineState>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTrigger)
            {
                timeline.Play();
            }
        }
    }

    private void Update()
    {
        if(isTrigger && state.isCompelete)
        {
            if (isOnce) return;
            isTrigger = false;
        }
    }
}
