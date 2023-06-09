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
        public readonly QueueDictionary<Vector3Int, ModuleSet> RemovalQueue;
        private HashSet<Slot> workArea;
        public readonly Queue<Slot> BuildQueue;

        private int backtrackBarrier;
        private int backtrackAmount = 0;

        public AbstractMap()
        {
            InfiniteMap.Random = new System.Random();

            this.History = new RingBuffer<HistoryItem>(AbstractMap.HISTORY_SIZE);
            this.History.OnOverflow = item => item.Slot.Forget();
            this.RemovalQueue = new QueueDictionary<Vector3Int, ModuleSet>(() => new ModuleSet());
            this.BuildQueue = new Queue<Slot>();
            this.backtrackBarrier = 0;
        }

        public abstract Slot GetSlot(Vector3Int position);

        public abstract IEnumerable<Slot> GetAllSlots();

        public void Undo(int steps)
        {
            while (steps > 0 && this.History.Any())
            {
                var item = this.History.Pop();

                foreach (var slotAddress in item.RemovedModules.Keys)
                {
                    this.GetSlot(slotAddress).AddModules(item.RemovedModules[slotAddress]);
                }

                item.Slot.Module = null;
                steps--;
            }
            if (this.History.Count == 0)
            {
                this.backtrackBarrier = 0;
            }
        }
    }
}