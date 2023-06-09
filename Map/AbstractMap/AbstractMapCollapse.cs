using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;

namespace Sailing.WaveFunctionCollapse
{
    public abstract partial class AbstractMap
    {
        public Queue<Slot> Order;

        public void Collapse(Vector3Int start, Vector3Int size, bool showProgress = false)
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
            this.Collapse(targets, showProgress);
        }

        private void Collapse(IEnumerable<Vector3Int> targets, bool showProgress = false)
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

        public Slot Select(IEnumerable<Slot> slots)
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