#define SAILING_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sailing.WaveFunctionCollapse
{
    public class Slot : AbstractSlot
    {
        public Vector3Int Position;
        public GameObject Instantiation;
        public Module Module;

        // List of modules that can still be placed here
        public ModuleSet Modules;

        private AbstractMap map;


        public bool Collapsed;

        public Slot(Vector3Int position, AbstractMap map)
        {
            this.Position = position;
            this.map = map;
            this.Modules = new ModuleSet(initializeFull: true);
        }

        public Slot(Vector3Int position, AbstractMap map, Slot prototype)
        {
            this.Position = position;
            this.map = map;
            this.Modules = new ModuleSet(prototype.Modules);
        }

        // TODO only look up once and then cache???
        public Slot GetNeighborSlot(int direction)
        {
            return this.map.GetSlot(this.Position + Orientations.Direction[direction]);
        }

        public override void Collapse()
        {
            if (!this.Modules.Any())
            {
                throw new CollapseFailedException(this);
            }
            if (this.Collapsed)
            {
                throw new Exception("Slot is already collapsed.");
            }

            float max = this.Modules.Select(module => module.Prototype.Probability).Sum();
            float roll = (float)(InfiniteMap.Random.NextDouble() * max);
            float p = 0;
            foreach (var candidate in this.Modules)
            {
                p += candidate.Prototype.Probability;
                if (p >= roll)
                {
                    this.doCollapse(candidate);
                    return;
                }
            }
            this.doCollapse(this.Modules.First());
        }

        private void doCollapse(Module module)
        {

            for (int d = 0; d < 6; d++)
            {
                var neighbors = module.PossibleNeighbors[d];

                var neighborSlot = this.GetNeighborSlot(d);

                if (neighborSlot != null)
                {
                    neighborSlot.UpdateModules(neighbors);
                }
            }

            this.Module = module;
            this.Collapsed = true;
        }

        public void UpdateModules(ModuleSet modulesToSet)
        {
            this.Modules.Intersect(modulesToSet);
        }

        public override int GetHashCode()
        {
            return this.Position.GetHashCode();
        }
    }
}