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
        private HashSet<Slot> workArea;
        private IEnumerable<Vector3Int> targets;
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
                        targets.Add(start + new Vector3Int(x, y, z));
                    }
                }
            }
            this.targets = targets;
        }

        public override Slot GetSlot(Vector3Int position)
        {
            if (this.Slots.ContainsKey(position))
            {
                return this.Slots[position];
            }

            this.Slots[position] = new Slot(position, this);

            return this.Slots[position];
        }

        public override IEnumerable<Slot> GetAllSlots()
        {
            return this.Slots.Values;
        }
        
        public override void Collapse(){
            this.workArea = new HashSet<Slot>(targets.Select(target => this.GetSlot(target)).Where(slot => slot != null && !slot.Collapsed));

            for (int i = 0; i < this.workArea.Count; i++)
            {
                var selected = this.Select(workArea);

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