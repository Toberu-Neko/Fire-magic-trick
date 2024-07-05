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

        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
    }
}
