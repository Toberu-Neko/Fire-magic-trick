using MoreMountains.Feedbacks;
using UnityEngine;

public class TPDashCollider : MonoBehaviour, ITriggerNotifier
{
    [SerializeField] GameObject VFX_CardHit;
    [SerializeField] private float bulletTime = 0.5f;
    [SerializeField] private MMF_Player HitEnemy;
    
    //delegate
    public event MyDelegates.OnTriggerHandler OnTrigger;

    //Script
    private BulletTime BulletTime;
    private VibrationController vibrationController;

    private void Start()
    {
        BulletTime = GameManager.Instance.GetComponent<BulletTime>();
        vibrationController = GameManager.Instance.GetComponent<VibrationController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            vibrationController.Vibrate(0.5f, 0.25f);

            OnTrigger?.Invoke(other);
            HitEnemy.PlayFeedbacks();
            BulletTime.BulletTime_Slow(bulletTime);
            GameObject vfx = Instantiate(VFX_CardHit, transform.position, Quaternion.identity);
            Destroy(vfx, 1.5f);
        }
    }
}
