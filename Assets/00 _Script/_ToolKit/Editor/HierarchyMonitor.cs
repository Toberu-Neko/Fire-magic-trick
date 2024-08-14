using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyMonitor
{
    static HierarchyMonitor()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }


    static void OnHierarchyChanged()
    {
        var allLine = GameObject.FindObjectsOfType<CableProceduralSimple>();

        foreach (var item in allLine)
        {
            if (!Application.isPlaying)
            {
                CableProceduralSimple script = item.GetComponent<CableProceduralSimple>();

                script.Init();
            }
        }

        var allMapObj = GameObject.FindObjectsOfType<DataPersistMapObjBase>();

        foreach (var item in allMapObj)
        {
            if (!Application.isPlaying)
            {
                DataPersistMapObjBase script = item.GetComponent<DataPersistMapObjBase>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
        }

        var allTempObj = GameObject.FindObjectsOfType<TempDataPersist_MapObjBase>();

        foreach (var item in allTempObj)
        {
            if (!Application.isPlaying)
            {
                TempDataPersist_MapObjBase script = item.GetComponent<TempDataPersist_MapObjBase>();

                if (!script.isAddedID)
                {
                    script.isAddedID = true;
                    script.ID = System.Guid.NewGuid().ToString();
                    EditorUtility.SetDirty(item);
                }
            }
        }
    }
}