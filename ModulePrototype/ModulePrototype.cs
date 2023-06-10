using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

namespace Sailing.WaveFunctionCollapse
{
    public class ModulePrototype : MonoBehaviour
    {
        public float Probability = 1.0f;
        public bool Spawn = true;
        public bool IsInterior = false;

        public Mesh GetMesh(bool createEmptyFallbackMesh = true)
        {
            var meshFilter = this.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                return meshFilter.sharedMesh;
            }
            if (createEmptyFallbackMesh)
            {
                var mesh = new Mesh();
                return mesh;
            }
            return null;
        }

        void Update() { }

        void Reset() { }
    }
}