using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Settings;
using System;
namespace Game.GameCore
{
    public class BoardFabric
    {
        private const int MAX_X_SIZE = 10;
        private const int MAX_Y_SIZE = 10;

        private Square[,] squareMatrix;
        private float squareSize;
        private Vector3 startPosition;

        private LevelBoard boardSettings;
        private ElementsSettings elementsSettings;
        private Transform back;

        private Board board;

        public Board CreateBoard(LevelSettings levelSettings, ElementsSettings elementsSettings, GameObject backObject, GameObject boardObject)
        {
            this.boardSettings = levelSettings.Board;
            this.elementsSettings = elementsSettings;
            back = backObject.transform;
            this.board = boardObject.AddComponent<Board>();

            float borderYSize = Camera.main.orthographicSize * 2;
            float borderXSize = borderYSize * (float)Screen.width / (float)Screen.height;
            squareSize = borderXSize / (Mathf.Max(boardSettings.XSize, boardSettings.YSize) + 1);         

            if ((boardSettings.XSize > MAX_X_SIZE) || (boardSettings.XSize < 2))
            {
                Debug.LogWarning(String.Format("Board XSize out off Range(2, {0})", MAX_X_SIZE.ToString()));
                return null;
            }
            if ((boardSettings.YSize > MAX_Y_SIZE) || (boardSettings.YSize < 2))
            {
                Debug.LogWarning(String.Format("Board YSize out off Range(2, {0})", MAX_X_SIZE.ToString()));
                return null;
            }
            startPosition = back.transform.position + new Vector3(-(((float)boardSettings.XSize - 1) / 2) * squareSize,
                                                                   (((float)boardSettings.YSize - 1) / 2) * squareSize,
                                                                  -0.01f);
           

            SetSquareMatrix();
            CreateSquares();

            board.Init(squareMatrix, elementsSettings, boardSettings.XSize, boardSettings.YSize, levelSettings.MaxElementTypes);

            //Calculate Screen Positions for border
            Vector3 leftUp = back.transform.position + new Vector3(-(((float)boardSettings.XSize) / 2) * squareSize,
                                                                   (((float)boardSettings.YSize) / 2) * squareSize,
                                                                  -0.01f);
            Vector2 leftUpScreen = Camera.main.WorldToScreenPoint(leftUp);


            Vector3 rightUp = back.transform.position + new Vector3((((float)boardSettings.XSize) / 2) * squareSize,
                                                                   (-((float)boardSettings.YSize) / 2) * squareSize,
                                                                  -0.01f);
            Vector2 rightUpScreen = Camera.main.WorldToScreenPoint(rightUp);

            float screenSquareSize = (rightUpScreen.x - leftUpScreen.x) / boardSettings.XSize;

            board.SetupScreenSize(screenSquareSize, leftUpScreen, rightUpScreen);

            return board;
        }



        private void SetSquareMatrix()
        {
            squareMatrix = new Square[boardSettings.XSize, boardSettings.YSize];

            for (int i = 0; i < boardSettings.XSize; i++)
            {
                for (int j = 0; j < boardSettings.YSize; j++)
                {
                    squareMatrix[i, j] = new Square(i, j);
                    squareMatrix[i, j].Type = SquareType.Container;

                }
            }

            List<BoardSquareInBoard> bsSettings = boardSettings.boardSquareSettings;
            List<Square> emptySquares = new List<Square>();
            if (bsSettings != null)
            {
                for (int i = 0; i < bsSettings.Count; i++)
                {
                    int bsX = bsSettings[i].X;
                    int bsY = bsSettings[i].Y;
                    if ((bsX >= 0) && (bsX < boardSettings.XSize) && (bsY >= 0) && (bsY < boardSettings.YSize))
                    {

                        squareMatrix[bsX, bsY].Type = bsSettings[i].Type;
                        if (bsSettings[i].Type == SquareType.NullSquare)
                        {
                            emptySquares.Add(squareMatrix[bsX, bsY]);
                        }
                    }
                }
            }
            SetFlagForNullSquares(emptySquares);
        }

        private void SetFlagForNullSquares(List<Square> emptySquares)
        {
            //Border Null Squares
            for (int i = emptySquares.Count - 1; i >= 0; i--)
            {
                bool drawNullSquare = (emptySquares[i].X == 0) ||
                                      (emptySquares[i].X == boardSettings.XSize - 1) ||
                                      (emptySquares[i].Y == 0) ||
                                      (emptySquares[i].Y == boardSettings.YSize - 1);
                if (drawNullSquare)
                {
                    emptySquares[i].OutsideNull = true;
                    emptySquares.RemoveAt(i);
                }
            }

            //Inside Null Squares
            while (emptySquares.Count > 0)
            {
                for (int j = emptySquares.Count - 1; j >= 0; j--)
                {
                    int x = emptySquares[j].X;
                    int y = emptySquares[j].Y;

                    bool outsideNeighbourds = squareMatrix[x - 1, y].OutsideNull == true ||
                                                   squareMatrix[x + 1, y].OutsideNull == true ||
                                                   squareMatrix[x, y - 1].OutsideNull == true ||
                                                   squareMatrix[x, y + 1].OutsideNull == true;
                    if (outsideNeighbourds)
                    {
                        emptySquares[j].OutsideNull = true;
                        emptySquares.RemoveAt(j);
                        break;
                    }

                    if (j == 0)
                    {
                        emptySquares.Clear();
                    }
                }
            }
        }


        private void CreateSquares()
        {

            for (int i = 0; i < boardSettings.XSize; i++)
            {
                for (int j = 0; j < boardSettings.YSize; j++)
                {
                    squareMatrix[i, j].SquareObject =
                                 CreateSquare(startPosition - j * squareSize * Vector3.up + i * squareSize * Vector3.right,
                                              squareSize,
                                              Mathf.Max(boardSettings.YSize, boardSettings.XSize) * j + i,
                                              elementsSettings.GetBoardSquare(SquareType.Container).Square);
                }
            }
        }

        private GameObject CreateSquare(Vector3 position, float size, int id, GameObject square)
        {
            GameObject sqObj = (GameObject)MonoBehaviour.Instantiate(square, board.transform);
            sqObj.transform.position = position;
            sqObj.transform.localScale = size * Vector3.one;
            sqObj.name = id.ToString();
            return sqObj;
        }

    }


}
