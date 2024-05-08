using System.Threading.Tasks;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    [SerializeField] private float ForwardTime;
    [SerializeField] private float speed;
    [SerializeField] private bool isMove;
    [Header("VFX")]
    [SerializeField] private ParticleSystem vfx_Charge;
    [SerializeField] private ParticleSystem vfx_OnChargeBall;
    [SerializeField] private ParticleSystem vfx_ChageFinishBall;

    private Collider coli;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coli = GetComponent<Collider>();
        particleSetting();
    }
    private void Start()
    {
        startDelay();
        coli.enabled = false;
    }
    private void Update()
    {
        move();
    }
    private void particleSetting()
    {
        var chargemain = vfx_Charge.main;
        vfx_Charge.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
        coli.enabled = true ;
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
