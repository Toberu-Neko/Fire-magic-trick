using UnityEngine;

public class WaterBulletTest : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Instantiate(bullet,this.transform.position,this.transform.rotation);
        }
    }
}
