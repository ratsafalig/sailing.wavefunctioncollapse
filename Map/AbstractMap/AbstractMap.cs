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
        public const float BLOCK_SIZE = 1f;
        public const int HISTORY_SIZE = 3000;

        public static System.Random Random;

        public readonly RingBuffer<HistoryItem> History;
        
        private HashSet<Slot> workArea;

        public AbstractMap()
        {
            InfiniteMap.Random = new System.Random();
            this.History = new RingBuffer<HistoryItem>(AbstractMap.HISTORY_SIZE);
        }

        public abstract Slot GetSlot(Vector3Int position);

        public abstract IEnumerable<Slot> GetAllSlots();
    }
}