#define SAILING_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sailing.WaveFunctionCollapse
{
    public class Slot
    {
        public Vector3Int Position;

        // List of modules that can still be placed here
        public ModuleSet Modules;

        private AbstractMap map;

        public Module Module;

        public GameObject GameObject;

        public CollapseStrategy strategy;

        public bool Collapsed
        {
            get
            {
                return this.Module != null;
            }
        }

        public bool ConstructionComplete
        {
            get
            {
                return this.GameObject != null || (this.Collapsed && !this.Module.Prototype.Spawn);
            }
        }

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

        public void Collapse(){
            strategy.Collapse(this);
        }

        /// <summary>
        /// Add modules non-recursively.
        /// Returns true if this lead to this slot changing from collapsed to not collapsed.
        /// </summary>
        public void AddModules(ModuleSet modulesToAdd)
        {
            foreach (var module in modulesToAdd)
            {
                if (this.Modules.Contains(module) || module == this.Module)
                {
                    continue;
                }
                for (int d = 0; d < 6; d++)
                {
                    int inverseDirection = (d + 3) % 6;
                    var neighbor = this.GetNeighborSlot(d);
                    if (neighbor == null || neighbor.Forgotten)
                    {
                        continue;
                    }
                }
                this.Modules.Add(module);
            }

            if (this.Collapsed && !this.Modules.Empty)
            {
                this.Module = null;
            }
        }

        public override int GetHashCode()
        {
            return this.Position.GetHashCode();
        }

        public void Forget()
        {
            this.Modules = null;
        }

        public bool Forgotten
        {
            get
            {
                return this.Modules == null;
            }
        }
    }
}