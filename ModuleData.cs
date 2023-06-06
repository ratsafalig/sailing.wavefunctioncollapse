using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;

namespace Sailing.WaveFunctionCollapse
{
    [CreateAssetMenu(menuName = "Wave Function Collapse/Sailing Module Data", fileName = "modules.asset")]
    public partial class ModuleData : ScriptableObject, ISerializationCallbackReceiver
    {
        public static Module[] Current;

        public GameObject Prototypes;

        public Module[] Modules;

#if UNITY_EDITOR
        // irr
        public void SimplifyNeighborData()
        {
            ModuleData.Current = this.Modules;
            const int height = 12;
            int count = 0;
            var center = new Vector3Int(0, height / 2, 0);

            int p = 0;
            foreach (var module in this.Modules)
            {
                var map = new InfiniteMap(height);
                var slot = map.GetSlot(center);
                try
                {
                    slot.Collapse(module);
                }
                catch (CollapseFailedException exception)
                {
                    throw new InvalidOperationException("Module " + module.Name + " creates a failure at relative position " + (exception.Slot.Position - center) + ".");
                }
                for (int direction = 0; direction < 6; direction++)
                {
                    var neighbor = slot.GetNeighbor(direction);
                    int unoptimizedNeighborCount = module.PossibleNeighbors[direction].Count;
                    module.PossibleNeighbors[direction].Intersect(neighbor.Modules);
                    count += unoptimizedNeighborCount - module.PossibleNeighbors[direction].Count;
                }
                module.PossibleNeighborsArray = module.PossibleNeighbors.Select(ms => ms.ToArray()).ToArray();
                p++;
                EditorUtility.DisplayProgressBar("Simplifying... " + count, module.Name, (float)p / this.Modules.Length);
            }
            Debug.Log("Removed " + count + " impossible neighbors.");
            EditorUtility.ClearProgressBar();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private IEnumerable<ModulePrototype> getPrototypes()
        {
            foreach (Transform transform in this.Prototypes.transform)
            {
                var item = transform.GetComponent<ModulePrototype>();
                if (item != null && item.enabled)
                {
                    yield return item;
                }
            }
        }

		private string[][] getPrototypeNeighbors(ModulePrototype root){
			string[][] result = new string[6][];
			foreach(Transform transform in root.transform){
				var name = transform.name;
				if(name.Contains("FORWARD")){
					result[Orientations.FORWARD] = getPrototypeNeighbor(transform);
				}
				if(name.Contains("BACK")){
					result[Orientations.BACK] = getPrototypeNeighbor(transform);
				}
				if(name.Contains("LEFT")){
					result[Orientations.LEFT] = getPrototypeNeighbor(transform);
				}
				if(name.Contains("RIGHT")){
					result[Orientations.RIGHT] = getPrototypeNeighbor(transform);
				}
				if(name.Contains("UP")){
					result[Orientations.UP] = getPrototypeNeighbor(transform);
				}
				if(name.Contains("DOWN")){
					result[Orientations.DOWN] = getPrototypeNeighbor(transform);
				}
			}
			return result;
		}

        private string[] getPrototypeNeighbor(Transform transform){
            string[] result = new string[transform.childCount];

            for (int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);

                result[i] = child.name;
            }

            return result;
        }
#endif

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            ModuleData.Current = this.Modules;
            foreach (var module in this.Modules)
            {
                module.PossibleNeighborsArray = module.PossibleNeighbors.Select(ms => ms.ToArray()).ToArray();
            }
        }

        public void SavePrototypes()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this.Prototypes);
            AssetDatabase.SaveAssets();
            foreach (var module in this.Modules)
            {
                module.Prototype = module.Prefab.GetComponent<ModulePrototype>();
            }
#endif
        }
    }
}