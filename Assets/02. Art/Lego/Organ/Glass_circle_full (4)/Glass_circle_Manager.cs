using UnityEngine;

public class Glass_circle_Manager : MonoBehaviour
{
    public GlassSystem[] glassSystem;
    public bool canCrash;
    public bool canEnemyCrash;
    public bool canSuperJump;
    private void OnValidate()
    {
        glassSystem = GetComponentsInChildren<GlassSystem>();
        for(int i =0; i < glassSystem.Length ; i++)
        {
            glassSystem[i].canCrash = canCrash;
            glassSystem[i].canEnemyCrash = canEnemyCrash;
            glassSystem[i].canSuperJump = canSuperJump;
        }
    }
    public void BrokenAll()
    {
        for (int i = 0; i < glassSystem.Length; i++)
        {
            glassSystem[i].BrokenSuperFast();
        }
    }
}
