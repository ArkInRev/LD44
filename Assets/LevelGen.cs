using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public GridLayout level;
    public int cols = 4;
    public int rows = 10;
    public Room[,] lvlLayout;


    public void Awake()
    {
        lvlLayout = new Room[rows, cols];

    }

    public void Start()
    {
        CreateLevel(ref level);
    }

    public void CreateLevel(ref GridLayout lvl) {
        int row = 0; // current row in the loop
        int col = 0; // current col in the loop

        col = Random.Range(0, cols); // which column is the exit going to be placed in?
        int exitCol = Random.Range(0, cols); // which column is the exit going to be placed in?

        bool done = false;
        Direction lastDir = Direction.Up;
        RoomType lastRoomType = RoomType.Start;

        //place the starting room
        lvlLayout[row, col].lastRoomDir = lastDir;
        lvlLayout[row, col].thisRoom = lastRoomType;

        int choice = 0; //random choice in the path logic.

        Debug.Log("Start Column = " + col.ToString()+" End Column = "+exitCol);
      

        while (!done)
        {

            // If I am in the Leftmost Column
            if(col == 0)
            {
                // Am I in the final row? 
                if (row == rows - 1) 
                {
                    // Can only go right or end
                    // Am I in the end column
                    if(exitCol == col)
                    {
                        lvlLayout[row, col].nextRoomDir = Direction.None;
                        lvlLayout[row, col].thisRoom = RoomType.Exit;
                        done = true;
                    } else // I must go right to reach the exit 
                    {
                        // Did I drop into this room?
                        if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                        lastRoomType = RoomType.Standard;
                        lvlLayout[row, col].nextRoomDir = Direction.Right;
                        lvlLayout[row, col + 1].thisRoom = lastRoomType;
                        lvlLayout[row, col + 1].lastRoomDir = Direction.Left;
                        col += 1;
                        lastDir = Direction.Right;
                    }

                }
                else
                // I can go either right or down
                {

                    //Am I in the start room?
                    if(row ==0 && lastRoomType == RoomType.Start)
                    {
                        // I have to go Right since I can't go down in the Start Room
                        lvlLayout[row, col].nextRoomDir = Direction.Right;
                        lvlLayout[row, col + 1].thisRoom = RoomType.Standard;
                        lvlLayout[row, col + 1].lastRoomDir = Direction.Left;
                        col += 1;
                        lastDir = Direction.Right;
                        lastRoomType = RoomType.Standard;
                    } else 
                    {
                        // I can go right or down
                        choice = Random.Range(0, 2);
                        switch (choice)
                        {
                            case 0:
                                // Did I drop into this room?
                                if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                                lastRoomType = RoomType.Standard;
                                        lvlLayout[row, col].nextRoomDir = Direction.Right;
                                lvlLayout[row, col + 1].thisRoom = lastRoomType;
                                lvlLayout[row, col + 1].lastRoomDir = Direction.Left;
                                col += 1;
                                lastDir = Direction.Right;
                                break;
                            case 1:
                                // Did I drop into this room?
                                if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Climb;
                                lastRoomType = RoomType.Drop;
                                        lvlLayout[row, col].nextRoomDir = Direction.Down;
                                lvlLayout[row + 1, col].thisRoom = lastRoomType;
                                lvlLayout[row + 1, col].lastRoomDir = Direction.Left;
                                row += 1;
                                lastDir = Direction.Down;
                                break;
                        }
                    }
                }

            }
            // If I am in the Rightmost Column
            else if (col == cols-1)         
            {
                // Am I in the final row? 
                if (row == rows - 1)
                {
                    // Am I in the end column?
                    if (exitCol == col)
                    {
                        lvlLayout[row, col].nextRoomDir = Direction.None;
                        lvlLayout[row, col].thisRoom = RoomType.Exit;
                        done = true;
                    }
                    else // I must go left to reach the exit 
                    {
                        // Did I drop into this room?
                        if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                        lastRoomType = RoomType.Standard;
                        lvlLayout[row, col].nextRoomDir = Direction.Left;
                        lvlLayout[row, col - 1].thisRoom = lastRoomType;
                        lvlLayout[row, col - 1].lastRoomDir = Direction.Right;
                        col -= 1;
                        lastDir = Direction.Left;
                    }
                }
                else
                // I can go either left or down
                {
                    //Am I in the start room?
                    if (row == 0 && lastRoomType == RoomType.Start)
                    {
                        // I have to go Left since I can't go down in the Start Room
                        lvlLayout[row, col].nextRoomDir = Direction.Left;
                        lvlLayout[row, col - 1].thisRoom = RoomType.Standard;
                        lvlLayout[row, col - 1].lastRoomDir = Direction.Right;
                        col -= 1;
                        lastDir = Direction.Left;
                        lastRoomType = RoomType.Standard;
                    }
                    else
                    {
                        choice = Random.Range(0, 2);
                        switch (choice)
                        {
                            case 0: // chose to go Left
                                    // Did I drop into this room?
                                if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                                lastRoomType = RoomType.Standard;
                                        lvlLayout[row, col].nextRoomDir = Direction.Left;
                                lvlLayout[row, col - 1].thisRoom = RoomType.Standard;
                                lvlLayout[row, col - 1].lastRoomDir = Direction.Right;
                                col -= 1;
                                lastDir = Direction.Left;
                                break;
                            case 1:
                                // Did I drop into this room?
                                if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Climb;
                                lastRoomType = RoomType.Drop;
                                        lvlLayout[row, col].nextRoomDir = Direction.Down;
                                lvlLayout[row + 1, col].thisRoom = lastRoomType;
                                lvlLayout[row + 1, col].lastRoomDir = Direction.Left;
                                row += 1;
                                lastDir = Direction.Down;
                                break;
                        }
                    }
                }
            }
            // If I am in a Middle Column
            else
            {
                // Am I in the final row? 
                if (row == rows - 1)
                {
                    // Can only go left, right, or end
                    // Am I in the end column?
                    if (exitCol == col)
                    {
                        lvlLayout[row, col].nextRoomDir = Direction.None;
                        lvlLayout[row, col].thisRoom = RoomType.Exit;
                        done = true;
                    }
                    else if (exitCol<col) // I must go left to reach the exit 
                    {
                        // Did I drop into this room?
                        if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                        lastRoomType = RoomType.Standard;
                        lvlLayout[row, col].nextRoomDir = Direction.Left;
                        lvlLayout[row, col - 1].thisRoom = lastRoomType;
                        lvlLayout[row, col - 1].lastRoomDir = Direction.Right;
                        col -= 1;
                        lastDir = Direction.Left;
                    }
                    else //I must go right to reach the exit.
                    {
                        // Did I drop into this room?
                        if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                        lastRoomType = RoomType.Standard;
                        lvlLayout[row, col].nextRoomDir = Direction.Right;
                        lvlLayout[row, col + 1].thisRoom = lastRoomType;
                        lvlLayout[row, col + 1].lastRoomDir = Direction.Left;
                        col += 1;
                        lastDir = Direction.Right;

                    }

                }
                else
                // I can go either left, right, or down
                {
                    choice = Random.Range(0, 3);
                    switch (choice)
                    {
                        case 0: // chose to go Left
                                // Did I drop into this room?
                            if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                            lastRoomType = RoomType.Standard;
                                lvlLayout[row, col].nextRoomDir = Direction.Left;
                            lvlLayout[row, col - 1].thisRoom = lastRoomType;
                            lvlLayout[row, col - 1].lastRoomDir = Direction.Right;
                            col -= 1;
                            lastDir = Direction.Left;
                            break;
                        case 1: //I chose to drop
                                // Did I drop into this room?
                            if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Climb;
                            lastRoomType = RoomType.Drop;
                                lvlLayout[row, col].nextRoomDir = Direction.Down;
                            lvlLayout[row + 1, col].thisRoom = lastRoomType;
                            lvlLayout[row + 1, col].lastRoomDir = Direction.Left;
                            row += 1;
                            lastDir = Direction.Down;
                            break;
                        case 2: // I chose to go Right 
                                // Did I drop into this room?
                            if (lastDir == Direction.Down) lvlLayout[row, col].thisRoom = RoomType.Drop;
                            lastRoomType = RoomType.Standard;
                                lvlLayout[row, col].nextRoomDir = Direction.Right;
                            lvlLayout[row, col + 1].thisRoom = lastRoomType;
                            lvlLayout[row, col + 1].lastRoomDir = Direction.Left;
                            col += 1;
                            lastDir = Direction.Right;
                            break;
                    }
                }
            }

            if (row >= rows) done = true;



        }

        Debug.Log(lvlLayout.ToString());
        string logLine = "";
        for (int i = 0; i < rows; i++)
        {
            logLine = "Row "+i.ToString()+": ";
            for(int j = 0; j < cols; j++)
            {
                logLine += lvlLayout[i, j].thisRoom.ToString() + " : ";
            }
            Debug.Log(logLine);
        }
    }

    public struct Room
    {
        public Direction lastRoomDir;
        public Direction nextRoomDir;
        public RoomType thisRoom;

    }

    public enum Direction
    {
        Left = 0,
        Up = 1,
        Right = 2,
        Down = 3,
        None = 4
    }

    public enum RoomType
    {
        Filler = 0,
        Start = 1,
        Standard = 2,
        Climb = 3,
        Drop = 4,
        Exit = 5

    }
}
