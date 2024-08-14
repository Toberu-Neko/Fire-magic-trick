using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float speed;
    [SerializeField] private Transform fireTransform;


    public void Fire()
    {
        GameObject bullet = ObjectPoolManager.SpawnObject(bulletPrefab, fireTransform.position, fireTransform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = fireTransform.forward * speed;
    }
}
