using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

namespace Sailing.WaveFunctionCollapse
{
    public class MapBehaviour : MonoBehaviour
    {
        public InfiniteMap Map;
        public int MapHeight = 1;
        public int collapseAreaSize;
        public ModuleData ModuleData;
        public float minRate = .5f;
        public float maxRate = 1;
        public int discrete = 1;

        public Vector3 GetWorldspacePosition(Vector3 position)
        {
            return this.transform.position
                + Vector3.up * InfiniteMap.BLOCK_SIZE / 2f
                + position * InfiniteMap.BLOCK_SIZE;
        }

        public void Clear()
        {
            var children = new List<Transform>();
            foreach (Transform child in this.transform)
            {
                children.Add(child);
            }
            foreach (var child in children)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
            this.Map = null;
        }

        public void InitializeRectMap()
        {
            ModuleData.Current = this.ModuleData.Modules;
            this.Clear();
            this.Map = new InfiniteMap(new Vector3(this.collapseAreaSize, this.MapHeight, this.collapseAreaSize));
        }

        public void InitializeUnevenMap(){
            ModuleData.Current = this.ModuleData.Modules;
            this.Clear();

            List<Vector3> positions = new List<Vector3>();
            List<int> previousYPositions;
            System.Random random = new System.Random();

            for (int z = 0; z < this.MapHeight; z++){
                previousYPositions = new List<int>();

                for (int x = 0; x < this.collapseAreaSize; x++){
                    int yToStart = 0;
                    int yToBe = random.Next(
                        (int)(this.collapseAreaSize * minRate), 
                        (int)(this.collapseAreaSize * maxRate));

                    if(previousYPositions.Count == 0){
                        yToStart = 0;
                        yToBe += yToStart;
                    }else{
                        var previousYPosition = previousYPositions[0];

                        yToStart = random.Next(
                            previousYPosition - (yToBe - 1) / discrete, 
                            previousYPosition + (yToBe - 1) / discrete);
                        yToBe += yToStart;
                    }

                    for (; yToStart < yToBe; yToStart++){
                        positions.Add(new Vector3(x, z, yToStart));
                        previousYPositions.Add(yToStart);
                    }
                }   
            }

            this.Map = new InfiniteMap(positions);
        }

        public bool BuildSlot(Slot slot)
        {
            if (slot.Instantiation != null)
            {
#if UNITY_EDITOR
			    GameObject.DestroyImmediate(slot.Instantiation);
#else
                GameObject.Destroy(slot.GameObject);
#endif
            }

            if(!slot.Collapsed){
                return false;
            }

            var gameObject = GameObject.Instantiate(slot.Module.Prefab);
            gameObject.name = slot.Module.Prefab.name + " " + slot.Position;
            GameObject.DestroyImmediate(gameObject.GetComponent<ModulePrototype>());
            gameObject.transform.parent = this.transform;
            gameObject.transform.position = this.GetWorldspacePosition(slot.Position);
            slot.Instantiation = gameObject;
            
            return true;
        }

        public void BuildAllSlots()
        {
            foreach(var slot in this.Map.GetAllSlots()){
                this.BuildSlot(slot);
            }
        }

        public bool VisualizeSlots = false;

#if UNITY_EDITOR
        [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
        static void DrawGizmo(MapBehaviour mapBehaviour, GizmoType gizmoType) {
            if (!mapBehaviour.VisualizeSlots) {
                return;
            }
            if (mapBehaviour.Map == null) {
                return;
            }
            foreach (var slot in mapBehaviour.Map.GetAllSlots()) {
                if (slot.Collapsed || slot.Modules.Count == ModuleData.Current.Length) {
                    continue;
                }
                Handles.Label(mapBehaviour.GetWorldspacePosition(slot.Position), slot.Modules.Count.ToString());
            }
        }
#endif
    }
}