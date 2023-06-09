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
        private Dictionary<Vector3Int, Slot> slots;

        public readonly int Height;

        public Vector3Int rangeLimitCenter;
        public int RangeLimit = 80;

        private TilingMap defaultColumn;

        public InfiniteMap(int height) : base()
        {
            this.Height = height;
            this.slots = new Dictionary<Vector3Int, Slot>();
            this.defaultColumn = new TilingMap(new Vector3Int(1, height, 1));

            if (ModuleData.Current == null || ModuleData.Current.Length == 0)
            {
                throw new InvalidOperationException("Module data was not available, please create module data first.");
            }
        }

        public override Slot GetSlot(Vector3Int position)
        {
            if (position.y >= this.Height || position.y < 0)
            {
                return null;
            }

            if (this.slots.ContainsKey(position))
            {
                return this.slots[position];
            }

            if (this.IsOutsideOfRangeLimit(position))
            {
                return null;
            }

            this.slots[position] = new Slot(position, this, this.defaultColumn.GetSlot(position));
            return this.slots[position];
        }

        public bool IsOutsideOfRangeLimit(Vector3Int position)
        {
            return (position - this.rangeLimitCenter).magnitude > this.RangeLimit;
        }

        public override IEnumerable<Slot> GetAllSlots()
        {
            return this.order;
        }
    }
}