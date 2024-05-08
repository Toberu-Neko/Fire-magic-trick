using UnityEngine;

public class BossSystem_Soha : MonoBehaviour
{
    //Script
    private Soha soha;
    private Boss_System system;
    private void Awake()
    {
        soha = GetComponent<Soha>();
        system = GetComponent<Boss_System>();
    }
}
