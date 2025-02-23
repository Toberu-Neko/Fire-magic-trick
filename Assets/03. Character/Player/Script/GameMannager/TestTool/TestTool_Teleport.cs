using Unity.VisualScripting;
using UnityEngine;

public class TestTool_Teleport : MonoBehaviour
{
    [SerializeField] private Transform[] point;

    private Transform player;

    private void Start()
    {
        player = GameManager.Instance.Player;

    }
    private void Update()
    {
        if(point != null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                TeleportToPoint(point[0].transform);
            }
            if(Input.GetKeyDown(KeyCode.Alpha9)) 
            {
                TeleportToPoint(point[1].transform);
            }
            if(Input.GetKeyDown(KeyCode.Alpha8))
            {
                TeleportToPoint(point[2].transform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                TeleportToPoint(point[3].transform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                TeleportToPoint(point[4].transform);
            }
            if(Input.GetKeyDown (KeyCode.Alpha4))
            {
                TeleportToPoint(point[5].transform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TeleportToPoint(point[6].transform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TeleportToPoint(point[7].transform);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TeleportToPoint(point[8].transform);
            }
        }
    }
    private void TeleportToPoint(Transform transform)
    {
        player.transform.position = transform.position;
    }
}
