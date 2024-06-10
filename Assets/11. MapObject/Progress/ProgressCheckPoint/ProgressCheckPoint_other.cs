using UnityEngine;

public class ProgressCheckPoint_other : MonoBehaviour
{
    private ProgressSystem _progressSystem;
    [SerializeField] private TriggerArea_ProgressCheckPointArea _progressCheckPoint;
    private void Start()
    {
        _progressSystem = GameManager.Instance.GetComponent<ProgressSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _progressSystem.ProgressCheckPoint = _progressCheckPoint.transform;
        }
    }
}
