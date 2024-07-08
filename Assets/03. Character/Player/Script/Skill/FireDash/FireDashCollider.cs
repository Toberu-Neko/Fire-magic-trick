using MoreMountains.Feedbacks;
using UnityEngine;

public class FireDashCollider : MonoBehaviour
{
    [SerializeField] private MMF_Player HitFeedbacks;

    //Script
    private Basic_AimSupportSystem _aimSupportSystem;
    private PlayerDamage _playerDamage;
    private VibrationController vibrationController;
    private NGP_Dash dash;

    //value
    private float CrashForce;
    private float CrashForceUp;
    private bool IsDash;
    private bool canTriggerDamage;
    private bool isTriggerDamage;

    private void Start()
    {
        dash = GameManager.Instance.NewGamePlay.GetComponent<NGP_Dash>();
        _aimSupportSystem = GameManager.Instance.Player.GetComponent<Basic_AimSupportSystem>();
        _playerDamage = GetComponent<PlayerDamage>();
        vibrationController = GameManager.Instance.GetComponent<VibrationController>();

        Initialization();
    }

    private void Initialization()
    {
        CrashForce = dash.CrashForce;
        CrashForceUp = dash.CrashForceUp;
    }
    private void OnTriggerEnter(Collider other)
    {
        ToDashHitEnemy(other);
        ToHitGlass(other);
    }

    private void OnTriggerStay(Collider other)
    {
        ToDashHitEnemy(other);
        ToHitGlass(other);
    }

    private void ToDashHitEnemy(Collider other)
    {
        if (IsDash)
        {
            if (canTriggerDamage)
            {
                if (other.CompareTag("Enemy"))
                {
                    vibrationController.Vibrate(0.5f, 0.25f);               

                    canTriggerDamage = false;
                    Vector3 direction = transform.parent.transform.forward;
                    Vector3 Enemyup = other.transform.up;

                    if(!isTriggerDamage)
                    {
                        _playerDamage.ToDamageEnemy(other);
                        SetIsTriggerDamage(true);
                    }

                    if(other.TryGetComponent(out EnemyHealthSystem enemy))
                    {
                        enemy.SetAtCrash(true);
                    }

                    _aimSupportSystem.ToAimSupport(other.gameObject, _aimSupportSystem.aimSupportTime);
                    HitFeedbacks.PlayFeedbacks();
                    if(other.GetComponent<AgentController>() != null)
                    {
                        other.GetComponent<AgentController>().DisableAgent(); 
                    }
                    other.GetComponent<Rigidbody>().AddForce(direction * CrashForce + Enemyup * CrashForceUp, ForceMode.Impulse);
                }
            }
        }
    }
    private void ToHitGlass(Collider other)
    {
        if (IsDash)
        {
            if (other.CompareTag("Glass"))
            {
                
                if(other.TryGetComponent(out GlassSystem glass))
                {
                    if (glass.canCrash)
                    {
                        HitFeedbacks.PlayFeedbacks();
                        glass.BrokenCheck_Crash();
                    }
                }
            }
        }
    }

    public void SetIsDash(bool value)
    {
        IsDash = value;
        canTriggerDamage = value;
    }

    public void SetIsTriggerDamage(bool active)
    {
        isTriggerDamage = active;
    }
}
