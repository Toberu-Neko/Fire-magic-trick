using UnityEngine;

public class OrganCan : MonoBehaviour
{
    [SerializeField] private EnergyCan can;

    private void Start()
    {
        can.OnBroke += onBroke;
    }
    private void Initialization()
    {
        can.Initialization();
    }
    private void onBroke()
    {
    }
}
