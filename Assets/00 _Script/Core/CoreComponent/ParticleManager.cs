using UnityEngine;

public class ParticleManager : CoreComponent
{

    public GameObject StartParticles(GameObject particlePrefab, Vector3 position, Quaternion rotation)
    {
        return ObjectPoolManager.SpawnObject(particlePrefab, position, rotation, ObjectPoolManager.PoolType.ParticleSystem);
    }

    public GameObject StartParticles(GameObject particlePrefab)
    {
        return StartParticles(particlePrefab, transform.position, Quaternion.identity);
    }

    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab)
    {
        var randomRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        return StartParticles(particlePrefab, transform.position, randomRotation);
    }
    public GameObject StartParticlesWithRandomRotation(GameObject particlePrefab, Vector3 position)
    {
        var randomRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        return StartParticles(particlePrefab, position, randomRotation);
    }
}
