using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Sailing.WaveFunctionCollapse
{
    [CustomEditor(typeof(ModulePrototype))]
    public class ModulePrototypeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ModulePrototype modulePrototype = (ModulePrototype)target;
            if (GUILayout.Button("Distribute"))
            {
                int i = 0;
                foreach (Transform transform in modulePrototype.transform.parent)
                {
                    transform.localPosition = Vector3.forward * i * InfiniteMap.BLOCK_SIZE * 2f;
                    i++;
                }
            }
        }
    }
}