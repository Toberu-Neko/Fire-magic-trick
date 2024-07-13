using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ControllerInput _input;
    public PlayerState _playerState;
    public GameObject UISystem;
    public GameObject EnergySystem;
    public GameObject ShootingSystem;
    public Transform Player;
    public VFX_List VFX_List;
    public Feedbacks_List Feedbacks_List;
    public Collider_List Collider_List;
    public GameObject NewGamePlay;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Application.targetFrameRate = -1;
        ResumeGame();

    }

    public void PauseGame()
    {
        IsPaused = true;
        BulletTimeManager.Instance.TimeScaleZero();
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        BulletTimeManager.Instance.TimeScaleOne();
        Cursor.lockState = CursorLockMode.Locked;
    }
}
