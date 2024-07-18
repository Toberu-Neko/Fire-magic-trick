using System;
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
    public event Action OnPlayerReborn;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        Application.targetFrameRate = -1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        BulletTimeManager.Instance.TimeScaleZero();
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        BulletTimeManager.Instance.TimeScaleOne();
    }

    public void PlayerReborn()
    {
        OnPlayerReborn?.Invoke();
    }
}
