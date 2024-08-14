using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvicibleBullet : MonoBehaviour
{
    [SerializeField] private float force = 15f;

    private float startTime;
    private void OnEnable()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if(Time.time - startTime > 3.5f)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit");
            other.TryGetComponent(out IKnockbackable knockbackable);
            knockbackable.Knockback(transform.forward, force, transform.position);
        }
    }
}
