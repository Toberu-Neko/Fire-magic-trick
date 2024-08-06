using UnityEngine;
using UnityEngine.VFX;
using System.Threading.Tasks;
using UnityEditor;
using MoreMountains.Feedbacks;

public class Satun_Laser_New : MonoBehaviour
{
    [SerializeField] private Collider damageCollider;
    [SerializeField] private ParticleSystem VFX_Aiming;
    [SerializeField] private ParticleSystem VFX_Laser;

    private ParticleSystem[] particle;
    private VisualEffect vfx;

    private float timer;
    private bool isTimer;
    private void Awake()
    {
        vfx = GetComponentInChildren<VisualEffect>();
        particle = GetComponentsInChildren<ParticleSystem>();
    }
    private void Start()
    {
        ActiveParticle(false);
    }
    private void Update()
    {
        timerSystem();
    }
    public void PlayLaser()
    {
        isTimer = true;
        timer = 0;

        VFX_Aiming.Play();
    }
    private void timerSystem()
    {
        if(isTimer)
        {
            timer += Time.deltaTime;
        }
        if(timer> 3.5f)
        {
            VFX_Laser.Play();
            vfx.Play();
            Laser();
            isTimer = false;
        }
    }
    
    public void active(bool state)
    {
        ActiveParticle(state);
    }
    private async void Laser()
    {
        damageCollider.gameObject.SetActive(true);
        await Task.Delay(250);
        damageCollider.gameObject.SetActive(false);
    }
    private void ActiveParticle(bool active)
    {
        for (int i = 0; i < particle.Length; i++)
        {
            if (active)
            {
                particle[i].Play();
            }
            else
            {
                particle[i].Stop();
            }
        }
    }
}
