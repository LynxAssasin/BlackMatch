using System;
using UnityEngine;
using System.Collections.Generic;
namespace Game.Settings
{
    [Serializable]
    public class LevelBoard
    {
        [SerializeField]
        private int xSize;
        [SerializeField]
        private int ySize; 

        public int XSize { get { return xSize; } }
        public int YSize { get { return ySize; } }

        public List<BoardSquareInBoard> boardSquareSettings;
    }

    [Serializable]
    public class BoardSquareInBoard
    {
        [SerializeField]
        private SquareType type;
        [SerializeField]
        private int x;
        [SerializeField]
        private int y;

        public SquareType Type { get { return type; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }

    }
}