using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Settings;
using Game.Tools;
using System;
namespace Game.GameCore
{

    public class Board: MonoBehaviour
    {
        private int xSize;
        private int ySize; 
        private Square[,] squareMatrix;
        private List<Element> elements; 

        private float squareSizeScreen;
        private Vector2 leftUpScreen;
        private Vector2 rightDownScreen;
        private ElementsSettings elementSettings;
        private int maxElementTypes;

        private Timer myTimer;

        private const float ELEMENT_OFFSET = 0.03f;

        #region Init

        public void Init(Square[,] squareMatrix, ElementsSettings elementSettings, int xSize, int ySize,  int maxElementTypes)
        {
            this.squareMatrix = squareMatrix;
            this.elementSettings = elementSettings;        
            this.maxElementTypes = maxElementTypes;
            this.xSize = xSize;
            this.ySize = ySize; 
            CommandKeeper.Instance.CreateNewElements += CreateNewElements;
            CommandKeeper.Instance.AskSquareUnderFinger += ReturnSquareByCoordinates;
            CommandKeeper.Instance.DeleteElement += DeleteElement;
            CommandKeeper.Instance.GetSquareByIndex += GetSquareByIndex; 
            elements = elementSettings.Elements.GetRange(0, maxElementTypes);
            myTimer = gameObject.AddComponent<Timer>();
        }

        public void SetupScreenSize(float squareSizeScreen, Vector2 leftUpScreen, Vector2 rightDownScreen)
        {
            this.squareSizeScreen = squareSizeScreen;
            this.leftUpScreen = leftUpScreen;
            this.rightDownScreen = rightDownScreen;
        }

        #endregion

        #region SquareByXY 
        private const float smallOffsetPercent = 0.1f; 
        private Square ReturnSquareByCoordinates(Vector2 pos, bool withOffsets = false)
        {
            if ((pos.x >= leftUpScreen.x) && (pos.x <= rightDownScreen.x) && (pos.y <= leftUpScreen.y ) && (pos.y >= rightDownScreen.y))
            {
                Vector2 positionOnBoard = pos - leftUpScreen;

                int x = (int)(positionOnBoard.x / squareSizeScreen);
                int y = -(int)(positionOnBoard.y / squareSizeScreen);
                if (withOffsets)
                {
                    float xOfset = positionOnBoard.x / squareSizeScreen - (float)x;
                    float yOfset = -positionOnBoard.y / squareSizeScreen - (float)y;
                    if ((xOfset < smallOffsetPercent) || ((1 - xOfset) < smallOffsetPercent) || (yOfset < smallOffsetPercent) || ((1 - yOfset) < smallOffsetPercent))
                    {
                        return null;
                    }
                    return squareMatrix[x, y];
                }
                else
                {
                    return squareMatrix[x, y];
                }
            }
            return null; 
        }

        #endregion

        #region CreateElements

        private void CreateNewElements(Action finish)
        {
            MoveElementsStep();
            bool created = CreateElementsOnFirstRow();
            if (created)
            {
                myTimer.StartTimer(elementSettings.ElementShowTime, () =>
                {                   
                    CreateNewElements(finish);
                });
            }
            else
            {
                bool allMovesFinished = AllMovesFinished();
                if (allMovesFinished)
                {
                    ClearMovingIndex();
                    finish();
                }
                else
                {
                    ClearMovingIndex();
                    myTimer.StartTimer(elementSettings.ElementShowTime, () =>
                    {
                        CreateNewElements(finish);
                    });
                }
            } 
        }

        private Square GetSquareByIndex(int x, int y)
        {
            return squareMatrix[x, y];
        }

        private bool CreateElementsOnFirstRow()
        {
            bool created = false; 
            for (int i = 0; i < xSize; i++)
            {
                if ((squareMatrix[i, 0].Empty()) && (squareMatrix[i, 0].isContainer())) 
                {
                    CreateRandomElement(i, 0);
                    created = true; 
                }
            }
            return created; 
        }

        private void CreateRandomElement(int x, int y)
        {
            //No range check!!
            Vector3 pos = squareMatrix[x, y].SquareObject.transform.position;
            int rndValue = UnityEngine.Random.Range(0, elements.Count);
            GameObject elementObj = (GameObject)Instantiate(elements[rndValue].gameObject);

            Element element = elementObj.GetComponent<Element>();
            squareMatrix[x, y].InsideElement = element;
            Transform sqParent = squareMatrix[x, y].SquareObject.transform;
            elementObj.transform.position = sqParent.position - Vector3.forward * ELEMENT_OFFSET;
            elementObj.transform.parent = sqParent;
            elementObj.transform.localScale = Vector3.one * elementSettings.ElementCoefSize;
            element.ShowElement(elementSettings.ElementShowTime);
            if (element is ColorElement) 
            {
                float rnd = UnityEngine.Random.value;
                if (rnd < 0.02) element.SetLineType(LineType.Both);
                if ((rnd >= 0.02) && (rnd < 0.06)) element.SetLineType(LineType.Horizontal);
                if ((rnd >= 0.06) && (rnd < 0.1)) element.SetLineType(LineType.Vertical);
            }
        }
        #endregion

        #region DeleteElement

        private void DeleteElement(int x, int y)
        {
            Square sq = squareMatrix[x, y];
            GameObject.Destroy(sq.InsideElement.gameObject);
            sq.InsideElement = null; 
        }

        #endregion

        #region MoveAlgoritm

        private void MoveElementsStep()
        {
            ClearMovingFlag();
            for (int j = ySize - 1; j >= 0; j--)
            {
                for (int i = 0; i < xSize; i++)
                {
                    CheckEmptySquareForMove(i, j);
                }
            }   
        }

        private bool CheckEmptySquareForMove(int x, int y)
        {
            if (EmptyContainer(x, y))
            {
                if( y > 0)
                {
                    Square sq = squareMatrix[x, y - 1];
                    if ((sq.Empty()) && (sq.isContainer()) && (!sq.Moving))
                    {
                        bool result = CheckEmptySquareForMove(x, y - 1);
                        if (!result)
                        {
                            if (CheckSquareToMove(x, y - 1, x, y)) return true;
                            if (CheckSquareToMove(x - 1, y - 1, x, y)) return true;
                            if (CheckSquareToMove(x + 1, y - 1, x, y)) return true;
                        }
                    }
                    else
                    {
                        if (CheckSquareToMove(x, y - 1, x, y)) return true;
                        if (CheckSquareToMove(x - 1, y - 1, x, y)) return true;
                        if (CheckSquareToMove(x + 1, y - 1, x, y)) return true;
                    }
                }
                return false;
            }
            return false;
        }

        private bool AllMovesFinished()
        {
            for (int j = ySize - 2; j >= 0; j--)
            {
                for (int i = 0; i < xSize; i++)
                {
                    Square sq = squareMatrix[i, j];

                    if ((!sq.Empty()) && (sq.isContainer()) && (!sq.Moving))
                    {
                        if (InBoard(i, j + 1))
                        {
                            if (squareMatrix[i, j + 1].isContainer() && squareMatrix[i, j + 1].Empty())
                            {
                                return false;
                            }
                        }
                        if (InBoard(i + 1, j - 1))
                        {
                            if (squareMatrix[i + 1, j + 1].isContainer() && squareMatrix[i + 1, j + 1].Empty())
                            {
                                return false;
                            }
                        }
                        if (InBoard(i - 1, j - 1))
                        {
                            if (squareMatrix[i - 1, j + 1].isContainer() && squareMatrix[i - 1, j + 1].Empty())
                            {
                                return false;
                            }
                        }
                    }
                    
                }
            }
            return true; 
        } 

        private bool NoEmptySquares()
        {
            for (int j = 0; j < ySize; j++)
            {
                for (int i = 0; i < xSize; i++)
                {
                   if(EmptyContainer(i, j))
                    {
                        return false;
                     
                    }
                }
            }
            
            return true; 
        }

        private bool EmptyContainer(int x, int y)
        {
            Square sq = squareMatrix[x, y];
            return ((sq.Empty()) && (sq.isContainer()));
        }

        private bool InBoard(int x, int y)
        {
            return ((x >= 0) && (x < xSize) && (y >= 0) && (y < ySize));
        }

        private bool CheckSquareToMove(int x, int y, int newX, int newY)
        {
            if (InBoard(x, y))
            {
                Square sq = squareMatrix[x, y];
                if ((!sq.Empty()) && (sq.isContainer()) && (!sq.Moving))
                {
                    Element el = sq.InsideElement.GetComponent<Element>();
                    //Do not move back
                    int moveIndex = (x - newX) + (y - newY) * 2;
                    if (el.LastMoveIndex != -moveIndex)
                    {
                        el.LastMoveIndex = moveIndex;
                        Square sqNew = squareMatrix[newX, newY];
                        sqNew.Moving = true;
                        MoveElement(x, y, newX, newY);
                        CheckEmptySquareForMove(x, y);

                        return true;
                    }
                }
            }
            return false; 
        }


        private void MoveElement(int x, int y, int newX , int newY)
        {
            Element element = squareMatrix[x, y].InsideElement;
            squareMatrix[newX, newY].InsideElement = element;
            squareMatrix[x, y].InsideElement = null;
            Transform sqParent = squareMatrix[newX, newY].SquareObject.transform;
            element.transform.parent = sqParent;
            Vector3 newPosition = sqParent.position - Vector3.forward * ELEMENT_OFFSET;
            element.MoveDown(newPosition, elementSettings.ElementMoveDownTime);
        }


        private void ClearMovingFlag()
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    squareMatrix[i, j].Moving = false;
                }
            }
        }

        private void ClearMovingIndex()
        {
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < ySize; j++)
                {
                    if (squareMatrix[i, j].InsideElement != null)
                    {
                        squareMatrix[i, j].InsideElement.LastMoveIndex = 0;
                    }
                }
            }
        }
        #endregion

        void OnDestroy()
        {
            CommandKeeper.Instance.CreateNewElements -= CreateNewElements;
            CommandKeeper.Instance.AskSquareUnderFinger -= ReturnSquareByCoordinates;
            CommandKeeper.Instance.DeleteElement -= DeleteElement;
            CommandKeeper.Instance.GetSquareByIndex -= GetSquareByIndex;
        }

    }

}
