using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject normalCardPrefab;
    [SerializeField] private GameObject windCardPrefab;
    [SerializeField] private GameObject fireCardPrefab;

    [Header("Spawn Position")]
    [SerializeField] private Transform frontSpawnPos;
    [SerializeField] private Transform backSpawnPos;

    [Header("Shoot Check")]
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] public Transform debugTransform;
    [SerializeField] private float maxShootDistance;
    [SerializeField] private float minShootDistance;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float cardStateDuration = 5f;

    [Header("Super Dash")]
    [SerializeField] private LayerMask whatIsSuperDashTarget;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float superDashDistance = 10f;
    public Transform SuperDashTarget { get; private set; }
    public bool HasSuperDashTarget { get; private set; }

    private Vector3 targetPosition;
    private Vector2 screenCenterPoint;

    private CardType currentCardType;
    public enum CardType
    {
        Normal,
        Wind,
        Fire
    }

    private float startCardStateTime;
    private float endCardStateTime;

    private float startShootTime;

    [SerializeField] private int windMaxEnergy;
    [SerializeField] private int fireMaxEnergy;
    private int windCardEnergy;
    private int fireCardEnergy;

    private void OnEnable()
    {
        targetPosition = Vector3.zero;
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        currentCardType = CardType.Normal;
        startCardStateTime = 0f;
        endCardStateTime = 0f;
        startShootTime = 0f;

        HasSuperDashTarget = false;
    }

    private void Update()
    {
        ShootRay();

        if(currentCardType != CardType.Normal)
        {
            if(Time.time >= endCardStateTime)
            {
                currentCardType = CardType.Normal;
            }
        }
    }

    private void ShootRay()
    {
        Ray ray = player.InputHandler.MainCam.ScreenPointToRay(screenCenterPoint);

        bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask);
        bool superDashRaycastHit = Physics.Raycast(ray, out RaycastHit superDashHit, superDashDistance, whatIsSuperDashTarget);

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

        if (superDashRaycastHit)
        {
            bool groundHit = Physics.Raycast(ray, out RaycastHit groundHitOut, superDashHit.distance, whatIsGround);

            if (!groundHit)
            {
                HasSuperDashTarget = true;
                SuperDashTarget = superDashHit.transform;
            }
            else
            {
                HasSuperDashTarget = false;
                SuperDashTarget = null;
            }
        }
        else
        {
            HasSuperDashTarget = false;
            SuperDashTarget = null;
        }
    }

    public void Shoot()
    {
        if (Time.time < startShootTime + shootCooldown)
        {
            return;
        }

        startShootTime = Time.time;

        Vector3 aimDir = (targetPosition - frontSpawnPos.position).normalized;

        if(Vector3.Distance(targetPosition, frontSpawnPos.position) < minShootDistance)
        {
            aimDir = transform.forward;
        }

        player.Anim.SetTrigger("shoot");
        // Debug.Log(transform.forward - player.InputHandler.MainCam.transform.forward);

        switch (currentCardType)
        {
            case CardType.Normal:
                Instantiate(normalCardPrefab, frontSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
                break;
            case CardType.Wind:
                Instantiate(windCardPrefab, frontSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
                break;
            case CardType.Fire:
                Instantiate(fireCardPrefab, frontSpawnPos.position, Quaternion.LookRotation(aimDir, Vector3.up));
                break;
        }
        
    }

    public void ChangeCard(CardType cardType)
    {
        if(currentCardType == cardType)
        {
            return;
        }

        startCardStateTime = Time.time;
        endCardStateTime = startCardStateTime + cardStateDuration;
        currentCardType = cardType;
    }

    public void AddWindCardEnergy()
    {
        windCardEnergy++;
        Mathf.Clamp(windCardEnergy, 0, windMaxEnergy);
    }

    public void AddFireCardEnergy()
    {
        fireCardEnergy++;
        Mathf.Clamp(fireCardEnergy, 0, fireMaxEnergy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, superDashDistance);
    }
}
