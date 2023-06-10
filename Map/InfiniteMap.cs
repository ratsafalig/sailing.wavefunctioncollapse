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
        private Dictionary<Vector3Int, Slot> Slots;

        public readonly int Height;

        public int RangeLimit = 80;

        public static System.Random Random;
        public Queue<Slot> Order;
        public const float BLOCK_SIZE = 1f;
        private HashSet<Slot> workArea;


        public InfiniteMap(int height) : base()
        {
            InfiniteMap.Random = new System.Random();
            this.Height = height;
            this.Slots = new Dictionary<Vector3Int, Slot>();
        }

        public override Slot GetSlot(Vector3Int position)
        {
            if (position.y >= this.Height || position.y < 0)
            {
                return null;
            }

            if (this.Slots.ContainsKey(position))
            {
                return this.Slots[position];
            }

            if (this.IsOutsideOfRangeLimit(position))
            {
                return null;
            }

            this.Slots[position] = new Slot(position, this);

            return this.Slots[position];
        }

        public bool IsOutsideOfRangeLimit(Vector3Int position)
        {
            return position.magnitude > this.RangeLimit;
        }

        public override IEnumerable<Slot> GetSlots()
        {
            return this.Order;
        }
        
        public override void Collapse(){

        }

        public void Collapse(Vector3Int start, Vector3Int size)
        {
            this.Order = new Queue<Slot>();

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
            this.Collapse(targets);
        }

        private void Collapse(IEnumerable<Vector3Int> targets)
        {
            this.workArea = new HashSet<Slot>(targets.Select(target => this.GetSlot(target)).Where(slot => slot != null && !slot.Collapsed));

            for (int i = 0; i < this.workArea.Count; i++)
            {
                try
                {
                    var selected = this.Select(workArea);

                    if (selected != null)
                    {
                        selected.Collapse();
                    }
                    
                    Order.Enqueue(selected);
                }
                catch (CollapseFailedException)
                {

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