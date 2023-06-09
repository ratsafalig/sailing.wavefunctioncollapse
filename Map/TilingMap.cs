using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Sailing.WaveFunctionCollapse
{
    /// <summary>
    /// A finite sized map that uses horizontal world wrapping.
    /// That means you can horizontally tile copies of this map and the edges will match
    /// </summary>
    public class TilingMap : AbstractMap
    {
        public readonly Vector3Int Size;

        private readonly Slot[,,] slots;

        public TilingMap(Vector3Int size) : base()
        {
            this.Size = size;
            this.slots = new Slot[size.x, size.y, size.z];

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        this.slots[x, y, z] = new Slot(new Vector3Int(x, y, z), this);
                    }
                }
            }
        }

        public override Slot GetSlot(Vector3Int position)
        {
            if (position.y < 0 || position.y >= this.Size.y)
            {
                return null;
            }
            return this.slots[
                position.x % this.Size.x + (position.x % this.Size.x < 0 ? this.Size.x : 0),
                position.y,
                position.z % this.Size.z + (position.z % this.Size.z < 0 ? this.Size.z : 0)];
        }

        public override IEnumerable<Slot> GetAllSlots()
        {
            for (int x = 0; x < this.Size.x; x++)
            {
                for (int y = 0; y < this.Size.y; y++)
                {
                    for (int z = 0; z < this.Size.z; z++)
                    {
                        yield return this.slots[x, y, z];
                    }
                }
            }
        }
    }
}