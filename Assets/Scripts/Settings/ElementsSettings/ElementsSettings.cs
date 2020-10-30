using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.GameCore; 
namespace Game.Settings
{
    public class ElementsSettings : ScriptableObject
    {
        [SerializeField]
        private List<BoardSquareSettings> boardSquare;


        public BoardSquareSettings GetBoardSquare(SquareType type)
        {
            for (int i = 0; i < boardSquare.Count; i++)
            {
                if (boardSquare[i].IsType(type))
                {
                    return (boardSquare[i]);
                }
            }
            return null;
        }

        [SerializeField]
        private List<Element> elements;
        public List<Element> Elements { get { return elements; } }

        [SerializeField]
        private float elementShowTime;
        public float ElementShowTime { get { return elementShowTime; } }

        [SerializeField]
        private float elementMoveDownTime;
        public float ElementMoveDownTime { get { return elementMoveDownTime; } }

        [SerializeField]
        private float elementCoefSize;
        public float ElementCoefSize { get { return elementCoefSize; } }

        [SerializeField]
        private float elementDeleteTime;
        public float ElementDeleteTime { get { return elementDeleteTime; } }
    }
}
