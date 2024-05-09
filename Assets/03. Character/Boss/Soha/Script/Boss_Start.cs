using UnityEngine;

public class Boss_Start : MonoBehaviour
{
    [SerializeField] private Boss_System system;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            system.StartBossFight();
        }    
    }
}
