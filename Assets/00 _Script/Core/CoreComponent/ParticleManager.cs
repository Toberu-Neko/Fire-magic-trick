using UnityEngine;

public class ParticleManager : CoreComponent
{
    public GameObject StartParticles(GameObject particlePrefab, Vector3 position, Quaternion rotation)
    {
        return ObjectPoolManager.SpawnObject(particlePrefab, position, rotation, ObjectPoolManager.PoolType.ParticleSystem);
    }

    public GameObject StartParticles(GameObject particlePrefab, Vector3 position, Transform ParentObj)
    {
        GameObject obj = ObjectPoolManager.SpawnObject(particlePrefab, position, particlePrefab.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);
        obj.transform.parent = ParentObj;
        return obj;
    }

    public GameObject StartParticles(GameObject particlePrefab, Vector3 position)
    {
        return ObjectPoolManager.SpawnObject(particlePrefab, position, particlePrefab.transform.rotation, ObjectPoolManager.PoolType.ParticleSystem);
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
