using UnityEngine;
using DG.Tweening;

namespace Game.GameCore
{
    public class ColorElement : Element
    {
        [SerializeField]
        protected ColorElementType type;
        [SerializeField]
        protected Sprite elementSprite; 
        [SerializeField]
        protected Renderer elementRenderer;
        [SerializeField]
        protected Color elementColor;
        [SerializeField]
        protected Color elementColorSelected;
        [SerializeField]
        protected float tweenPulseSpeed = 0.5f;
        [SerializeField]
        protected float tweenScale = 1f;

        [SerializeField]
        protected GameObject hObject;
        [SerializeField]
        protected GameObject vObject;

        public Sprite ElementSprite { get { return elementSprite; } }
        public Color ElementColor { get { return elementColor; } }
        public ColorElementType Type { get { return type; } }

        private Tweener pulseTween;

        public void Paint()
        {
            elementRenderer.material.color = elementColorSelected;
        }

        public void SetToDefault()
        {
            elementRenderer.material.color = elementColor;
        }

        public override void SetLineType(LineType type)
        {
            base.SetLineType(type);
            if (type == LineType.Horizontal) hObject.SetActive(true);
            if (type == LineType.Vertical) vObject.SetActive(true);
            if (type == LineType.Both)
            {
                hObject.SetActive(true);
                vObject.SetActive(true);
            }
        }

        public void MakePulse()
        {
            pulseTween = gameObject.transform.DOScale(tweenScale, tweenPulseSpeed).SetLoops(-1, LoopType.Yoyo);  
        }
        public void StopPuls()
        {
            pulseTween.Kill();
            gameObject.transform.localScale = Vector3.one * scaleSize;
        }
    }
}