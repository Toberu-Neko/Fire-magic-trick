using UnityEngine;

public class WaterBullet_Collider : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject WaterPool;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();
            if (health != null) health.ToDamagePlayer(damage);
            Instantiate(WaterPool, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if(collision.gameObject.tag != "WaterBullet")
        {
            Instantiate(WaterPool, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
       
    }
}
