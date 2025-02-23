using UnityEngine;
using UnityEngine.Events;

public class TriggerFunction : MonoBehaviour
{
    [Header("OnStartTrigger")]
    [SerializeField] private bool OnStartTrigger = false;
    [SerializeField] UnityEvent OnStartTriggerEvent;
    [Header("OnTriggerEnter")]
    [SerializeField] UnityEvent<Collider> OnTriggerEvent;
    [SerializeField] private bool OnlyOnce = true;
    [Header("OnTriggerStay")]
    [SerializeField] private bool useTriggerStay = false;
    [SerializeField] UnityEvent<Collider> OnTriggerStayEvent;
    [Header("OnTriggerExit")]
    [SerializeField] private bool useTriggerExit = false;
    [SerializeField] UnityEvent<Collider> OnTriggerExitEvent;


    private void Start()
    {
        if(OnStartTrigger)
        {
            OnStartTriggerEvent.Invoke();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerEvent.Invoke(other);
            if (OnlyOnce)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(useTriggerStay)
        {
            if (other.CompareTag("Player"))
            {
                if (OnTriggerStayEvent != null)
                {
                    OnTriggerStayEvent.Invoke(other);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(useTriggerExit)
        {
            if (other.CompareTag("Player"))
            {
                if (OnTriggerExitEvent != null)
                {
                    //Debug.Log("trigger Function");
                    OnTriggerExitEvent.Invoke(other);
                }
            }
        }
    }
}
