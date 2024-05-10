using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearDamage : MonoBehaviour
{
    private Gear gear;

    private int damage;
    private float force;
    private Transform center;
    private float t = 5;
    private void Awake()
    {
        gear = this.transform.parent.GetComponent<Gear>();
        damage = gear.damage;
        force = gear.force;
        center = this.transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthSystem>().ToDamagePlayer(damage);
            HitPlayer(other);
        }
    }
    private void HitPlayer(Collider other)
    {
        Transform Player = other.transform;
        Vector3 direction = (Player.position - center.position).normalized;
        Vector3 force = direction * this.force * t;
        other.GetComponent<ImpactReceiver>().AddImpact(force);
    }
}
