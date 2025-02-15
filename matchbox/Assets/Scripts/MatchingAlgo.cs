using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchingAlgo
{
    public struct Index
    {
        public int X;
        public int Y;

        public static bool operator ==(Index obj1, Index obj2)
        {
            return (obj1.Y == obj2.Y) && (obj1.X == obj2.X);
        }

        public static bool operator !=(Index obj1, Index obj2)
        {
            return !(obj1 == obj2);
        }
    }
    
    
    public static bool CheckMatch(Index startingIndex, IMatchInterface[,] board, ref List<Index> indexes, ref List<IMatchInterface> elements)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        indexes.Clear();
        elements.Clear();

        // make sure starting conditions are valid
        if (CheckValidIndex(startingIndex, ref board))
        {
            // get out starting information
            IMatchInterface startingBox = board[startingIndex.X, startingIndex.Y];
            string Color = startingBox.Color;
            

            // we only need to look for matches across each of the two cases X and Y rows
            // I am extremely tempted to write a for loop to do this bc I'm a psychopath, but let's do this normally once

            for (int j = 0; j < 2; j++)
            {
                List<Index> SetFound = new List<Index>();
                SetFound.Add(startingIndex);

                for (int i = 0; i < 2; i++)
                {
                    int Direction = 1 - (2 * i);
                    List<Index> ToCheck = new List<Index>();
                    ToCheck.Add(startingIndex);


                    while (ToCheck.Count > 0)
                    {
                        Index NextIndex = ToCheck[0];
                        if (j % 2 == 0)
                        {
                            NextIndex.X += Direction;
                        }
                        else
                        {
                            NextIndex.Y += Direction;
                        }
                        
                        ToCheck.RemoveAt(0);

                        if (CheckValidIndex(NextIndex, ref board))
                        {
                            if (board[NextIndex.X, NextIndex.Y].Color == Color)
                            {
                                ToCheck.Add(NextIndex);
                                SetFound.Add(NextIndex);
                            }
                        }
                    }
                }

                if (SetFound.Count >= 3)
                {
                    indexes.AddRange(SetFound);
                }
            }
        }

        if (indexes.Count >= 3)
        {
            foreach(Index boxIndex in indexes)
            {
                elements.Add(board[boxIndex.X,boxIndex.Y]);
            }
            return true;
        }
        
        return false;
    }

    static public bool CheckValidIndex(Index index, ref IMatchInterface[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        return rows > 0 && cols > 0 && index.X < rows && index.Y < cols && index is { Y: >= 0, X: >= 0 };
    }
}
