using MoreMountains.Feedbacks;
using UnityEngine;
using System.Threading.Tasks;

public class HealthSystem : MonoBehaviour
{
    [Header("Feedbacks")]
    [SerializeField] private MMF_Player Feedbacks;
    [Header("Invicible")]
    [SerializeField] private float HitInvicibleTime = 0.5f;
    
    //Script
    private EnergySystem energySystem;
    private ImpactReceiver impactReceiver;
    private VibrationController vibrationController;

    //value
    public bool Invincible;

    private void Start()
    {
        energySystem = GameManager.Instance.Player.GetComponent<EnergySystem>();
        impactReceiver = GameManager.Instance.Player.GetComponent<ImpactReceiver>();
        vibrationController = GameManager.Instance.GetComponent<VibrationController>();
    }
    public void ToPushPlayer(Vector3 ImpactDirection)
    {
        toPushPlayer(ImpactDirection);
    }
    public void ToDamagePlayer(int Damage)
    {
        if(!Invincible)
        {
            DamagePlayer(Damage);
            vibrationController.Vibrate(0.5f, 0.25f);
        }
    }
    public void ToDamagePlayer(int Damage,Vector3 ImpactDirection)
    {
        if(!Invincible)
        {
            DamagePlayer(Damage, ImpactDirection);
            vibrationController.Vibrate(0.5f, 0.25f);
        }
    }
    private void toPushPlayer(Vector3 ImpactDirection)
    {
        ToImpactPlayer(ImpactDirection);
    }
    private void DamagePlayer(int Damage)
    {
        /* old system
        energySystem.DecreaseEnergy(Damage);
        ToInvincible();
        PlayFeedbacks();
        */

        energySystem.GetEnergyByDamage(Damage);
        PlayFeedbacks();
        ToInvincible();
    }
    private void DamagePlayer(int Damage, Vector3 ImpactDirection)
    {
        DamagePlayer(Damage);
        ToImpactPlayer(ImpactDirection);
    }
    public void ToImpactPlayer(Vector3 Direction)
    {
        impactReceiver.AddImpact(Direction);
    }
    private void PlayFeedbacks()
    {
        Feedbacks.PlayFeedbacks();
    }
    public async void ToInvincible()
    {
        SetInvincible(true);
        await Task.Delay((int)(HitInvicibleTime*1000));
        SetInvincible(false);
    }
    public void SetStoryInvincible(bool active)
    {
        Invincible = active;
    }
    private void SetInvincible(bool Active)
    {
        Invincible = Active;
    }
}
