using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using UnityEngine;

public class EnergyCan : MonoBehaviour ,IHealth
{
    [SerializeField] private EnemyHealthSystem healthSystem;
    [Header("FireEnergyObject")]
	[SerializeField] private GameObject fireEnergy;

    [Header("EnemyHealth")]
    [SerializeField] private int health;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player Feedbacks_Broken;

    [Header("CheckArea")]
    [SerializeField] private GameObject brokenBoomArea;

    private ProgressSystem progressSystem;

    private void Start()
    {
        progressSystem = GameManager.singleton.GetComponent<ProgressSystem>();

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
    public int iHealth
    {
        get { return health; }
        set { health = value; }
    }

    public void TakeDamage(int damage , PlayerDamage.DamageType damageType)
    {
        health -= damage;

        if(health <= 0)
        {
            Broke(); 
        }
    }

    public void Broke()
    {
        if(!isBroken)
        {
            setIsBroken(true);
            Feedbacks_Broken.PlayFeedbacks();
            Instantiate(fireEnergy, transform.position, Quaternion.identity);
            
            DestroyCan();
        }
    }
    private void OnEnemyRebirth()
    {
        this.gameObject.SetActive(true);
        health = 1;
        setIsBroken(false);
    }
    private async void DestroyCan()
    {
        brokenBoomArea.SetActive(true);
        await Task.Delay(250);
        brokenBoomArea.SetActive(false);
        this.gameObject.SetActive(false);
    }
    private void setIsBroken(bool value)
    {
        isBroken = value;
    }
}
