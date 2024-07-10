using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShakingSce : MonoBehaviour
{
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = new Vector3(50, -10, 0);
    }
}
