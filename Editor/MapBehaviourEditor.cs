using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;

namespace Sailing.WaveFunctionCollapse
{
    [CustomEditor(typeof(MapBehaviour))]
    public class MapBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapBehaviour mapBehaviour = (MapBehaviour)target;
            if (GUILayout.Button("Clear"))
            {
                mapBehaviour.Clear();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            GUILayout.BeginHorizontal();
            int.TryParse(GUILayout.TextField(mapBehaviour.collapseAreaSize.ToString()), out mapBehaviour.collapseAreaSize);

            if (GUILayout.Button("Initialize Rect Map " + mapBehaviour.collapseAreaSize + "x" + mapBehaviour.collapseAreaSize + " area"))
            {
                var startTime = System.DateTime.Now;
                mapBehaviour.InitializeRectMap();
                mapBehaviour.Map.Collapse();
                mapBehaviour.BuildAllSlots();
                Debug.Log("Initialized in " + (System.DateTime.Now - startTime).TotalSeconds + " seconds.");
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            
            int.TryParse(GUILayout.TextField(mapBehaviour.discrete.ToString()), out mapBehaviour.discrete);

            float.TryParse(GUILayout.TextField(mapBehaviour.minRate.ToString()), out mapBehaviour.minRate);
            float.TryParse(GUILayout.TextField(mapBehaviour.maxRate.ToString()), out mapBehaviour.maxRate);
            
            if (GUILayout.Button("Initialize Uneven Map " + mapBehaviour.collapseAreaSize + "x" + mapBehaviour.collapseAreaSize + " area"))
            {
                var startTime = System.DateTime.Now;
                mapBehaviour.InitializeUnevenMap();
                mapBehaviour.Map.Collapse();
                mapBehaviour.BuildAllSlots();
                Debug.Log("Initialized in " + (System.DateTime.Now - startTime).TotalSeconds + " seconds.");
            }

            GUILayout.EndHorizontal();
        }
    }
}