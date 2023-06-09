#define SAILING_DEBUG

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Sailing.WaveFunctionCollapse{
    public class CollapseStrategy{
        public void Collapse(Slot slot)
        {
            this.doCollapse_0(slot);
        }

        private void doCollapse_0(Slot slot){
            if (!slot.Modules.Any())
            {
                throw new CollapseFailedException(slot);
            }
            if (slot.Collapsed)
            {
                throw new Exception("Slot is already collapsed.");
            }

            float max = slot.Modules.Select(module => module.Prototype.Probability).Sum();
            float roll = (float)(InfiniteMap.Random.NextDouble() * max);
            float p = 0;
            foreach (var candidate in slot.Modules)
            {
                p += candidate.Prototype.Probability;
                if (p >= roll)
                {
                    this.doCollapse_1(slot, candidate);
                    return;
                }
            }
            this.doCollapse_1(slot, slot.Modules.First());
        }

        private void doCollapse_1(Slot slot, Module module){

            for (int d = 0; d < 6; d++){
                var neighbors = module.PossibleNeighbors[d];

                var neighborSlot = slot.GetNeighborSlot(d);

                if(neighborSlot != null)
                {
                    neighborSlot.UpdateModules(neighbors);
                }
            }

            slot.Module = module;
            slot.Collapsed = true;
        }
    }
}