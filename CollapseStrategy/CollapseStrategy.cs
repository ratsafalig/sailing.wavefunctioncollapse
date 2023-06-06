using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Sailing.WaveFunctionCollapse{
    public class CollapseStrategy{
        public Slot Select(IEnumerable<Slot> slots){
            Slot selected = null;
            float minEntropy = float.PositiveInfinity;
            foreach (var slot in slots)
            {
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