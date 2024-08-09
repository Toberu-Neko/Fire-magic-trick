using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Playables;
using Eflatun.SceneReference;

public class TriggerDemoEnd : MonoBehaviour
{
    //Script
    [SerializeField] private PlayableDirector EndingPlayer;
    [SerializeField] private int creditTime = 10;
    [SerializeField] private SceneReference mainMenuScene;

    //variable
    private bool trigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DemoEnd();
        }
    
    }

    public async void DemoEnd()
    {
        if(!trigger)
        {
            trigger = true;
            EndingPlayer.Play();
            await Task.Delay(creditTime * 1000);

            DataPersistenceManager.Instance.SaveGame();
            LoadSceneManager.Instance.LoadSceneSingle(mainMenuScene.Name);
        }
    }
}
