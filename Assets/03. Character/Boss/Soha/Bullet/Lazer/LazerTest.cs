using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LazerTest : MonoBehaviour
{
    [SerializeField] private float forwardDelay;
    [SerializeField] private float keepTime;
    [SerializeField] private Collider coli;
    [Header("VFX")]
    [SerializeField] private ParticleSystem PowerCharge;
    [SerializeField] private ParticleSystem PowerBall;
    [SerializeField] private ParticleSystem AttackCharge;
    [SerializeField] private ParticleSystem Lazer;
    private void Start()
    {
        coli.enabled = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            ChargePower(); // �W�O����
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            LazerAttack();  //�p�g����
        }
    }
    public async void ChargePower()
    {
        PowerBall.Play();
        PowerCharge.Play();

        await Task.Delay(3000);
        LazerAttack();
    }
    public async void LazerAttack()
    {
        AttackCharge.Play();
        PowerBall.Play();
        await Task.Delay((int)forwardDelay * 1000); //��ڹB�ή��קK�� await
        coli.enabled = true;
        Lazer.Play();
        await Task.Delay((int)keepTime * 1000);
        coli.enabled = false;
        Lazer.Stop();
        PowerBall.Stop();
    }
}
