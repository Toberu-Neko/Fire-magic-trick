using UnityEngine;

public class SuperJump : MonoBehaviour
{
    /*
    [SerializeField] private ParticleSystem VFX_SuperJumpStart;
    [SerializeField] private float Force = 10f;

    //Script
    private PlayerDamage playerDamage;
    private VibrationController vibrationController;

    private void Start()
    {
        playerDamage = GetComponent<PlayerDamage>();
        vibrationController = GameManager.Instance.GetComponent<VibrationController>();
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Glass"))
        {
            GlassSystem glass = other.GetComponent<GlassSystem>();
            if(glass!=null)
            {
                glass.BrokenCheck_SuperJump();
            }
        }

        if (other.CompareTag("Enemy"))
        {
            playerDamage.ToDamageEnemy(other);
            vibrationController.Vibrate(0.5f, 0.25f);

            Vector3 dir = (other.transform.position - transform.position).normalized;
            Rigidbody rb = other.GetComponent<Rigidbody>();

            rb.AddForce(dir * Force, ForceMode.Impulse);
        }
    }
    */
}
