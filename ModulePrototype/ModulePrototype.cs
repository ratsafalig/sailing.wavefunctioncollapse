using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System;

namespace Sailing.WaveFunctionCollapse
{
    public partial class ModulePrototype : MonoBehaviour
    {
        [System.Serializable]
        public abstract class FaceDetails
        {
            public bool Walkable;

            public int Connector;

            public virtual void ResetConnector()
            {
                this.Connector = 0;
            }

            public ModulePrototype[] ExcludedNeighbours;

            public bool EnforceWalkableNeighbor = false;

            public bool IsOcclusionPortal = false;
        }

        [System.Serializable]
        public class HorizontalFaceDetails : FaceDetails
        {
            public bool Symmetric = true;
            public bool Flipped = true;

            public override string ToString()
            {
                return this.Connector.ToString() + (this.Symmetric ? "s" : (this.Flipped ? "F" : ""));
            }

            public override void ResetConnector()
            {
                base.ResetConnector();
                this.Symmetric = false;
                this.Flipped = false;
            }
        }

        [System.Serializable]
        public class VerticalFaceDetails : FaceDetails
        {
            public bool Invariant = true;
            public int Rotation = 0;

            public override string ToString()
            {
                return this.Connector.ToString() + (this.Invariant ? "i" : (this.Rotation != 0 ? "_bcd".ElementAt(this.Rotation).ToString() : ""));
            }

            public override void ResetConnector()
            {
                base.ResetConnector();
                this.Invariant = false;
                this.Rotation = 0;
            }
        }

        public float Probability = 1.0f;
        public bool Spawn = true;
        public bool IsInterior = false;

        public HorizontalFaceDetails Forward;
        public HorizontalFaceDetails Back;
        public HorizontalFaceDetails Left;
        public HorizontalFaceDetails Right;
        public VerticalFaceDetails Up;
        public VerticalFaceDetails Down;

        public FaceDetails[] Faces
        {
            get
            {
                return new FaceDetails[] {
                this.Left,
                this.Down,
                this.Back,
                this.Right,
                this.Up,
                this.Forward
            };
            }
        }

        public Mesh GetMesh(bool createEmptyFallbackMesh = true)
        {
            var meshFilter = this.GetComponent<MeshFilter>();
            if (meshFilter != null && meshFilter.sharedMesh != null)
            {
                return meshFilter.sharedMesh;
            }
            if (createEmptyFallbackMesh)
            {
                var mesh = new Mesh();
                return mesh;
            }
            return null;
        }

        /// <summary>
        /// Compare Rotated Variants
        /// check if hexahedron's rotation r1 is identical to rotation r2
        ///  -----
        /// /    /|
        /// ----/ |
        /// |   | |
        /// |   | |
        /// ----|/
        /// </summary>
        public bool CompareRotatedVariants(int r1, int r2)
        {

            // if up & down face isn't invariant, return false
            if (!(this.Faces[Orientations.UP] as VerticalFaceDetails).Invariant || !(this.Faces[Orientations.DOWN] as VerticalFaceDetails).Invariant)
            {
                return false;
            }

            for (int i = 0; i < 4; i++)
            {
                var face1 = this.Faces[Orientations.Rotate(Orientations.HorizontalDirections[i], r1)] as HorizontalFaceDetails;
                var face2 = this.Faces[Orientations.Rotate(Orientations.HorizontalDirections[i], r2)] as HorizontalFaceDetails;

                if (face1.Connector != face2.Connector)
                {
                    return false;
                }

                if (!face1.Symmetric && !face2.Symmetric && face1.Flipped != face2.Flipped)
                {
                    return false;
                }
            }

            return true;
        }

        void Update() { }

        void Reset()
        {
            this.Up = new VerticalFaceDetails();
            this.Down = new VerticalFaceDetails();
            this.Right = new HorizontalFaceDetails();
            this.Left = new HorizontalFaceDetails();
            this.Forward = new HorizontalFaceDetails();
            this.Back = new HorizontalFaceDetails();

            foreach (var face in this.Faces)
            {
                face.ExcludedNeighbours = new ModulePrototype[] { };
            }
        }
    }
}