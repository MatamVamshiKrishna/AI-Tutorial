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

        switch (direction)
        {
            case Direction.UP:
                nextIndex = currentIndex + columns;
                break;
            case Direction.DOWN:
                nextIndex = currentIndex - columns;
                break;
            case Direction.LEFT:
                nextIndex = currentIndex - 1;
                break;
            case Direction.RIGHT:
                nextIndex = currentIndex + 1;
                break;
        }


        if (IsValidNextIndex(nextIndex))
        {
            return nextIndex;
        }
        else
        {
            attempted.Add(direction);
            var equivalentIndex = currentIndex + (columns * (destinationRow - currentRow));

            if (currentRow > destinationRow)
            {
                if (!attempted.Contains(Direction.DOWN))
                {
                    direction = Direction.DOWN;
                    nextIndex = GetNextIndex(attempted);
                    return nextIndex;
                }
                else
                {
                    if (equivalentIndex < destinationIndex)
                    {
                        if (!attempted.Contains(Direction.RIGHT))
                        {
                            direction = Direction.RIGHT;
                            nextIndex = GetNextIndex(attempted);
                            return nextIndex;
                        }
                    }

                    if (equivalentIndex > destinationIndex)
                    {
                        if (!attempted.Contains(Direction.LEFT))
                        {
                            direction = Direction.LEFT;
                            nextIndex = GetNextIndex(attempted);
                            return nextIndex;
                        }
                    }

                    if (!attempted.Contains(Direction.UP))
                    {
                        direction = Direction.UP;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }
                }
            }
            else if (currentRow < destinationRow)
            {
                if (!attempted.Contains(Direction.UP))
                {
                    direction = Direction.UP;
                    nextIndex = GetNextIndex(attempted);
                    return nextIndex;
                }
                else
                {
                    if (equivalentIndex < destinationIndex)
                    {
                        if (!attempted.Contains(Direction.RIGHT))
                        {
                            direction = Direction.RIGHT;
                            nextIndex = GetNextIndex(attempted);
                            return nextIndex;
                        }
                    }

                    if (equivalentIndex > destinationIndex)
                    {
                        if (!attempted.Contains(Direction.LEFT))
                        {
                            direction = Direction.LEFT;
                            nextIndex = GetNextIndex(attempted);
                            return nextIndex;
                        }
                    }

                    if (!attempted.Contains(Direction.DOWN))
                    {
                        direction = Direction.DOWN;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }
                }
            }
            else
            {
                if (equivalentIndex < destinationIndex)
                {
                    if (!attempted.Contains(Direction.RIGHT))
                    {
                        direction = Direction.RIGHT;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }

                    if (!attempted.Contains(Direction.UP))
                    {
                        direction = Direction.UP;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }

                    if (!attempted.Contains(Direction.DOWN))
                    {
                        direction = Direction.DOWN;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }
                }

                if (equivalentIndex > destinationIndex)
                {
                    if (!attempted.Contains(Direction.LEFT))
                    {
                        direction = Direction.LEFT;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }

                    if (!attempted.Contains(Direction.UP))
                    {
                        direction = Direction.UP;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }

                    if (!attempted.Contains(Direction.DOWN))
                    {
                        direction = Direction.DOWN;
                        nextIndex = GetNextIndex(attempted);
                        return nextIndex;
                    }
                }

            }

            return nextIndex;
        }
    }





    /*private bool IsDestinationReached()
    {
        if (currentRow == destinationRow && currentColumn == destinationColumn)
        {
            return true;
        }

        return false;
    }*/

    private bool IsTileWalkable(int index)
    {
        return gridGO.transform.GetChild(index).GetComponent<Tile>().IsWalkable();
    }

    private bool IsValidNextIndex(int index)
    {
        if (index < 0 || index >= rows * columns)
            return false;

        if (direction == Direction.LEFT || direction == Direction.RIGHT)
        {
            if (index / columns != currentIndex / columns)
                return false;
        }

        return IsTileWalkable(index);
    }

}

