using UnityEngine;

public class TriggerArea_SuperPumber : MonoBehaviour
{
    //Script
    private Pumber pumber;
    private DeathSystem deathSystem;
    private ImpactReceiver impactReceiver;
    //Varable
    private bool isDeathPumber;
    private float SuperPumberForce;



    private void Awake()
    {
        pumber = GetComponentInParent<Pumber>();
        isDeathPumber = pumber.isDeathPumber;
        SuperPumberForce = pumber.SuperPumberForce;
    }
    private void Start()
    {
        deathSystem = GameManager.Instance.UISystem.GetComponent<DeathSystem>();
        impactReceiver = GameManager.Instance.Player.GetComponent<ImpactReceiver>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(isDeathPumber)
            {
                deathSystem.Death_Fast();
                return;
            }

            Vector3 direction = this.transform.up;
            Vector3 force = direction * SuperPumberForce;
            impactReceiver.AddImpact(force);
        }
    }
}
