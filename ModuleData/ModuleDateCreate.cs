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
                module.PossibleNeighbors = new ModuleSet[6];

                var prototypeNeighbors = this.getModulePrototypeNeighbors(module.Prototype);

                for (int direction = 0; direction < 6; direction++)
                {
                    var neighbors = prototypeNeighbors[direction];

                    var possibleNeighborModules = modules.Where(
                        neighbor => neighbors.Any(
                            elem => elem == neighbor.Prototype.name
                        )
                    );

                    module.PossibleNeighbors[direction] = new ModuleSet(possibleNeighborModules);
                }
            }

            foreach(var module in modules){
                var prototypeNeighbors = this.getModulePrototypeNeighbors(module.Prototype);

                for (int direction = 0; direction < 6; direction++)
                {
                    var neighbors = prototypeNeighbors[direction];
                    int oppositeDirection = (direction + 3) % 6;

                    var possibleNeighborModules = modules.Where(
                        neighbor => neighbors.Any(
                            elem => elem == neighbor.Prototype.name
                        )
                    );

                    foreach(var possibleNeighborModule in possibleNeighborModules){
                        var prev = possibleNeighborModule.PossibleNeighbors[oppositeDirection];

                        prev.Add(module);

                        possibleNeighborModule.PossibleNeighbors[oppositeDirection] = prev;
                    }
                }
            }

            this.Modules = modules.ToArray();
        }
    }
}