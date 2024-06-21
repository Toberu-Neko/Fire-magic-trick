using UnityEngine;

public class CardCircle : MonoBehaviour
{
    private Transform player;
    private void Start()
    {
        player = GameManager.Instance.Player;
        SetParent();
    }
    private void SetParent()
    {
        this.transform.SetParent(player);
    }
}
