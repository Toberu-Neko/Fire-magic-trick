using MagicaCloth2;
using UnityEngine;

public class NGP_Shot : NGP_Basic_Shot
{
    [Space(20)]
    [Header("Bullet")]
    [SerializeField] private Transform normalCard;
    [SerializeField] private Transform boomCard;
    [SerializeField] private Transform fireCard;
    [SerializeField] private Transform windCard;
    [SerializeField] private Transform MoneyCard;

    [Header("Shot")]
    [SerializeField] private float shotCoolingTime;

    //Script
    private Shooting_Normal shooting_Normal;
    private NewGamePlay_Combo combo;
    private Shooting_Magazing magazing;

    //Variable
    private bool isShot;
    private float timer;
    public enum ShotType
    {
        Normal,
        Boom,
        Fire,
        Wind,
        Money
    }
    public ShotType shotType;
    private void Awake()
    {
        combo = GetComponent<NewGamePlay_Combo>();
    }
    public override void Start()
    {
        base.Start();
        shooting_Normal = GameManager.Instance.ShootingSystem.GetComponent<Shooting_Normal>();
        magazing = GameManager.Instance.ShootingSystem.GetComponent<Shooting_Magazing>();
    }
    public override void Update()
    {
        base.Update();
        shotTimer();
    }
    private void shotTimer()
    {
        if (isShot)
        {
            timer += Time.deltaTime;
        }

        if (timer >= shotCoolingTime)
        {
            SetIsShot(false);
            timer = 0;
        }
    }
    public void Normal_Shot()
    {
        if (!isShot)
        {
            Shot(0f);
            SetIsShot(true);
        }
        else
        {
            //is Coolling.
        }
    }
    private bool canShot()
    {
        if (magazing.isReload || magazing.Bullet == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Shot()
    {
        if (canShot())
        {
            return;
        }
        Shot(normalCard);
        shooting_Normal.PlayShootFeedbacks();
    }
    public void Shot(ShotType shotType)
    {
        if (canShot())
        {
            return;
        }
        Transform preferb = ChooseBullet(shotType);
        Shot(preferb);
        shooting_Normal.PlayShootFeedbacks();
    }
    public void Shot(float rotate_x)
    {
        if (canShot())
        {
            return;
        }
        Shot(normalCard, rotate_x);
        shooting_Normal.PlayShootFeedbacks();
    }
    public void Shot(float rotate_x, ShotType shotType)
    {
        if (canShot())
        {
            return;
        }
        Transform preferb = ChooseBullet(shotType);
        Shot(preferb, rotate_x);
        shooting_Normal.PlayShootFeedbacks();
    }
    public void Shot(float rotate_x, float rotate_y)
    {
        if (canShot())
        {
            return;
        }
        Shot(normalCard, rotate_x, rotate_y);
        shooting_Normal.PlayShootFeedbacks();
    }
    public void Shot(float rotate_x, float rotate_y, ShotType shotType)
    {
        if (canShot())
        {
            return;
        }
        Transform preferb = ChooseBullet(shotType);
        Shot(preferb, rotate_x, rotate_y);
        shooting_Normal.PlayShootFeedbacks();
    }
    public void Shot(Vector3 positionOffset, float rotate_x, float rotate_y, ShotType shotType)
    {
        if (canShot())
        {
            return;
        }
        Transform preferb = ChooseBullet(shotType);
        Shot(preferb, positionOffset, rotate_x, rotate_y);
        shooting_Normal.PlayShootFeedbacks();
    }
    private void SetIsShot(bool value)
    {
        this.isShot = value;
    }
    private Transform ChooseBullet(ShotType shotType)
    {
        Transform shotPreferb = null;

        switch (shotType)
        {
            case ShotType.Normal:
                shotPreferb = this.normalCard;
                break;

            case ShotType.Boom:
                shotPreferb = boomCard;
                break;

            case ShotType.Fire:
                shotPreferb = fireCard;
                break;

            case ShotType.Wind:
                shotPreferb = windCard;
                break;

            case ShotType.Money:
                shotPreferb = MoneyCard;
                break;
        }

        return shotPreferb;
    }
    public void SetShotType(ShotType shotType)
    {
        this.shotType = shotType;
    }
}
