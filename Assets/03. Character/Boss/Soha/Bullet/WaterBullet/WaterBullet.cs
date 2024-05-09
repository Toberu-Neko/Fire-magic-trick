using System.Threading.Tasks;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float addSpeed;
    [SerializeField] private float destroyTime = 2f;

    private Rigidbody rb;
    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Vector3 direction = this.transform.forward;
        rb.velocity = direction * speed * Time.deltaTime;
        timer = Time.time;
    }
    private void Update()
    {
        move();

        if(Time.time - timer >= destroyTime)
        {
            Destroy(this.gameObject);
        }
    }
    private void move()
    {
        Vector3 direction = this.transform.forward;
        rb.AddForce(transform.forward * addSpeed, ForceMode.Acceleration);
    }
}
