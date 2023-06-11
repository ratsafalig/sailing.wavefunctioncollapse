using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

namespace Sailing.WaveFunctionCollapse
{
    public class InfiniteMap : AbstractMap
    {
        public int RangeLimit = 80;
        public static System.Random Random;
        public const float BLOCK_SIZE = 1f;
        private List<Vector3> Targets;
        private Dictionary<Vector3, Slot> Slots;

        public InfiniteMap(Vector3 size) : base()
        {
            InfiniteMap.Random = new System.Random();

            this.Slots = new Dictionary<Vector3, Slot>();

            var start = Vector3.zero;

            var targets = new List<Vector3>();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        var position = start + new Vector3(x, y, z);

                        targets.Add(position);

                        this.Slots[position] = new Slot(position, Vector3.zero, this);
                    }
                }
            }

            this.Targets = targets;
        }

        public InfiniteMap(IEnumerable<Vector3> positions){
            InfiniteMap.Random = new System.Random();

            this.Slots = new Dictionary<Vector3, Slot>();

            var start = Vector3.zero;

            var targets = new List<Vector3>();

            foreach(var position in positions){
                targets.Add(position);
                this.Slots[position] = new Slot(position, Vector3.zero, this);
            }

            this.Targets = targets;
        }

        public override Slot GetSlot(Vector3 position)
        {
            if(IsOutOfRange(position)){
                return null;
            }

            if (this.Slots.ContainsKey(position))
            {
                return this.Slots[position];
            }

            return null;
        }

        private bool IsOutOfRange(Vector3 position){
            foreach(var target in this.Targets){
                if(position.x == target.x && position.y == target.y && position.z == target.z){
                    return false;
                }
            }
            return true;
        }

        public override IEnumerable<Slot> GetAllSlots()
        {
            return this.Slots.Values;
        }
        
        public override void Collapse(){


            for (int i = 0; i < this.Targets.Count; i++)
            {
                var selected = this.Select(this.Slots.Values);

                if (selected != null)
                {
                    selected.Collapse();
                }
            }
        }

        public override Slot Select(IEnumerable<Slot> slots)
        {
            Slot selected = null;

            float minEntropy = float.PositiveInfinity;

            foreach (var slot in slots)
            {
                if (slot.Collapsed)
                {   
                    continue;
                }

                float entropy = slot.Modules.Entropy;
                
                if (entropy < minEntropy)
                {
                    selected = slot;
                    minEntropy = entropy;
                }
            }

            return selected;
        }
    }
}