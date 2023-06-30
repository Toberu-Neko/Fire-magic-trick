using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGroundAbsorb : MonoBehaviour
{
    public Transform target; // Ŀ������
    public ParticleSystem _particleSystem; // ����ϵ�y
    public float convergenceSpeed = 5f; // �R���ٶ�

    private ParticleSystem.Particle[] particles; // �������

    private void Start()
    {
        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    }

    private void Update()
    {
        int particleCount = _particleSystem.GetParticles(particles);

        for (int i = 0; i < particleCount; i++)
        {
            // ʹ�� Lerp ��ʽ׌���ӅR�۵�Ŀ������
            particles[i].position = Vector3.Lerp(particles[i].position, target.position, convergenceSpeed * Time.deltaTime);
        }

        _particleSystem.SetParticles(particles, particleCount);
    }
}
