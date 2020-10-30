using UnityEngine;
using DG.Tweening; 
namespace Game.GameCore
{
    public class Element : MonoBehaviour
    {
        private int lastMoveIndex;
        private LineType lineType = LineType.NotLine;
        public LineType LineType { get { return lineType; } }
        public int LastMoveIndex { set { lastMoveIndex = value; } get { return lastMoveIndex; } }

        protected float scaleSize; 
        public void ShowElement(float time)
        {
            scaleSize = gameObject.transform.localScale.x;
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(scaleSize, time);
        }
        public virtual void SetLineType(LineType type)
        {
            lineType = type; 
        }
        public void MoveDown(Vector3 newPosition, float time)
        {
            gameObject.transform.DOMove(newPosition, time).SetEase(Ease.Linear);
        }

    }
}