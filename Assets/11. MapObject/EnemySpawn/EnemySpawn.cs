using UnityEngine;
using UnityEngine.Events;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] Enemys;

    [SerializeField] private bool Test_BrokenRightNow;
    //Script
    private ProgressSystem progressSystem;

    //event
    public event MyDelegates.OnHandler OnSpawn;
    public event MyDelegates.OnHandler OnDeath;
    public event MyDelegates.OnHandler OnClearM;
    public UnityEvent OnStart;
    public UnityEvent OnClear;
    //variable
    public bool isClear;
    public bool isSpawned;

    private void Start()
    {
        //Script
        progressSystem = GameManager.Instance.GetComponent<ProgressSystem>();

        //event
        progressSystem.OnPlayerDeath += ToDeath;


        if(Test_BrokenRightNow)
        {
            ToClear();
        }
    }
    private void Update()
    {
        if(!isClear)
        {
            clearCheck();
        }
    }
    public void ToSpawn()
    {
        if(!isClear)
        {
            OnSpawn?.Invoke();
            OnStart?.Invoke();
            setIsSpawned(true);
        }
    }
    public void ToDeath()
    {
        if(!isClear)
        {
            OnDeath?.Invoke();
            setIsSpawned(false);
        }
    }
    public void ToClear()
    {
        OnClearM?.Invoke();
        OnClear?.Invoke();
        setIsClear(true);
    }
    private void clearCheck()
    {
        for(int i = 0; i < Enemys.Length; i++)
        {
            if (Enemys[i] != null)
            {
                if(Enemys[i].activeSelf == true)
                {
                    return;
                }
            }
        }
        ToClear();
    }
    private void setIsClear(bool value)
    {
        isClear = value;
    }
    private void setIsSpawned(bool value)
    {
        isSpawned = value;
    }
}
