using UnityEngine;

public class NewGamePlay_Basic_Kick : MonoBehaviour
{
    //Script
    private SuperDashKick kick;

    protected virtual void Start()
    {
        kick =GameManager.Instance.EnergySystem.GetComponent<SuperDashKick>();

        //Subscribe
        kick.OnKick += Onkick;
    }
    protected virtual void Onkick() {  }
}
