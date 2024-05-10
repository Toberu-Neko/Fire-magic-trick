using UnityEngine;

public class GearsManager : MonoBehaviour
{
    private Gear[] gears;

    private void Awake()
    {
        gears =GetComponentsInChildren<Gear>();
    }
    public void ToMove()
    { 
        for (int i = 0; i < gears.Length; i++)
        {
            gears[i].ToMove();
        }
    }
    public void ToStop()
    {
        for (int i = 0; i < gears.Length; i++)
        {
            gears[i].ToStop();
        }
    }
}
