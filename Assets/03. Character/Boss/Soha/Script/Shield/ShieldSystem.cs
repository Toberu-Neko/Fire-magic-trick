using UnityEngine;

public class ShieldSystem : MonoBehaviour
{
    [SerializeField] private GameObject Shield;
    public void OpenShield()
    {
        Shield.SetActive(true);
    }
    public void CloseShield()
    {
        Shield.SetActive(false);
    }

}
