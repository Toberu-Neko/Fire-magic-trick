using UnityEngine;

public class TestingTool : MonoBehaviour
{
    public Siren siren;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            siren.Play();
    }
}
