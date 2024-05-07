using UnityEngine;

public class OrganCan : MonoBehaviour
{
    [SerializeField] private EnergyCan can;
    [SerializeField] private OrganStar star;

    private void Start()
    {
        can.OnBroke += onBroke;
    }
    public void Initialization()
    {
        can.Initialization();
    }
    private void onBroke()
    {
        if(star != null)
        {
            star.Catapult();
        }
    }
}
