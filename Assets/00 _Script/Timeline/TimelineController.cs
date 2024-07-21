using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : DataPersistMapObjBase
{
    private PlayableDirector director;

    [Header("Awake會在讀取關卡的瞬間觸發")]
    [SerializeField] private PlayType playType;
    private enum PlayType { OnColliderEnter, OnEnable, OnEvent }
    [SerializeField] private bool playOnce = true;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        director.stopped += Director_stopped;
    }
    private void OnDisable()
    {
        director.stopped -= Director_stopped;
    }

    protected override void Start()
    {
        base.Start();

        if (isActivated)
        {
            gameObject.SetActive(false);
            return;
        }

        isActivated = true;

        if (playType == PlayType.OnEnable)
        {
            director.Play();
        }
        else if(playType == PlayType.OnColliderEnter)
        {
            if (gameObject.GetComponent<Collider>() == null)
            {
                Debug.LogError("No Collider attached to " + gameObject.name + ", object disabled.");
                gameObject.SetActive(false);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playType == PlayType.OnColliderEnter)
        {
            director.Play();
        }
    }

    public void EventTrigger()
    {
        if (playType == PlayType.OnEvent)
        {
            director.Play();
        }
    }


    private void Director_stopped(PlayableDirector obj)
    {
        DataPersistenceManager.Instance.SaveGame();
        gameObject.SetActive(false);
    }
}
