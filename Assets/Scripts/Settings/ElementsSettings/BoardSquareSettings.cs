using UnityEngine;
using System;

namespace Game.Settings
{
    [Serializable]
    public class BoardSquareSettings
    {
        [SerializeField]
        private SquareType type; 
        [SerializeField]
        private GameObject square;

        public bool IsType(SquareType type) { return this.type == type;  }
        public GameObject Square { get { return square; } }
    }
}