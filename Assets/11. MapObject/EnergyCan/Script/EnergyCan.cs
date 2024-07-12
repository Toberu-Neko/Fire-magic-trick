using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;

public class EnergyCan : MonoBehaviour ,IDamageable
{
    [SerializeField] private EnemyHealthSystem healthSystem;
    [Header("FireEnergyObject")]
	[SerializeField] private GameObject fireEnergy;

    [Header("EnemyHealth")]
    [SerializeField] private float health;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player Feedbacks_Broken;

    [Header("CheckArea")]
    [SerializeField] private GameObject brokenBoomArea;
    //delegate
    public delegate void OnBrokenHandler();
    public event OnBrokenHandler OnBroke;

    private void Start()
    {
        if(healthSystem ==null)
        {
            healthSystem = this.transform.parent.GetComponent<EnemyHealthSystem>();
        }
        if(healthSystem != null)
        {
            healthSystem.OnEnemyRebirth += OnEnemyRebirth;
        }
    }

    private bool isBroken = false;
    public void Initialization()
    {
        this.gameObject.SetActive(true);
        health = 1;
        isBroken = false;
    }

    public void Broke()
    {
        if(!isBroken)
        {
            isBroken = true;
            Feedbacks_Broken.PlayFeedbacks();
            Instantiate(fireEnergy, transform.position, Quaternion.identity);
            OnBroke?.Invoke();
            DestroyCan();
        }
    }
    private void OnEnemyRebirth()
    {
        Initialization();
    }
    private async void DestroyCan()
    {
        brokenBoomArea.SetActive(true);
        await Task.Delay(250);
        brokenBoomArea.SetActive(false);
        this.gameObject.SetActive(false);
        
    }

    public void Damage(float damageAmount, Vector3 damagePosition, bool trueDamage = false)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Broke();
        }
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
