using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

namespace Sailing.WaveFunctionCollapse
{
    [System.Serializable]
    public class Module
    {
        public ModulePrototype Prototype;
        public GameObject Prefab;

        public ModuleSet[] Neighbors;

        [HideInInspector]
        public int Index;

        public float PLogP;

        public Module(GameObject prefab, int index)
        {
            this.Index = index;
            this.Prefab = prefab;
            this.Prototype = this.Prefab.GetComponent<ModulePrototype>();
            this.PLogP = this.Prototype.Probability * Mathf.Log(this.Prototype.Probability);
        }
    }
}