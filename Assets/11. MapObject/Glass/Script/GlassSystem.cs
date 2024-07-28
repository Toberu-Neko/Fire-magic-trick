using UnityEngine;
using System.Threading.Tasks;
using MoreMountains.Feedbacks;

public class GlassSystem : MonoBehaviour, IDamageable
{
    public enum Mode
    {
        Fast,
        Delay,
        unlimited
    }
    public Mode mode;
    [Header("UniversalFeedbacks")]
    [SerializeField] private MMF_Player feedbacks_Broken;
    [Header("FastMode")]
    [SerializeField] private float fastTime;
    [Header("DelayMode")]
    [SerializeField] private MMF_Player feedbacks_Delay;
    [SerializeField] private float delayTime;
    [Header("Crash")]
    [SerializeField] public bool canCrash;
    [Header("EnemyCrash")]
    [SerializeField] public bool canEnemyCrash;
    [Header("SuperJump")]
    [SerializeField] public bool canSuperJump;
    [SerializeField] public bool jumpDelayMode;

    private Collider glassCollider;
    private MeshRenderer glassRender;
    private bool isBroken;
    private bool isBrokenFoever;

    private void Awake()
    {
        glassCollider = GetComponent<Collider>();
        glassRender = GetComponent<MeshRenderer>();
        GameManager.Instance.OnPlayerReborn += GlassRebirth;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerReborn -= GlassRebirth;
    }

    public void BrokenCheck_Crash()
    {
        if (canCrash)
        {
            BrokenSuperFast();
        }
    }

    private void BrokenCheck_EnemyCrash(Collision collision)
    {
        if(canEnemyCrash)
        {
            if(collision.collider.CompareTag("Enemy"))
            {
                EnemyHealthSystem enemy = collision.collider.GetComponent<EnemyHealthSystem>();
                if(enemy.atCrash)
                {
                    BrokenSuperFast();
                }
            }
        }
    }
    public void Broken()
    {
        if(!isBroken && mode != Mode.unlimited)
        {
            switch (mode)
            {
                case Mode.Fast:
                    fastMode();
                    break;
                case Mode.Delay:
                    delayMode();
                    break;
            }

            SetIsBroken(true);
        }
    }
    public void  BrokenSuperFast()
    {
        SetGlass(false);
    }
    public void GlassRebirth()
    {
        if(!isBrokenFoever)
        {
            SetIsBroken(false);
            SetGlass(true);
        }
    }
    private async void fastMode()
    {
        await Task.Delay((int)(fastTime * 1000));
        SetGlass(false);

    }
    private async void delayMode()
    {
        feedbacks_Delay.PlayFeedbacks();
        await Task.Delay((int)(delayTime * 1000));
        SetGlass(false);
    }

    private void SetGlass(bool active)
    {
        // Debug.Log("SetGlass + " + active);
        SetCollider(active);
        SetColliderRender(active);

        if(!active)
        {
            feedbacks_Broken.PlayFeedbacks();
        }
    }
    public void QuickSetGlassFalse()
    {
        SetCollider(false);
        SetColliderRender(false);
    }
    private void SetIsBroken(bool active)
    {
        isBroken = active;
    }
    private void SetCollider(bool active)
    {
        if(glassCollider != null)
        {
            glassCollider.enabled = active;
        }
        else
        {
            Debug.LogError("Glass Collider == null in " + gameObject.name);
        }
    }
    private void SetColliderRender(bool active)
    {
        if(glassRender != null)
        {
            glassRender.enabled = active;
        }
        else
        {
            Debug.LogError("Glass Render == null in " + gameObject.name);
        }
    }
    private void SetCanSuperJump(bool active)
    {
        canSuperJump = active;
    }

    public void Damage(float damageAmount, Vector3 damagePosition, bool trueDamage = false)
    {
        if(!canCrash)
        {
            return;
        }
        BrokenSuperFast();
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}