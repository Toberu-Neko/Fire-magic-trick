using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Playables;

public class TriggerDemoEnd : MonoBehaviour
{
    //Script
    [SerializeField] private PlayableDirector EndingPlayer;
    [SerializeField] private int creditTime = 10;
    private SenceManagerment senceManagerment;

    //variable
    private bool trigger;
    private void Start()
    {
        senceManagerment = GameManager.singleton.GetComponent<SenceManagerment>();
    }
    public async void DemoEnd()
    {
        if(!trigger)
        {
            trigger = true;
            EndingPlayer.Play();
            await Task.Delay(creditTime * 1000);
            senceManagerment.ReStartGame();
        }
    }
}
