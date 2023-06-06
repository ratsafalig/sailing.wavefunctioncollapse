using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

namespace Sailing.WaveFunctionCollapse
{
    [CreateAssetMenu(menuName = "Wave Function Collapse/Sailing Module Prototype Config", fileName = "config.asset")]
    public class ModulePrototypeConfig : ScriptableObject
    {
        public ModulePrototype FORWARD;
        public ModulePrototype BACK;
        public ModulePrototype LEFT;
        public ModulePrototype RIGHT;
        public ModulePrototype UP;
        public ModulePrototype DOWN;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
