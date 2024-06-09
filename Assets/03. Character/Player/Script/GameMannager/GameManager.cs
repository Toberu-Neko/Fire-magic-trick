using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager singleton = null;
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

    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }
}
