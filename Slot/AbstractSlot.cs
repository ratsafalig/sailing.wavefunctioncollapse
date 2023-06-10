#define SAILING_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sailing.WaveFunctionCollapse
{
    public abstract class AbstractSlot
    {
        public abstract void Collapse();
        public abstract void Update(ModuleSet modulesToSet);
        public abstract AbstractSlot GetNeighbor(int direction);
    }
}