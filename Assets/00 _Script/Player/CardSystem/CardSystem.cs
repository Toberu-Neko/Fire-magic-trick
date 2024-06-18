using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    [SerializeField] private Player player;
    [field: SerializeField] public GameObject NormalCardPrefab { get; private set; }
    [field: SerializeField] public GameObject WindCardPrefab { get; private set; }
    [field: SerializeField] public GameObject FireCardPrefab { get; private set; }

    [Header("Spawn Position")]
    [SerializeField] private Transform frontSpawnPos;
    [SerializeField] private Transform backSpawnPos;


    [Header("Shoot Check")]
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] public Transform debugTransform;
    [SerializeField] private float maxShootDistance;
    [SerializeField] private float minShootDistance;

    private Vector3 targetPosition;
    private Vector2 screenCenterPoint;

    private int windCardEnergy;
    private int fireCardEnergy;

    private void Awake()
    {
        targetPosition = Vector3.zero;
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    private void Update()
    {
        ShootRay();
    }

    private void ShootRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask);

        if (raycastHit)
        {
            debugTransform.position = hit.point;
            targetPosition = hit.point;
        }
        else
        {
            debugTransform.position = ray.origin + ray.direction * maxShootDistance;
            targetPosition = debugTransform.position;
        }
    }

    public void Shoot(GameObject prefab = null)
    {
        Vector3 aimDir = (targetPosition - frontSpawnPos.position).normalized;

        if(Vector3.Distance(targetPosition, frontSpawnPos.position) < minShootDistance)
        {
            aimDir = transform.forward;
        }

        player.Anim.SetTrigger("shoot");
        //TODO: Decide which card to shoot in this script
        // Debug.Log(transform.forward - player.InputHandler.MainCam.transform.forward);
        Instantiate(NormalCardPrefab, frontSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
}
