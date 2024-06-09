using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is in the trigger");
            // Load the scene
            // SceneManager.LoadScene("SceneName");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is out of the trigger");
        }
    }
}
