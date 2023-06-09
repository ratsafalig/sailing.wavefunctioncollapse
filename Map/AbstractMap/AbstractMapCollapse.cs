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
        public void Collapse(Vector3Int start, Vector3Int size, bool showProgress = false)
        {
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

        public void Collapse(IEnumerable<Vector3Int> targets, bool showProgress = false)
        {
#if UNITY_EDITOR
            try
            {
#endif
                this.workArea = new HashSet<Slot>(targets.Select(target => this.GetSlot(target)).Where(slot => slot != null && !slot.Collapsed));

                for(int i = 0; i < this.workArea.Count; i++)
                {
                    try
                    {
                        var selected = this.Select(workArea);

                        if(selected != null){
                            selected.Collapse();
                        }
                    }
                    catch (CollapseFailedException)
                    {

                    }

#if UNITY_EDITOR
                    if (showProgress && this.workArea.Count % 20 == 0)
                    {
                        if (EditorUtility.DisplayCancelableProgressBar("Collapsing area... ", this.workArea.Count + " left...", 1f - (float)this.workArea.Count() / targets.Count()))
                        {
                            EditorUtility.ClearProgressBar();
                            throw new Exception("Map generation cancelled.");
                        }
                    }
#endif
                }

#if UNITY_EDITOR
                if (showProgress)
                {
                    EditorUtility.ClearProgressBar();
                }
            }
            catch (Exception exception)
            {
                if (showProgress)
                {
                    EditorUtility.ClearProgressBar();
                }
                Debug.LogWarning("Exception in world generation thread at" + exception);
                throw exception;
            }
#endif
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