using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshManager : MonoBehaviour
{
    [SerializeField, Tooltip("NavMesh物件")] NavMeshSurface[] navigationObject;

    public void Build()
    {
        foreach(NavMeshSurface nav in navigationObject)
        {
            nav.BuildNavMesh();
        }
    }
}
