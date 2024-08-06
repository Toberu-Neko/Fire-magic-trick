using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private float Energy;

    [Header("Movement")]
    [SerializeField] private float StopTime;
    [SerializeField] private float speed_Start = 100;
    [SerializeField] private float rotateDeviation = 60;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    [Header("NearbyDetect")]
    [SerializeField] private float movementStartDistance;
    [SerializeField] private float absorbDistance;

    private float timer;

    private bool isTimer;
    private bool isTimerFinish;
    private float startTime;

    private GameObject player;
    private Rigidbody rb;
    private void Start()
    {
        player = GameManager.Instance.Player.gameObject;

        rb = GetComponent<Rigidbody>();

        startMove(speed_Start);
        SetIsTimer(true);
        timer = 0;
        startTime = Time.time;
    }
    private void Update()
    {
        CheckBallMoveToPlayerDistance();
        timerSystem();

        if(Time.time > startTime + 15f)
        {
            Destroy(gameObject);
        }
    }
    private void timerSystem()
    {
        if(isTimer)
        {
            timer += Time.deltaTime;
        }

        if(timer > StopTime)
        {
            SetIsTimer(false);
            SetIsTimerFinish(true);
        }
    }

    private void startMove(float speed)
    {
        float x = Random.Range(-rotateDeviation, rotateDeviation);
        float y = Random.Range(-rotateDeviation, rotateDeviation);
        float z = Random.Range(-rotateDeviation, rotateDeviation);
        Quaternion rotate = Quaternion.Euler(x, y, z);
        rb.gameObject.transform.rotation = rotate;
        rb.drag = 3;
        rb.AddForce(transform.up * speed,ForceMode.Impulse);
    }

    private void CheckBallMoveToPlayerDistance()
    {
        if (distanceToPlayer() <= movementStartDistance)
        {
            moveToPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out Player player);
            player.DecreaseHealthUntilInit(Energy);
            Destroy(gameObject);
        }
    }

    private void moveToPlayer()
    {
        if(isTimerFinish)
        {
            rb.drag = 0;
            Vector3 Direction = player.transform.position - transform.position;
            rb.velocity = (Direction.normalized * speed);
        }
    }

    private float distanceToPlayer()
    {
        Vector3 distanceVector = player.transform.position - transform.position;
        float distanceToPlayer = distanceVector.magnitude;
        return distanceToPlayer;
    }

    private void SetIsTimer(bool value)
    {
        isTimer = value;
    }
    private void SetIsTimerFinish(bool value)
    {
        isTimerFinish = value;
    }
}
