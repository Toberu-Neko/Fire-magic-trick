using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public int damage;
    public float force;

    [Header("Move")]
    [MMReadOnly][SerializeField] private bool isMove;
    [MMReadOnly][SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private GameObject gearObj;
    [SerializeField] private Transform origin;
    [SerializeField] private Transform end;

    private float t = 5;
    

    private void Awake()
    {
        target = end;
    }
    private void Update()
    {
        
    }
    private void moveSystem()
    {

    }
    public void ToMove()
    {

    }
    public void ToStop()
    {

    }
    
    
    
}
