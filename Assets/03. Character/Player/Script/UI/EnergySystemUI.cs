using MoreMountains.Tools;
using System.Threading.Tasks;
using UnityEngine;

public class EnergySystemUI : MonoBehaviour
{
    [SerializeField] private MMProgressBar MMProgressBar;
    [SerializeField] private MMProgressBar OverBurning;

    private float test = 0;

    private void Start()
    {
        Initialization();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            test += 0.1f;
            if (test >= 1) test = 1;
            UpdateBarOverBurning(test);
            Debug.Log("J");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            test -= 0.1f;
            if (test<=0) test = 0;
            UpdateBarOverBurning(test);
            Debug.Log("K");
        }
    }
    private async void Initialization()
    {
        await Task.Delay(250);
        UpdateBarOverBurning(0);
    }
    public void UpdateBar(float value)
    {
        if (MMProgressBar != null)
        {
            MMProgressBar.UpdateBar01(value);
        }
    }
    public void UpdateBarOverBurning(float value)
    {
        if(OverBurning!=null)
        {
            OverBurning.UpdateBar01(value);
        }
    }
}
