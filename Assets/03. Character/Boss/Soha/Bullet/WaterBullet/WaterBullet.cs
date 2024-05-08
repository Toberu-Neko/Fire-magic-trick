using System.Threading.Tasks;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    [SerializeField] private GameObject Collider;
    [SerializeField] private float ForwardTime;
    [SerializeField] private float speed;
    [SerializeField] private float TargetPoint;
    [SerializeField] private bool isMove;
    [Header("VFX")]
    [SerializeField] private ParticleSystem vfx_Charge;
    [SerializeField] private ParticleSystem vfx_OnChargeBall;
    [SerializeField] private ParticleSystem vfx_ChageFinishBall;

    private Rigidbody rb;
    private void Start()
    {
        particleSetting();
    }
    private void Update()
    {
        move();
    }
    private void particleSetting()
    {
        var chargemain = vfx_Charge.main;
        chargemain.duration = ForwardTime;

        var onChargeMain = vfx_OnChargeBall.main;
        onChargeMain.startLifetime = ForwardTime;

        var FinishBallMain = vfx_ChageFinishBall.main;
        FinishBallMain.startDelay = ForwardTime;
    }
    private async void startDelay()
    {
        await Task.Delay((int)ForwardTime * 1000);
        isMove = true;
        Collider.SetActive(true);
    }
    private void move()
    {
        if(isMove)
        {
            Vector3 direction = this.transform.forward;
            rb.velocity = direction * speed * Time.deltaTime;
        }
    }
}
