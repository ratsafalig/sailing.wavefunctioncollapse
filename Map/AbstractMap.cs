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
        public AbstractMap() { }

        public abstract Slot GetSlot(Vector3 position);

        public abstract IEnumerable<Slot> GetAllSlots();

        public abstract Slot Select(IEnumerable<Slot> slots);

        public abstract void Collapse();
    }
}