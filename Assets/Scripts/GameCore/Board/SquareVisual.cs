using UnityEngine;
namespace Game.GameCore
{
    public class SquareVisual : MonoBehaviour
    {
        [SerializeField]
        private Renderer backRenderer;
        private Color defaultColor;
        private Element elementObject;

        private bool painted; 
        public bool Painted { get { return painted;  } }
        public Element ElementObject { set { elementObject = value; } }

        private void Start()
        {
            defaultColor = backRenderer.material.color; 
        }

        public bool CanBePaint()
        {
            if (elementObject != null)
            {
                return (elementObject is ColorElement);
            }
            else
            {
                return false;
            }
        }

        public ColorElementType? GetColorElementType()
        {
            if (CanBePaint())
            {
                ColorElement cElement = (ColorElement)elementObject;
                return cElement.Type; 
            }
            return null;
        }

        public void Paint()
        {   
            if(CanBePaint())
            {
                ColorElement cElement = (ColorElement)elementObject;
                Color clr = cElement.ElementColor;
                cElement.Paint();
                backRenderer.material.color = clr;
                painted = true; 
            }
        }

        public void SetElementAsHead()
        {
            if (CanBePaint())
            {
                ColorElement cElement = (ColorElement)elementObject;
                cElement.MakePulse(); 
            }
        }

        public void SetElementAsTail()
        {
            if (CanBePaint())
            {
                ColorElement cElement = (ColorElement)elementObject;
                cElement.StopPuls();
            }
        }

        public void SetToDefault()
        {
            if (CanBePaint())
            {
                ColorElement cElement = (ColorElement)elementObject;
                SetElementAsTail();
                cElement.SetToDefault();
                backRenderer.material.color = defaultColor;
                painted = false;
            }
        }



    }
}