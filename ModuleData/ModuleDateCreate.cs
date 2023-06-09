using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;

namespace Sailing.WaveFunctionCollapse
{

    public partial class ModuleData : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// using transform hierarchy to decide any two tiles can concat 
        /// </summary>
        /// <param name="respectNeigborExclusions"></param>
		public void CreateModules(bool respectNeigborExclusions = true)
        {
            doCreateModules(respectNeigborExclusions);
        }


        private void doCreateModules(bool respectNeigborExclusions = true){
            int count = 0;
            
            var modules = new List<Module>();

            var prototypes = this.getPrototypes().ToArray();

            var scenePrototype = new Dictionary<Module, ModulePrototype>();

            for (int i = 0; i < prototypes.Length; i++)
            {
                var prototype = prototypes[i];

                for (int face = 0; face < 6; face++)
                {
                    if (prototype.Faces[face].ExcludedNeighbours == null)
                    {
                        prototype.Faces[face].ExcludedNeighbours = new ModulePrototype[0];
                    }
                }

                for (int rotation = 0; rotation < 4; rotation++)
                {
                    if (rotation == 0 || !prototype.CompareRotatedVariants(0, rotation))
                    {
                        var module = new Module(prototype.gameObject, rotation, count);
                        modules.Add(module);
                        scenePrototype[module] = prototype;
                        count++;
                    }
                }

                EditorUtility.DisplayProgressBar("Creating module prototypes...", prototype.gameObject.name, (float)i / prototypes.Length);
            }

            ModuleData.Current = modules.ToArray();

            foreach (var module in modules)
            {
                module.PossibleNeighbors = new ModuleSet[6];

                var prototypeNeighbors = this.getPrototypeNeighbors(module.Prototype);

                for (int direction = 0; direction < 6; direction++)
                {
                    var face = scenePrototype[module].Faces[Orientations.Rotate(direction, module.Rotation)];

                    var neighbors = prototypeNeighbors[direction];

                    module.PossibleNeighbors[direction] = new ModuleSet(
                        modules.Where(
                            neighbor => neighbors.Any(
                                elem => elem == neighbor.Prototype.name
                            )
                        )
                    );
                }
            }
            EditorUtility.ClearProgressBar();

            this.Modules = modules.ToArray();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="respectNeigborExclusions"></param>
        public void CreateModulesConfig(bool respectNeigborExclusions = true){

        }
    }
}