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
		public void CreateModules()
        {
            doCreateModules();
        }


        private void doCreateModules(){
            int count = 0;
            
            var modules = new List<Module>();

            var prototypes = this.getModulePrototypes().ToArray();

            for (int i = 0; i < prototypes.Length; i++)
            {
                var prototype = prototypes[i];

                var module = new Module(prototype.gameObject, count++);

                modules.Add(module);
            }

            ModuleData.Current = modules.ToArray();

            foreach (var module in modules)
            {
                module.Neighbors = new ModuleSet[6];

                var prototypeNeighbors = this.getModulePrototypeNeighbors(module.Prototype);

                for (int direction = 0; direction < 6; direction++)
                {
                    var neighbors = prototypeNeighbors[direction];

                    var neighborModules = modules.Where(
                        neighbor => neighbors.Any(
                            elem => elem == neighbor.Prototype.name
                        )
                    );

                    module.Neighbors[direction] = new ModuleSet(neighborModules);
                }
            }

            foreach(var module in modules){
                var prototypeNeighbors = this.getModulePrototypeNeighbors(module.Prototype);

                for (int direction = 0; direction < 6; direction++)
                {
                    var neighbors = prototypeNeighbors[direction];
                    int oppositeDirection = (direction + 3) % 6;

                    var neighborModules = modules.Where(
                        neighbor => neighbors.Any(
                            elem => elem == neighbor.Prototype.name
                        )
                    );

                    foreach(var neighborModule in neighborModules){
                        var prev = neighborModule.Neighbors[oppositeDirection];

                        prev.Add(module);

                        neighborModule.Neighbors[oppositeDirection] = prev;
                    }
                }
            }

            this.Modules = modules.ToArray();
        }
    }
}