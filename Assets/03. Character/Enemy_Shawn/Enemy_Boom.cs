using UnityEngine;

public class Enemy_Boom : MonoBehaviour
{
    [SerializeField] private EnemyHealthSystem enemyHealthSystesm;
    [HideInInspector]private Rigidbody rb;
    private void Awake()        
    {
        rb = enemyHealthSystesm.GetComponent<Rigidbody>();
    }
    public void Boom()
    {
        enemyHealthSystesm.Boom = true;
    }
}
