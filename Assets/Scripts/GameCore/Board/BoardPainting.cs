using System.Collections.Generic;
using UnityEngine;
using Game.Tools;
using Game.Settings;
using System;
namespace Game.GameCore
{
    //RenameToPaint
    public class BoardPainting : MonoBehaviour
    {
        private bool enableToCheck;
        private List<Square> squareChain;
        private List<Square> additional = new List<Square>(); 
        private ColorElementType? startType;

        private const int MIN_ELEMENTS_TO_DELETE_IN_CHAIN = 3;
        private const int MIN_DISTANCE_TO_CLEAR_BEFORE_HEAD = 2;
        private Timer timer;

        private  float deleteTime = 0.1f;
        private int countDeleted = 0;
        private int xSize;
        private int ySize;

        public void Init(ElementsSettings elementSettings, int xSize, int ySize)
        {
            this.xSize = xSize;
            this.ySize = ySize;
            deleteTime = elementSettings.ElementDeleteTime;
            CommandKeeper.Instance.EnablePainting += Enable;
            CommandKeeper.Instance.CheckToDeleteElements += CheckToDeleteElements;
            CommandKeeper.Instance.StopPainting += ClearPainting;
            CommandKeeper.Instance.DeletePaintedElements += DeletePaintedElements;

            timer = gameObject.AddComponent<Timer>();
            ClearPainting();
        }

        private void Enable(bool enable)
        {
            enableToCheck = enable;
        }


        private void Update()
        {
            if (enableToCheck)
            {

                Square sq = CommandKeeper.Instance.AskSquareUnderFinger(Input.mousePosition, squareChain.Count > 0);

                if (sq != null)
                {
                    if (CheckThatSquareNear(sq))
                    {
                        TryToPaintSquare(sq);
                        CheckLines(); 
                    }
                }
            }
        }

        private void CheckLines()
        {
            foreach (var sq in additional)
            {
                ClearSquare(sq);  
            }

            additional = new List<Square>();
            int horizontal = 0;
            int vertical = 0; 
            foreach (var sq in squareChain)
            {
                if (sq.InsideElement.LineType == LineType.Horizontal)
                {
                    horizontal++;
                }
                if (sq.InsideElement.LineType == LineType.Vertical)
                {
                    vertical++;
                }
                if (sq.InsideElement.LineType == LineType.Both)
                {
                    horizontal++;
                    vertical++;
                }
            }

            if ((horizontal > 0) || (vertical > 0)) 
            {
                List<int> hIndexes = new List<int>();
                List<int> vIndexes = new List<int>();

                for (int i = squareChain.Count - 1; i >= 0; i--)
                {
                    if (!hIndexes.Contains(squareChain[i].Y))
                    {
                        if (hIndexes.Count == horizontal) break;
                        hIndexes.Add(squareChain[i].Y);
                    }
                }
                for (int i = squareChain.Count - 1; i >= 0; i--)
                {
                    if (!vIndexes.Contains(squareChain[i].X))
                    {
                        if (vIndexes.Count == vertical) break;
                        vIndexes.Add(squareChain[i].X);
                    }
                }

                if (hIndexes.Count > 0) {
                    hIndexes.Sort();
                    if (hIndexes.Count < horizontal)
                    {
                        int left = hIndexes[0];
                        for (int k = 0; k < left; k++)
                        {
                            hIndexes.Add(k);
                            if (hIndexes.Count == horizontal) return; 
                        }
                    }
                    hIndexes.Sort();
                    if (hIndexes.Count < horizontal)
                    {
                        int right = hIndexes[hIndexes.Count - 1];
                        for (int k = right; k < ySize; k++)
                        {
                            hIndexes.Add(k);
                            if (hIndexes.Count == horizontal) return;
                        }
                    }
                }

                if (vIndexes.Count > 0)
                {
                    vIndexes.Sort();
                    if (vIndexes.Count < vertical)
                    {
                        int up = vIndexes[0];
                        for (int k = 0; k < up; k++)
                        {
                            vIndexes.Add(k);
                            if (vIndexes.Count == vertical) return;
                        }
                    }
                    vIndexes.Sort();
                    if (vIndexes.Count < vertical)
                    {
                        int bottom = vIndexes[hIndexes.Count - 1];
                        for (int k = bottom; k < xSize; k++)
                        {
                            vIndexes.Add(k);
                            if (vIndexes.Count == vertical) return;
                        }
                    }
                }

                foreach (int indexH in hIndexes)
                {
                    RecursivelyHorizontalLinePaintCheck(indexH);
                }

                foreach (int indexV in vIndexes)
                {
                    RecursivelyVerticalLinePaintCheck(indexV);  
                }
            }
        }

        void RecursivelyHorizontalLinePaintCheck(int y)
        {
            for (int i = 0; i < xSize; i++)
            {
                Square sq = CommandKeeper.Instance.GetSquareByIndex(i, y);
                if (sq.Type == SquareType.Container)
                {
                    if ((!squareChain.Contains(sq)) && (!additional.Contains(sq)))
                    {
                        SquareVisual sv = sq.SquareVisual;
                        if (sv != null)
                        {
                             sv.Paint();
                             additional.Add(sq);
                             if ((sq.InsideElement.LineType == LineType.Horizontal) ||
                                (sq.InsideElement.LineType == LineType.Vertical) ||
                                (sq.InsideElement.LineType == LineType.Both))
                             {
                                  RecursivelyVerticalLinePaintCheck(i);
                             }
                        }
                    }
                }
            }
        }

        void RecursivelyVerticalLinePaintCheck(int x)
        {
            for (int i = 0; i < ySize; i++)
            {
                Square sq = CommandKeeper.Instance.GetSquareByIndex(x, i);
                if (sq.Type == SquareType.Container)
                {
                    if ((!squareChain.Contains(sq)) && (!additional.Contains(sq)))
                    {
                        SquareVisual sv = sq.SquareVisual;
                        if (sv != null)
                        {
                             sv.Paint();
                             additional.Add(sq);
                             if ((sq.InsideElement.LineType == LineType.Horizontal) || 
                                (sq.InsideElement.LineType == LineType.Vertical) ||
                                (sq.InsideElement.LineType == LineType.Both))
                             {
                                RecursivelyHorizontalLinePaintCheck(i); 
                             }
                          
                        }
                    }
                }
            }
        }

        private bool CheckToDeleteElements()
        {
            if(squareChain.Count >= MIN_ELEMENTS_TO_DELETE_IN_CHAIN)
            {
                return true; 
            }
            return false;
        }


        private void DeletePaintedElements(Action callback)
        {
            if (squareChain.Count > 0)
            {
                List<Square> allElements = new List<Square>(squareChain);
                allElements.AddRange(additional);
                foreach (var el in allElements)
                {
                    countDeleted++;
                    var colorType = el.SquareVisual.GetColorElementType().Value;
                    DeleteElement(el);
                    CommandKeeper.Instance.ScoreDeletedElement(countDeleted, colorType);
                }
                squareChain = new List<Square>();
                allElements = new List<Square>();
                countDeleted = 0;
                callback();
            }
            else
            {
                countDeleted = 0;
                callback();
            }
        }

        private void DeleteElement(Square sq)
        {
            ClearSquare(sq);
            CommandKeeper.Instance.DeleteElement(sq.X, sq.Y);
        }

        private bool CheckThatSquareNear(Square sq)
        {
            if (squareChain.Count > 0)
            {
                Square last = squareChain[squareChain.Count - 1];
                if ((sq.X != last.X) || (sq.Y != last.Y))
                {
                    if((Mathf.Abs(sq.X - last.X) <= 1) && (Mathf.Abs(sq.Y - last.Y) <= 1))
                    {                    
                        return true; 
                    }
                }
            }
            else
            {
                return true;
            }
            return false; 
        }


        private void TryToPaintSquare(Square sq)
        {
            SquareVisual sv = sq.SquareVisual;
            if (sv != null)
            {
                ColorElementType? checkType = sv.GetColorElementType();
                if (checkType != null)
                {
                    if (squareChain.Count > 0)
                    {
                        if (checkType == startType)
                        {
                            if (sv.Painted && !additional.Contains(sq))
                            {
                                int pInd = squareChain.IndexOf(sq);
                                if (Mathf.Abs(pInd - (squareChain.Count - 1)) < MIN_DISTANCE_TO_CLEAR_BEFORE_HEAD)
                                {
                                    ClearChainBeforeSquare(sq);
                                    sv.SetElementAsHead();
                                }
                            }
                            else
                            {
                                squareChain[squareChain.Count - 1].SquareVisual.SetElementAsTail();
                                if (additional.Contains(sq)) additional.Remove(sq); 
                                PaintElement(sq);
                            }
                        }
                    }
                    else
                    {
                        startType = checkType;
                        PaintElement(sq);
                    }
                }
            }
        }

        private void ClearChainBeforeSquare(Square sq)
        {
            for (int i = squareChain.Count - 1; i >= 0; i--)
            {
                if (squareChain[i] != sq)
                {
                    ClearSquare(squareChain[i]);
                    squareChain.RemoveAt(i);  
                }
                else
                {
                    break;
                }
            }
        }

        private void ClearPainting()
        {
            if (squareChain != null)
            {
                for (int i = 0; i < squareChain.Count; i++)
                {
                    ClearSquare(squareChain[i]);
                }             
            }

            if (additional != null)
            {
                for (int i = 0; i < additional.Count; i++)
                {
                    ClearSquare(additional[i]);
                }
            }

            squareChain = new List<Square>();
            additional = new List<Square>(); 
            startType = null;
        }

        private void PaintElement(Square sq)
        {
            squareChain.Add(sq);
            sq.SquareVisual.Paint();
            sq.SquareVisual.SetElementAsHead();
        }

        private void ClearSquare(Square sq)
        {
            sq.SquareVisual.SetToDefault();
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.EnablePainting -= Enable;
            CommandKeeper.Instance.CheckToDeleteElements -= CheckToDeleteElements;
            CommandKeeper.Instance.StopPainting -= ClearPainting;
            CommandKeeper.Instance.DeletePaintedElements -= DeletePaintedElements;
        }

    }
}