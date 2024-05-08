using UnityEngine;

public class WaterPool : MonoBehaviour
{
    [SerializeField] private float debufTime = 1.5f;
    private void Start()
    {
        Destroy(gameObject,2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerState>().SlowPlayer(debufTime);
        }    
    }
}
