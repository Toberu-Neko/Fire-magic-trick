using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(InstanceCombiner))]
public class InstanceCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InstanceCombiner instanceCombiner = (InstanceCombiner)target;
        if (GUILayout.Button("Combine Meshes"))
        {
            instanceCombiner.CombineMesh();
        }

        if (GUILayout.Button("Remove Components"))
        {
            instanceCombiner.RemoveComps();
        }
    }
}

public class InstanceCombiner : MonoBehaviour
{
    // Source Meshes you want to combine
    private List<MeshFilter> listMeshFilter;

    // Make a new mesh to be the target of the combine operation
    private MeshFilter TargetMesh;
    private MeshRenderer TargetMeshRenderer;
    private MeshCollider TargetMeshCollider;

    private void Awake()
    {
        Debug.LogError("This script is for Editor Only in " + transform.parent.name);
        enabled = false;
        return;
    }

    [ContextMenu("Combine Meshes")]
    public void CombineMesh()
    {
        AddMeshComps();
        GetMeshFiltersInChildren();
        //Make an array of CombineInstance.
        var combine = new CombineInstance[listMeshFilter.Count];

        //Set Mesh And their Transform to the CombineInstance
        for (int i = 0; i < listMeshFilter.Count; i++)
        {
            combine[i].mesh = listMeshFilter[i].sharedMesh;
            combine[i].transform = listMeshFilter[i].transform.localToWorldMatrix;
        }

        // Create a Empty Mesh
        var mesh = new Mesh();

        //Call targetMesh.CombineMeshes and pass in the array of CombineInstances.

        mesh.CombineMeshes(combine);
        transform.localPosition = mesh.bounds.center * -1;

        //Assign the target mesh to the mesh filter of the combination game object.
        TargetMesh.mesh = mesh;
        TargetMeshCollider.sharedMesh = mesh;
        TargetMeshRenderer.sharedMaterials = listMeshFilter[0].GetComponent<MeshRenderer>().sharedMaterials;
        
        // Save The Mesh To Location
        SaveMesh(TargetMesh.sharedMesh, gameObject.name, false, true);

        // Print Results
        print($"<color=#20E7B0>Combine Meshes was Successful!</color>");
        DestroyImmediate(this);
    }

    private void AddMeshComps()
    {
        TargetMesh = gameObject.AddComponent<MeshFilter>();
        TargetMeshRenderer = gameObject.AddComponent<MeshRenderer>();
        TargetMeshCollider = gameObject.AddComponent<MeshCollider>();
    }

    public void RemoveComps()
    {
        DestroyImmediate(TargetMesh);
        DestroyImmediate(TargetMeshRenderer);
        DestroyImmediate(TargetMeshCollider);
    }

    private void GetMeshFiltersInChildren()
    {
        listMeshFilter = new();
        foreach (var item in GetComponentsInChildren<MeshFilter>())
        {
            listMeshFilter.Add(item);
        }
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
#endif
