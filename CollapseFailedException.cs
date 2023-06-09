using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sailing.WaveFunctionCollapse
{

    public class CollapseFailedException : Exception
    {
        public readonly Slot Slot;

        public CollapseFailedException(Slot slot)
        {
            this.Slot = slot;
        }
    }
}
