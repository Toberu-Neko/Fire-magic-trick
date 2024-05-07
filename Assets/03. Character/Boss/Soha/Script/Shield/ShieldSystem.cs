using MoreMountains.Tools;
using UnityEngine;

public class ShieldSystem : MonoBehaviour
{
    [SerializeField] private GameObject Shield;

    //
    [SerializeField] private float RecycleTime;
    [SerializeField] [MMReadOnly]private float timer;
    private bool isTimer;

    public delegate void OnRecycleHandler();
    public event OnRecycleHandler OnRecycle;
    private void Update()
    {
        timerSystem();
    }
    private void timerSystem()
    {
        if(isTimer)
        {
            timer += Time.deltaTime;
        }

        if(timer > RecycleTime)
        {
            OnRecycle?.Invoke();
            setIsTimer(false);
        }
    }
    public void OpenShield()
    {
        Shield.SetActive(true);
        timer = 0;
    }
    public void CloseShield()
    {
        Shield.SetActive(false);
        setIsTimer(true);
    }
    public void StarRecycleTimer()
    {
        setIsTimer(true);
        timer = 0;
    }
    private void setIsTimer(bool active)
    {
        isTimer = active;
    }
}
