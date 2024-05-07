using MoreMountains.Feedbacks;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnergyCan : MonoBehaviour ,IHealth
{
    [SerializeField] private EnemyHealthSystem healthSystem;
    [Header("FireEnergyObject")]
	[SerializeField] private GameObject fireEnergy;

    [Header("EnemyHealth")]
    [SerializeField] private int health;
    private int originHealth;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player Feedbacks_Broken;

    [Header("CheckArea")]
    [SerializeField] private GameObject brokenBoomArea;

    private ProgressSystem progressSystem;
    //delegate
    public delegate void OnBrokenHandler();
    public event OnBrokenHandler OnBroke;

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

        originHealth = health;
    }

    private bool isBroken = false;
    public int iHealth
    {
        get { return health; }
        set { health = value; }
    }
    public void Initialization()
    {
        this.gameObject.SetActive(true);
        health = 1;
        setIsBroken(false);
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
        Initialization();
    }
    private async void DestroyCan()
    {
        brokenBoomArea.SetActive(true);
        await Task.Delay(250);
        brokenBoomArea.SetActive(false);
        this.gameObject.SetActive(false);
        OnBroke?.Invoke();
    }
    private void setIsBroken(bool value)
    {
        isBroken = value;
    }
}
