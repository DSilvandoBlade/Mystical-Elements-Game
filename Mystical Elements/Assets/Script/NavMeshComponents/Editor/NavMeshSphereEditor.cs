using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavMeshSphere))]
public class NavMeshSphereEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load navmesh data"))
        {
            (target as NavMeshSphere).LoadNavmeshData();
            if ((target as NavMeshSphere) == null)
            {
                Debug.Log("NULL");
            }
            Debug.Log("Loaded Navmesh");
        }

        if (GUILayout.Button("remove navmesh data"))
        {
            (target as NavMeshSphere).RemoveAllNavMeshLoadedData();
            Debug.Log("Navmesh Removed");
        }
    }
}