using UnityEngine;

public class EnemyDebuffPlayer : MonoBehaviour
{
    [SerializeField] private float debuffTime;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            //TODO: Stun
        }
    }

}
