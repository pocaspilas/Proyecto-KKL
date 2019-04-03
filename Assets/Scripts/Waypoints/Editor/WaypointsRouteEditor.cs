using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(WaypointsRoute))]
public class WaypointsRouteEditor : Editor {
    WaypointsRoute t;

    private void OnEnable()
    {
        t = (WaypointsRoute)target;
    }

    float heightVal;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        heightVal = EditorGUILayout.FloatField(heightVal);
        if(GUILayout.Button("Set Height From Ground"))
        {
            SetHeightFromGround(heightVal);
        }
        GUILayout.Space(20);
        if (GUILayout.Button("Refresh Nodes"))
        {
            RefreshNodes();
        }
    }

    private void RefreshNodes()
    {
        t.CalculateSpline();
    }

    private void SetHeightFromGround(float height)
    {
        Vector3 pos;
        for (int i = 0; i < t.waypointsList.Count; i++)
        {
            pos = t.waypointsList[i].position;
            pos.y = 50;
            RaycastHit[] hit;
            hit = Physics.RaycastAll(pos, Vector3.down, 100);
            if(hit.Length > 0 && hit[0].collider != null)
            {
                pos = hit[0].point;
                pos.y += height;
                t.waypointsList[i].position = pos;
            }
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}
