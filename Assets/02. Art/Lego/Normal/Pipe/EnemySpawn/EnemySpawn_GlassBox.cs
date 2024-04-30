using UnityEngine;

public class EnemySpawn_GlassBox : MonoBehaviour
{
    public EnemySpawn_Pipe[] pipes;

    private void Start()
    {
        pipes = GetComponentsInChildren<EnemySpawn_Pipe>();
    }
    private void Update()
    {
        
    }
}
