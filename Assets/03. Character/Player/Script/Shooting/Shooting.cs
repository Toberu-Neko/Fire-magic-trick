using MoreMountains.Feedbacks;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Shooting_Magazing _shooting_magazing;
    private Shooting_Check shootingCheck;
    [Header("Shoot Setting")]
    [SerializeField] private Transform spawnBulletPositionOrigin;
    [SerializeField] private Transform spawnBulletPosition;
    [Header("Feedbacks")]
    [SerializeField] private MMF_Player feedbacks_NoBullet;
    private void Start()
    {
        shootingCheck = GetComponent<Shooting_Check>();
        _shooting_magazing = GetComponent<Shooting_Magazing>();
    }
    public void Shoot(Transform preferb)
    {
        SpawnBulletPositionToNew();

        if (_shooting_magazing.enabled == true)
        {
            if (_shooting_magazing.Bullet <= 0)
            {
                feedbacks_NoBullet.PlayFeedbacks();
                return;
            }

            _shooting_magazing.UseBullet();
        }

        Vector3 aimDir = (shootingCheck.mouseWorldPosition - spawnBulletPosition.position).normalized;
        Instantiate(preferb, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

        
    }
    private void SpawnBulletPositionToNew()
    {
        Vector3 Direction = (shootingCheck.mouseWorldPosition - spawnBulletPositionOrigin.position).normalized;
        spawnBulletPositionOrigin.transform.forward = new Vector3(Direction.x, 0, Direction.z);
    }
}
