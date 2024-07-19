using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowerReturn : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    //variable
    private Transform player;

    public enum Type
    {
        Wind,
        Fire,
    }
    public Type type;

    private void Start()
    {
        player = GameManager.Instance.Player.transform;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == Type.Fire)
            {
                other.TryGetComponent(out CardSystem sys);
                sys.AddFireCardEnergy();
            }
            else
            if (type == Type.Wind)
            {
                other.TryGetComponent(out CardSystem sys);
                sys.AddWindCardEnergy();
            }

            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
