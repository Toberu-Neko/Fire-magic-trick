using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        FinishAnim();
    }
    private void FinishAnim()
    {
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}