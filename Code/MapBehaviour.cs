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

        public int MapHeight = 6;

        public BoundaryConstraint[] BoundaryConstraints;

        public bool ApplyBoundaryConstraints = true;

        public ModuleData ModuleData;

        // private CullingData cullingData;

        public Vector3 GetWorldspacePosition(Vector3Int position)
        {
            return this.transform.position
                + Vector3.up * InfiniteMap.BLOCK_SIZE / 2f
                + position.ToVector3() * InfiniteMap.BLOCK_SIZE;
        }

        public Vector3Int GetMapPosition(Vector3 worldSpacePosition)
        {
            var pos = (worldSpacePosition - this.transform.position) / InfiniteMap.BLOCK_SIZE;
            return Vector3Int.FloorToInt(pos + new Vector3(0.5f, 0, 0.5f));
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

        public void Initialize()
        {
            ModuleData.Current = this.ModuleData.Modules;
            this.Clear();
            this.Map = new InfiniteMap(this.MapHeight);
        }

        public bool Initialized
        {
            get
            {
                return this.Map != null;
            }
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
            gameObject.transform.rotation = Quaternion.Euler(Vector3.up * 90f * slot.Rotation);
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