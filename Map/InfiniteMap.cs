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
        private List<Vector3Int> Targets;
        private Dictionary<Vector3Int, Slot> Slots;

        public InfiniteMap(Vector3Int size) : base()
        {
            InfiniteMap.Random = new System.Random();

            this.Slots = new Dictionary<Vector3Int, Slot>();

            var start = Vector3Int.zero;

            var targets = new List<Vector3Int>();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        var position = start + new Vector3Int(x, y, z);

                        targets.Add(position);

                        this.Slots[position] = new Slot(position, this);
                    }
                }
            }

            this.Targets = targets;
        }

        public override Slot GetSlot(Vector3Int position)
        {
            if(IsOutOfRange(position)){
                return null;
            }

            if (this.Slots.ContainsKey(position))
            {
                return this.Slots[position];
            }

            return this.Slots[position];
        }

        private bool IsOutOfRange(Vector3Int position){
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