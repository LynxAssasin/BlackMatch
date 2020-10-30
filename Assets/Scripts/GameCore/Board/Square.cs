using UnityEngine;
namespace Game.GameCore
{
    public class Square
    {
        private SquareType type;       
        private Element insideObject;
        private GameObject squareObject;
        private SquareVisual squareVisual; 

        private bool outsideNull;
        private bool moving;

        private int x;
        private int y;

        public SquareType Type { get { return type; } set { type = value; } }

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Element InsideElement
        {
            get
            {
                return insideObject;
            }
            set
            {
                insideObject = value;
                if (squareVisual != null)
                {
                    SquareVisual.ElementObject = value; 
                } 
            }
        }

        public bool OutsideNull { get { return outsideNull; } set { outsideNull = value; } }

        public GameObject SquareObject
        {
            get
            {
                return squareObject;
            }
            set {
                squareObject = value;
                squareVisual = value.GetComponent<SquareVisual>();              
            }
        }

        public SquareVisual SquareVisual { get { return squareVisual; } set { squareVisual = value; } }
        public bool Moving { get { return moving; } set { moving = value; } }
        public int X { get { return x; } }
        public int Y { get { return y; } }

        public bool isContainer()
        {
            return (type == SquareType.Container);
        }

        public bool Empty()
        {
            return (insideObject == null);
        }
 
    }
}