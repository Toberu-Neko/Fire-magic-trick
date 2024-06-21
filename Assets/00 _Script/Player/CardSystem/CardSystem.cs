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

    private Vector3 targetPosition;
    private Vector2 screenCenterPoint;

    private CardType currentCardType;
    public enum CardType
    {
        Normal,
        Wind,
        Fire
    }

    [SerializeField] private float cardStateDuration = 5f;
    private float startCardStateTime;
    private float endCardStateTime;

    private float startShootTime;

    private int windCardEnergy;
    private int fireCardEnergy;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        targetPosition = Vector3.zero;
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        currentCardType = CardType.Normal;
        startCardStateTime = 0f;
        endCardStateTime = 0f;
        startShootTime = 0f;
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
        //TODO: Decide which card to shoot in this script
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
}
