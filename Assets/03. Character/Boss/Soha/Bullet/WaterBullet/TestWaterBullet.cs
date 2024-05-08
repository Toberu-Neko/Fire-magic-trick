using UnityEngine;

public class TestWaterBullet : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Instantiate(bullet,this.transform.position,Quaternion.identity);
        }
    }
}
