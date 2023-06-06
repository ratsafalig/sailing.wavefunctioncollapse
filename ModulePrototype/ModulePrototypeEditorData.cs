using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Sailing.WaveFunctionCollapse
{
    public class ModulePrototypeEditorData
    {
        public readonly ModulePrototype ModulePrototype;

        private readonly ModulePrototype[] prototypes;

        private readonly Dictionary<ModulePrototype, Mesh> meshes;

        public struct ConnectorHint
        {
            public readonly List<Mesh> meshes;
            public readonly List<int> Rotations;

            public ConnectorHint(List<int> rotations, List<Mesh> meshes)
            {
                this.Rotations = rotations;
                this.meshes = meshes;
            }
        }

        public ModulePrototypeEditorData(ModulePrototype modulePrototype)
        {
            this.ModulePrototype = modulePrototype;
            this.prototypes = modulePrototype.transform.parent.GetComponentsInChildren<ModulePrototype>();
            this.meshes = new Dictionary<ModulePrototype, Mesh>();
        }

        private Mesh getMesh(ModulePrototype modulePrototype)
        {
            if (this.meshes.ContainsKey(modulePrototype))
            {
                return this.meshes[modulePrototype];
            }
            var mesh = modulePrototype.GetMesh(false);
            this.meshes[modulePrototype] = mesh;
            return mesh;
        }

        public ConnectorHint GetConnectorHint(int direction)
        {
            var face = this.ModulePrototype.Faces[direction];
            List<Mesh> meshes = new List<Mesh>();
            List<int> rotations = new List<int>();

            if (face is ModulePrototype.HorizontalFaceDetails)
            {
                var horizontalFace = face as ModulePrototype.HorizontalFaceDetails;

                foreach (var prototype in this.prototypes)
                {
                    if (prototype == this.ModulePrototype || face.ExcludedNeighbours.Contains(prototype))
                    {
                        continue;
                    }
                    for (int rotation = 0; rotation < 4; rotation++)
                    {
                        var otherFace = prototype.Faces[Orientations.Rotate(direction, rotation + 2)] as ModulePrototype.HorizontalFaceDetails;
                        if (otherFace.ExcludedNeighbours.Contains(this.ModulePrototype))
                        {
                            continue;
                        }
                        if (otherFace.Connector == face.Connector && ((horizontalFace.Symmetric && otherFace.Symmetric) || otherFace.Flipped != horizontalFace.Flipped))
                        {
                            rotations.Add(rotation);
                            meshes.Add(this.getMesh(prototype));
                        }
                    }
                }
            }

            if(rotations.Count != 0 && meshes.Count != 0){
                return new ConnectorHint(rotations, meshes);
            }

            if (face is ModulePrototype.VerticalFaceDetails)
            {
                var verticalFace = face as ModulePrototype.VerticalFaceDetails;

                foreach (var prototype in this.prototypes)
                {
                    if (prototype == this.ModulePrototype || face.ExcludedNeighbours.Contains(prototype))
                    {
                        continue;
                    }
                    var otherFace = prototype.Faces[(direction + 3) % 6] as ModulePrototype.VerticalFaceDetails;
                    if (otherFace.ExcludedNeighbours.Contains(this.ModulePrototype) || otherFace.Connector != face.Connector)
                    {
                        continue;
                    }

                    rotations.Add(verticalFace.Rotation - otherFace.Rotation);
                    meshes.Add(this.getMesh(prototype));
                }
            }

            if(rotations.Count != 0 && meshes.Count != 0){
                return new ConnectorHint(rotations, meshes);
            }

            return new ConnectorHint();
        }
    }
}