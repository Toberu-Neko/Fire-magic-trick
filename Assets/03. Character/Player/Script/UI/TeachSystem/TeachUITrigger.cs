using UnityEngine;

public class TeachUITrigger : DataPersistMapObjBase
{
    [Header("Settings")]
    [SerializeField] private bool canTeachMutipleTimes = false;
    [SerializeField] private int index = 0;
    private IPlayerHandler playerHandler;

    private void OnEnable()
    {
        UIManager.Instance.OnTeachEnd += HandleTeachEnd;
    }

    protected override void Start()
    {
        base.Start();

        if (isActivated)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.OnTeachEnd -= HandleTeachEnd;
    }

    private void HandleTeachEnd()
    {
        playerHandler?.FinishCantControlState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!isActivated)
            {
                other.TryGetComponent(out playerHandler);
                playerHandler?.GotoCantControlState();

                UIManager.Instance.ActivateTeachUI(index);

                if (!canTeachMutipleTimes) isActivated = true;
            }
        }
    }

    public void TriggerThisTeach()
    {
        if (!isActivated)
        {
            UIManager.Instance.ActivateTeachUI(index);
            if (!canTeachMutipleTimes) isActivated = true;
        }
    }
}
