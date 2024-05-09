using MoreMountains.Tools;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool playerInSide;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInSide = true;
        }
        if(!playerInSide)
        {
            if (other.CompareTag("Bullet"))
            {
                other.GetComponent<Bullet>().OnHitRightNow();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInSide = false;
        }
    }
}
