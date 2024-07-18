using UnityEngine;
using UnityEngine.Playables;

public class Timeline_Trigger : DataPersistMapObjBase
{
    [Header("Setting")]
    [SerializeField] private bool isOnce = true;
    //Script
    private PlayableDirector timeline;

    //varable
    private bool isTrigger = false;

    [HideInInspector] public bool isCompelete = true;
    private void Awake()
    {
        timeline = transform.parent.GetComponent<PlayableDirector>();
    }

    protected override void Start()
    {
        base.Start();

        if (isActivated)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTrigger)
            {
                isTrigger = true;
                timeline.Play();
            }
        }
    }

}
