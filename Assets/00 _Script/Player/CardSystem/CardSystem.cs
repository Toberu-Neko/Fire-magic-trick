using System;
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
    [SerializeField] private float strongShootBulletTimeDuration;
    [SerializeField] private LayerMask aimColliderLayerMask;
    [SerializeField] public Transform debugTransform;
    [SerializeField] private float maxShootDistance;
    [SerializeField] private float minShootDistance;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float cardStateDuration = 5f;
    public bool StrongShoot { get; private set; }

    [Header("Super Dash")]
    [SerializeField] private LayerMask whatIsSuperDashTarget;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float superDashDistance = 10f;
    public Transform SuperDashTarget { get; private set; }
    public bool HasSuperDashTarget { get; private set; }

    private Vector3 targetPosition;
    private Vector2 screenCenterPoint;

    private CardType currentCardType;
    public CardType CurrentEquipedCard { get; private set; }
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
    public event Action<int> OnWindCardEnergyChanged;
    public event Action<int> OnFireCardEnergyChanged;

    private void OnEnable()
    {
        targetPosition = Vector3.zero;
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        currentCardType = CardType.Normal;
        CurrentEquipedCard = CardType.Fire;
        startCardStateTime = 0f;
        endCardStateTime = 0f;
        startShootTime = 0f;

        HasSuperDashTarget = false;
        StrongShoot = false;
    }

    private void Update()
    {
        ShootRay();

        if(currentCardType != CardType.Normal)
        {
            if(Time.time >= endCardStateTime)
            {
                ChangeCard(CardType.Normal);
            }
        }
    }

    #region Shoot

    private void ShootRay()
    {
        Ray ray = player.InputHandler.MainCam.ScreenPointToRay(screenCenterPoint);

        bool shootRaycastHit = Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask);
        bool superDashRaycastHit = Physics.Raycast(ray, out RaycastHit superDashHit, superDashDistance, whatIsSuperDashTarget);

        if (shootRaycastHit)
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
            bool groundHit = Physics.Raycast(ray, superDashHit.distance, whatIsGround);

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
        if (Time.unscaledTime < startShootTime + shootCooldown)
        {
            return;
        }

        startShootTime = Time.unscaledTime;

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

        if (StrongShoot)
        {
            BulletTimeManager.Instance.BulletTime_Slow(strongShootBulletTimeDuration);
        }

    }

    public void SetStrongShoot(bool value)
    {
        StrongShoot = value;
    }
    #endregion


    public void ChangeCard(CardType cardType)
    {
        if(cardType != CardType.Normal)
        {
            CurrentEquipedCard = cardType;
        }

        startCardStateTime = Time.time;
        endCardStateTime = startCardStateTime + cardStateDuration;
        currentCardType = cardType;
    }

    #region Card Energy

    public int CheckCurrentCardEnergy()
    {
        if(windCardEnergy == 0 && fireCardEnergy == 0)
        {
            return 0;
        }

        if(CurrentEquipedCard == CardType.Wind)
        {
            if(windCardEnergy == 0)
            {
                CurrentEquipedCard = CardType.Fire;
                return fireCardEnergy;
            }
            else
            {
                return windCardEnergy;
            }
        }
        else
        {
            if (fireCardEnergy == 0)
            {
                CurrentEquipedCard = CardType.Wind;
                return windCardEnergy;
            }
            else
            { 
                return fireCardEnergy;
            }
        }
    }

    public bool CheckCardEnergy(int amount)
    {
        if (windCardEnergy == 0 && fireCardEnergy == 0)
        {
            return false;
        }

        if (CurrentEquipedCard == CardType.Wind)
        {
            if(windCardEnergy >= amount)
            {
                return true;
            }
            else
            {
                if (fireCardEnergy >= amount)
                {
                    CurrentEquipedCard = CardType.Fire;
                    return true;
                }
            }
        }
        else if (CurrentEquipedCard == CardType.Fire)
        {
            if (fireCardEnergy >= amount)
            {
                return true;
            }
            else
            {
                if (windCardEnergy >= amount)
                {
                    CurrentEquipedCard = CardType.Wind;
                    return true;
                }
            }
        }

        return false;
    }

    public void DecreaseCardEnergy(int amount)
    {
        if(CurrentEquipedCard == CardType.Wind)
        {
            DecreaseWindCardEnergy(amount);
        }
        else if(CurrentEquipedCard == CardType.Fire)
        {
            DecreaseFireCardEnergy(amount);
        }
    }

    public void AddWindCardEnergy()
    {
        windCardEnergy++;
        windCardEnergy = Mathf.Clamp(windCardEnergy, 0, windMaxEnergy);
        OnWindCardEnergyChanged?.Invoke(windCardEnergy);
    }

    public void DecreaseWindCardEnergy(int value)
    {
        windCardEnergy -= value;
        windCardEnergy = Mathf.Clamp(windCardEnergy, 0, windMaxEnergy);
        OnWindCardEnergyChanged?.Invoke(windCardEnergy);
    }

    public void AddFireCardEnergy()
    {
        fireCardEnergy++;
        fireCardEnergy = Mathf.Clamp(fireCardEnergy, 0, fireMaxEnergy);
        OnFireCardEnergyChanged?.Invoke(fireCardEnergy);
    }

    public void DecreaseFireCardEnergy(int value)
    {
        fireCardEnergy -= value;
        fireCardEnergy = Mathf.Clamp(fireCardEnergy, 0, fireMaxEnergy);
        OnFireCardEnergyChanged?.Invoke(fireCardEnergy);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, superDashDistance);
    }
}
