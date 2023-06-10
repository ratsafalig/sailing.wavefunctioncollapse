using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Sailing.WaveFunctionCollapse
{
    public class Orientations
    {
        public const int LEFT = 0;
        public const int DOWN = 1;
        public const int BACK = 2;
        public const int RIGHT = 3;
        public const int UP = 4;
        public const int FORWARD = 5;

        public static Vector3Int[] Direction
        {
            get
            {
                var vectors = new Vector3[] {
                    Vector3.left,
                    Vector3.down,
                    Vector3.back,
                    Vector3.right,
                    Vector3.up,
                    Vector3.forward
                };
                return vectors.Select(vector => Vector3Int.RoundToInt(vector)).ToArray();
            }
        }
    }
}