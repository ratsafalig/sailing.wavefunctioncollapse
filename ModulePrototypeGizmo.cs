using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

namespace Sailing.WaveFunctionCollapse
{
    public partial class ModulePrototype : MonoBehaviour
    {
#if UNITY_EDITOR
        private static ModulePrototypeEditorData editorData;
        private static GUIStyle style;

        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        static void DrawGizmo(ModulePrototype modulePrototype, GizmoType gizmoType)
        {
            var transform = modulePrototype.transform;
            Vector3 position = transform.position;
            var rotation = transform.rotation;

            if (ModulePrototype.editorData == null || ModulePrototype.editorData.ModulePrototype != modulePrototype)
            {
                ModulePrototype.editorData = new ModulePrototypeEditorData(modulePrototype);
            }

            Gizmos.color = new Color(1f, 1f, 1f, 0.3f);
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    var hint = ModulePrototype.editorData.GetConnectorHint(i);
                    if (hint.Mesh != null)
                    {
                        Gizmos.DrawMesh(hint.Mesh,
                            position + rotation * Orientations.Direction[i].ToVector3() * AbstractMap.BLOCK_SIZE,
                            rotation * Quaternion.Euler(Vector3.up * 90f * hint.Rotation));
                    }
                }
            }
            for (int i = 0; i < 6; i++)
            {
                if (modulePrototype.Faces[i].Walkable)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(position + Vector3.down * 0.1f, position + rotation * Orientations.Rotations[i] * Vector3.forward * AbstractMap.BLOCK_SIZE * 0.5f + Vector3.down * 0.1f);
                }
                if (modulePrototype.Faces[i].IsOcclusionPortal)
                {
                    Gizmos.color = Color.blue;

                    var dir = rotation * Orientations.Rotations[i] * Vector3.forward;
                    Gizmos.DrawWireCube(position + dir, (Vector3.one - new Vector3(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z))) * AbstractMap.BLOCK_SIZE);
                }
            }

            if (ModulePrototype.style == null)
            {
                ModulePrototype.style = new GUIStyle();
                ModulePrototype.style.alignment = TextAnchor.MiddleCenter;
            }

            ModulePrototype.style.normal.textColor = Color.black;
            for (int i = 0; i < 6; i++)
            {
                var face = modulePrototype.Faces[i];
                Handles.Label(position + rotation * Orientations.Rotations[i] * Vector3.forward * InfiniteMap.BLOCK_SIZE / 2f, face.ToString(), ModulePrototype.style);
            }
        }
#endif
    }
}