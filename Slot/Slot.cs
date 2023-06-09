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
        public int Rotation;
        public GameObject GameObject;
        public Module Module;

        // List of modules that can still be placed here
        public ModuleSet Modules;

        private AbstractMap map;

        public CollapseStrategy strategy;

        public bool Collapsed;

        public Slot(Vector3Int position, AbstractMap map)
        {
            this.Position = position;
            this.map = map;
            this.Modules = new ModuleSet(initializeFull: true);
            this.strategy = new CollapseStrategy();
        }

        public Slot(Vector3Int position, AbstractMap map, Slot prototype)
        {
            this.Position = position;
            this.map = map;
            this.Modules = new ModuleSet(prototype.Modules);
            this.strategy = new CollapseStrategy();
        }

        // TODO only look up once and then cache???
        public Slot GetNeighborSlot(int direction)
        {
            return this.map.GetSlot(this.Position + Orientations.Direction[direction]);
        }

        public void Collapse(){
            strategy.Collapse(this);
        }

        public void SetModules(ModuleSet modulesToSet){
            this.Modules = modulesToSet;
        }

        public override int GetHashCode()
        {
            return this.Position.GetHashCode();
        }
    }
}