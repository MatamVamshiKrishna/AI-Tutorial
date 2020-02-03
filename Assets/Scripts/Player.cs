using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    NONE
}

public class Player : MonoBehaviour
{
    private GameObject gridGO = null;
    private int currentIndex = 0;
    private int rows = 0;
    private int columns = 0;
    private int destinationIndex = 0;
    private Direction direction = Direction.NONE;
   
    public void StartGame(GameObject gridGO, int startIndex, int destinationIndex, int rows, int columns)
    {
        this.gridGO = gridGO;
        currentIndex = startIndex;
        this.destinationIndex = destinationIndex;
        this.rows = rows;
        this.columns = columns;
        /*if (currentIndex < destinationIndex)
            direction = Direction.UP;
        else*/
            direction = Direction.DOWN;

        InvokeRepeating("Move", 0.0f, 1.0f);
    }

    private void Move()
    {
        List<Direction> attempted = new List<Direction>();
        currentIndex = GetNextIndex(attempted);
        transform.position = gridGO.transform.GetChild(currentIndex).position;
    }

    private int GetNextIndex(List<Direction> attempted)
    {
        var currentRow = currentIndex / columns;
        var destinationRow = destinationIndex / columns;
        var nextIndex = -1;
        attempted.Add(direction);

        if (currentRow > destinationRow)
        {
            var tempIndex = -1;
            switch (direction)
            {
                case Direction.UP:
                    tempIndex = currentIndex + columns;
                    if (isTileWalkable(tempIndex))
                    {
                        nextIndex = tempIndex;
                    }
                    else
                    {
                        if (tempIndex < destinationIndex)
                            direction = Direction.RIGHT;
                        else
                            direction = Direction.LEFT;
                    }
                    break;
                case Direction.DOWN:
                    tempIndex = currentIndex - columns;
                    if(isTileWalkable(tempIndex))
                    {
                        nextIndex = tempIndex;
                    }
                    else
                    {
                        if (tempIndex < destinationIndex)
                        {
                            direction = Direction.RIGHT;
                        }
                        else
                        {
                            direction = Direction.LEFT;
                        }
                    }
                    break;
                case Direction.RIGHT:
                    tempIndex = currentIndex + 1;
                    if (isTileWalkable(tempIndex))
                    {
                        nextIndex = tempIndex;
                    }
                    else
                    {
                       direction = Direction.LEFT;
                    }
                    break;
                case Direction.LEFT:
                    nextIndex = currentIndex - 1;
                    if (isTileWalkable(tempIndex))
                    {
                        nextIndex = tempIndex;
                    }
                    else
                    {
                        direction = Direction.RIGHT;
                    }
                    break;
            }
        }
        else if(currentRow < destinationRow)
        {

        }
        else
        {

        }

        if (nextIndex < 0)
            nextIndex = GetNextIndex(attempted);

        return nextIndex;
    }

   
   

    
   
    /*private bool IsDestinationReached()
    {
        if (currentRow == destinationRow && currentColumn == destinationColumn)
        {
            return true;
        }

        return false;
    }*/

    private bool isTileWalkable(int index)
    {
        return gridGO.transform.GetChild(index).GetComponent<Tile>().IsWalkable();
    }
}

