using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAbsorb : MonoBehaviour
{
    private Vector3 target; // Ŀ������
    public ParticleSystem _particleSystem; // ����ϵ�y
    public float convergenceSpeed = 5f; // �R���ٶ�

    private ParticleSystem.Particle[] particles; // �������

    private void Start()
    {
        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    }

    private void Update()
    {
        Absorb();
        target = GameManager.singleton._input.transform.position;
    }

    private void Absorb()
    {
        int particleCount = _particleSystem.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
            // ʹ�� Lerp ��ʽ׌���ӅR�۵�Ŀ������
            particles[i].position = Vector3.Lerp(particles[i].position, target, convergenceSpeed * Time.deltaTime);
        }

        _particleSystem.SetParticles(particles, particleCount);
    }
}
