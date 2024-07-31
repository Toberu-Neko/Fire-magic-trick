using UnityEngine;

public class TriggerArea_TeachFlaot : MonoBehaviour
{
    [SerializeField] TeachFloat.types types;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.OpenTeachFloat(types);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.CloseTeachFloat(types);
        }
    }
}
