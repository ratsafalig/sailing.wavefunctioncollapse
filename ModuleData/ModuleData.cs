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
        public Module[] Modules;
        public GameObject Prototypes;

        public static class Direction{
            public const string FORWARD = "FORWARD";
            public const string BACK = "BACK";
            public const string LEFT = "LEFT";
            public const string RIGHT = "RIGHT";
            public const string UP = "UP";
            public const string DOWN = "DOWN";
        }

        private IEnumerable<ModulePrototype> getModulePrototypes()
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

		private string[][] getModulePrototypeNeighbors(ModulePrototype root){
			string[][] result = new string[6][];

			foreach(Transform transform in root.transform){
				var name = transform.name;
				if(name.Contains(Direction.FORWARD)){
					result[Orientations.FORWARD] = getModulePrototypeNeighbor(transform);
				}
				if(name.Contains(Direction.BACK)){
					result[Orientations.BACK] = getModulePrototypeNeighbor(transform);
				}
				if(name.Contains(Direction.LEFT)){
					result[Orientations.LEFT] = getModulePrototypeNeighbor(transform);
				}
				if(name.Contains(Direction.RIGHT)){
					result[Orientations.RIGHT] = getModulePrototypeNeighbor(transform);
				}
				if(name.Contains(Direction.UP)){
					result[Orientations.UP] = getModulePrototypeNeighbor(transform);
				}
				if(name.Contains(Direction.DOWN)){
					result[Orientations.DOWN] = getModulePrototypeNeighbor(transform);
				}
			}
			return result;
		}

        private string[] getModulePrototypeNeighbor(Transform transform){
            string[] result = new string[transform.childCount];

            for (int i = 0; i < transform.childCount; i++){
                var child = transform.GetChild(i);

                result[i] = child.name;
            }

            return result;
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            ModuleData.Current = this.Modules;
        }
    }
}