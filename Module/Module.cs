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
        public string Name;

        public ModulePrototype Prototype;
        public GameObject Prefab;

        public int Rotation;

        public ModuleSet[] PossibleNeighbors;

        [HideInInspector]
        public int Index;

        public float PLogP;

        public Module(GameObject prefab, int rotation, int index)
        {
            this.Rotation = rotation;
            this.Index = index;
            this.Prefab = prefab;
            this.Prototype = this.Prefab.GetComponent<ModulePrototype>();
            this.Name = this.Prototype.gameObject.name + " R" + rotation;
            this.PLogP = this.Prototype.Probability * Mathf.Log(this.Prototype.Probability);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}