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

        if (playType == PlayType.OnEnable)
        {
            PlayDirector();
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
            PlayDirector();
        }
    }

    public void EventTrigger()
    {
        if (playType == PlayType.OnEvent)
        {
            PlayDirector();
        }
    }

    private void PlayDirector()
    {
        director.Play();
        isActivated = true;
    }


    private void Director_stopped(PlayableDirector obj)
    {
        DataPersistenceManager.Instance.SaveGame();
        gameObject.SetActive(false);
    }
}
