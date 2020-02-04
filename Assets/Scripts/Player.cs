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
    private Direction lastDirection = Direction.NONE;

    public void StartGame(GameObject gridGO, int startIndex, int destinationIndex, int rows, int columns)
    {
        this.gridGO = gridGO;
        currentIndex = startIndex;
        this.destinationIndex = destinationIndex;
        this.rows = rows;
        this.columns = columns;

        //gridGO.transform.GetChild(destinationIndex).GetComponent<Tile>().Weight = 0;

        //SetWeightAndDirection(destinationIndex);
        FillColumnsToRight(1, destinationIndex);
        FillColumnsToLeft(1, destinationIndex);
        FillColumnsToUp(1, destinationIndex);
        FillColumnsToDown(1, destinationIndex);

        var gridT = gridGO.transform;
        for(int k=0;k<rows*columns;++k)
        {
            for(int i=0;i<gridT.childCount;++i)
            {
                if (i == startIndex || i == destinationIndex)
                    continue;
                

                if(gridT.GetChild(i).GetComponent<Tile>().Weight == 1)
                {
                    int minWeight = rows * columns; // max number
                    if (IsValidRight(i) && IsTileWalkable(i+1))
                    {
                        if(gridT.GetChild(i + 1).GetComponent<Tile>().Weight != 1)
                        minWeight = Mathf.Min(minWeight, gridT.GetChild(i + 1).GetComponent<Tile>().Weight);
                    }
                    if (IsValidLeft(i) && IsTileWalkable(i-1))
                    {
                        if(gridT.GetChild(i - 1).GetComponent<Tile>().Weight != 1)
                        minWeight = Mathf.Min(minWeight, gridT.GetChild(i - 1).GetComponent<Tile>().Weight);
                    }
                    if (IsValidUp(i) && IsTileWalkable(i+columns))
                    {
                        if(gridT.GetChild(i + columns).GetComponent<Tile>().Weight != 1)
                        minWeight = Mathf.Min(minWeight, gridT.GetChild(i + columns).GetComponent<Tile>().Weight);
                    }
                    if (IsValidDown(i) && IsTileWalkable(i-columns))
                    {
                        if(gridT.GetChild(i - columns).GetComponent<Tile>().Weight != 1)
                        minWeight = Mathf.Min(minWeight, gridT.GetChild(i - columns).GetComponent<Tile>().Weight);
                    }

                    if (minWeight != rows * columns)
                    {
                        gridT.GetChild(i).GetComponent<Tile>().SetWeight(minWeight);
                    }

                }
            }
        }


        /*var currentRow = currentIndex / columns;
        var destinationRow = destinationIndex / columns;
        if (currentRow < destinationRow)
            direction = Direction.UP;
        else if (currentRow > destinationRow)
            direction = Direction.DOWN;
        else
        {
            if (currentIndex < destinationIndex)
                direction = Direction.RIGHT;
            else
                direction = Direction.LEFT;
        }*/

        InvokeRepeating("Move", 0.0f, 1.0f);
    }

    /*private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Called");
            var gridT = gridGO.transform;
            for (int i = 0; i < gridT.childCount; ++i)
            {
                if (i == currentIndex || i == destinationIndex)
                    continue;


                if (gridT.GetChild(i).GetComponent<Tile>().Weight == 1)
                {
                    int minWeight = rows * columns; // max number
                    Debug.Log("index start");
                    Debug.Log("index" + i + "weight" + minWeight);
                    if (IsValidRight(i) && IsTileWalkable(i + 1))
                    {
                        Debug.Log(gridT.GetChild(i + 1).GetComponent<Tile>().Weight);
                        if (gridT.GetChild(i + 1).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i + 1).GetComponent<Tile>().Weight);
                        Debug.Log("right index" + i + "weight" + minWeight);
                    }
                    if (IsValidLeft(i) && IsTileWalkable(i - 1))
                    {
                        if (gridT.GetChild(i - 1).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i - 1).GetComponent<Tile>().Weight);
                        Debug.Log("left index" + i + "weight" + minWeight);
                    }
                    if (IsValidUp(i) && IsTileWalkable(i + columns))
                    {
                        if (gridT.GetChild(i + columns).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i + columns).GetComponent<Tile>().Weight);
                        Debug.Log("up index" + i + "weight" + minWeight);
                    }
                    if (IsValidDown(i) && IsTileWalkable(i - columns))
                    {
                        if (gridT.GetChild(i - columns).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i - columns).GetComponent<Tile>().Weight);
                        Debug.Log("down index" + i + "weight" + minWeight);
                    }

                    Debug.Log("index" + i+ "weight" + minWeight);

                    if (minWeight != rows * columns)
                    {
                        gridT.GetChild(i).GetComponent<Tile>().SetWeight(minWeight);
                        //bAllFilled = bAllFilled && true;
                    }
                    else
                    {
                        //bAllFilled = bAllFilled && false;
                    }

                }
                else
                {
                    //bAllFilled = bAllFilled && true;
                }
            }
        }
    }*/

    private void Move()
    {
        if (currentIndex == destinationIndex)
        {
            return;
        }

        var gridT = gridGO.transform;
        var minWeight = rows * columns;
        var weight = 0;
        var nextIndex = -1;
        if(IsValidRight(currentIndex))
        {
            weight = gridT.GetChild(currentIndex + 1).GetComponent<Tile>().Weight;
            if(weight < minWeight && (weight != 1 || currentIndex+1 == destinationIndex))
            {
                minWeight = weight;
                nextIndex = currentIndex + 1;
            }
        }

        if (IsValidLeft(currentIndex))
        {
            weight = gridT.GetChild(currentIndex - 1).GetComponent<Tile>().Weight;
            if (weight < minWeight && (weight != 1 || currentIndex - 1 == destinationIndex))
            {
                minWeight = weight;
                nextIndex = currentIndex - 1;
            }
        }

        if (IsValidUp(currentIndex))
        {
            weight = gridT.GetChild(currentIndex + columns).GetComponent<Tile>().Weight;
            if (weight < minWeight && (weight != 1 || currentIndex + columns == destinationIndex))
            {
                minWeight = weight;
                nextIndex = currentIndex + columns;
            }
        }

        if (IsValidDown(currentIndex))
        {
            weight = gridT.GetChild(currentIndex - columns).GetComponent<Tile>().Weight;
            if (weight < minWeight && (weight != 1 || currentIndex - columns == destinationIndex))
            {
                minWeight = weight;
                nextIndex = currentIndex - columns;
            }
        }

        if (nextIndex != -1)
            currentIndex = nextIndex;

        transform.position = gridGO.transform.GetChild(currentIndex).position;
    }

    private int GetNextIndex(List<Direction> attempted)
    {
        var currentRow = currentIndex / columns;
        var destinationRow = destinationIndex / columns;
        var nextIndex = -1;

        if(attempted.Count == 0)
        {
            lastDirection = direction;
            if (currentRow < destinationRow && direction != Direction.DOWN)
                direction = Direction.UP;
            else if (currentRow > destinationRow && direction != Direction.UP)
                direction = Direction.DOWN;
            else
            {
                var equivalentIndex = currentIndex + (columns * (destinationRow - currentRow));
                if (equivalentIndex < destinationIndex)
                    direction = Direction.RIGHT;
                else
                    direction = Direction.LEFT;
            }
        }

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
                if (!attempted.Contains(Direction.DOWN) && lastDirection != Direction.DOWN)
                {
                    direction = Direction.DOWN;
                    nextIndex = GetNextIndex(attempted);
                    return nextIndex;
                }
                else
                {
                    if (equivalentIndex <= destinationIndex)
                    {
                        if (!attempted.Contains(Direction.RIGHT))
                        {
                            direction = Direction.RIGHT;
                            nextIndex = GetNextIndex(attempted);
                            return nextIndex;
                        }
                    }

                    if (equivalentIndex >= destinationIndex)
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
                if (!attempted.Contains(Direction.UP) && lastDirection != Direction.UP)
                {
                    direction = Direction.UP;
                    nextIndex = GetNextIndex(attempted);
                    return nextIndex;
                }
                else
                {
                    if (equivalentIndex <= destinationIndex)
                    {
                        if (!attempted.Contains(Direction.RIGHT))
                        {
                            direction = Direction.RIGHT;
                            nextIndex = GetNextIndex(attempted);
                            return nextIndex;
                        }
                    }

                    if (equivalentIndex >= destinationIndex)
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

    void SetWeightAndDirection(int index)
    {
        var column = index % columns + 1;
        var columnsToRight = columns - column;
        var columnsToLeft = column - 1;
        var row = index / columns + 1;
        var rowsToUp = rows - row;
        var rowsToDown = row - 1;

        var gridT = gridGO.transform;
        
        for(var i=0;i< columnsToRight;++i)
        {
            var currentWeight = gridT.GetChild(index + i).GetComponent<Tile>().Weight;
            var nextIndex = index + i + 1;
            gridT.GetChild(nextIndex).GetComponent<Tile>().SetWeight(currentWeight);

            for(var k=0;k<rowsToUp;k++)
            {
                currentWeight = gridT.GetChild(nextIndex + k * columns).GetComponent<Tile>().Weight;
                gridT.GetChild(nextIndex + (k + 1) * columns).GetComponent<Tile>().SetWeight(currentWeight);
            }

            for(var k=0;k<rowsToDown;k++)
            {
                currentWeight = gridT.GetChild(nextIndex - k * columns).GetComponent<Tile>().Weight;
                gridT.GetChild(nextIndex - (k + 1) * columns).GetComponent<Tile>().SetWeight(currentWeight);
            }
        }
        
        for (var i=0;i<columnsToLeft;++i)
        {
            var currentWeight = gridT.GetChild(index - i).GetComponent<Tile>().Weight;
            var nextIndex = index - i - 1;
            gridT.GetChild(nextIndex).GetComponent<Tile>().SetWeight(currentWeight);

            for (var k = 0; k < rowsToUp; k++)
            {
                currentWeight = gridT.GetChild(nextIndex + k * columns).GetComponent<Tile>().Weight;
                gridT.GetChild(nextIndex + (k + 1) * columns).GetComponent<Tile>().SetWeight(currentWeight);
            }

            for (var k = 0; k < rowsToDown; k++)
            {
                currentWeight = gridT.GetChild(nextIndex - k * columns).GetComponent<Tile>().Weight;
                gridT.GetChild(nextIndex - (k + 1) * columns).GetComponent<Tile>().SetWeight(currentWeight);
            }
        }

        for (var i = 0; i < rowsToUp; ++i)
        {
            var currentWeight = gridT.GetChild(index + i*columns).GetComponent<Tile>().Weight;
            gridT.GetChild(index + (i + 1)*columns).GetComponent<Tile>().SetWeight(currentWeight);
        }

        for (var i = 0; i < rowsToDown; ++i)
        {
            var currentWeight = gridT.GetChild(index - i * columns).GetComponent<Tile>().Weight;
            gridT.GetChild(index - (i + 1) * columns).GetComponent<Tile>().SetWeight(currentWeight);
        }

        /*if(IsValidLeft(index))
        {
            //gridT.GetChild(index - 1).GetComponent<Tile>().SetWeight(currentWeight);
            //SetWeightAndDirection(index - 1);
        }

        if (IsValidUp(index))
        {
            //gridT.GetChild(index + columns).GetComponent<Tile>().SetWeight(currentWeight);
            //SetWeightAndDirection(index + columns);
        }

        if(IsValidDown(index))
        {
            //gridT.GetChild(index - columns).GetComponent<Tile>().SetWeight(currentWeight);
            //SetWeightAndDirection(index - columns);
        }*/

    }

    private bool IsValidRight(int index)
    {
        var temp = index + 1;
        return (temp / columns == index / columns) && IsValidIndex(temp);
    }

    private bool IsValidLeft(int index)
    {
        var temp = index - 1;
        return (temp / columns == index / columns) && IsValidIndex(temp) ;
    }

    private bool IsValidUp(int index)
    {
        return IsValidIndex(index + columns);
    }

    private bool IsValidDown(int index)
    {
        return IsValidIndex(index - columns);
    }

    private bool IsValidIndex(int index)
    {
        if (index < 0 || index >= rows * columns)
            return false;

        return true;
    }

    private void FillColumnsToRight(int lastTileWeight, int lastTileIndex)
    {
        var nextTileIndex = lastTileIndex + 1;
        if((nextTileIndex / columns == lastTileIndex / columns) && IsValidIndex(nextTileIndex))
        {
            if(IsTileWalkable(nextTileIndex))
            {
                var nextTile = gridGO.transform.GetChild(nextTileIndex).GetComponent<Tile>();
                nextTile.SetWeight(lastTileWeight);
                lastTileWeight = nextTile.Weight;

                FillColumnsToUp(lastTileWeight, nextTileIndex);
                FillColumnsToDown(lastTileWeight, nextTileIndex);
                FillColumnsToRight(lastTileWeight, nextTileIndex);
            }
           
        }
    }

    private void FillColumnsToLeft(int lastTileWeight, int lastTileIndex)
    {
        var nextTileIndex = lastTileIndex - 1;
        if ((nextTileIndex / columns == lastTileIndex / columns) && IsValidIndex(nextTileIndex))
        {
            if (IsTileWalkable(nextTileIndex))
            {
                var nextTile = gridGO.transform.GetChild(nextTileIndex).GetComponent<Tile>();
                nextTile.SetWeight(lastTileWeight);
                lastTileWeight = nextTile.Weight;

                FillColumnsToUp(lastTileWeight, nextTileIndex);
                FillColumnsToDown(lastTileWeight, nextTileIndex);
                FillColumnsToLeft(lastTileWeight, nextTileIndex);
            }

        }
        
    }

    private void FillColumnsToUp(int lastTileWeight, int lastTileIndex)
    {
        var nextTileIndex = lastTileIndex + columns;
        if (IsValidIndex(nextTileIndex))
        {
            if (IsTileWalkable(nextTileIndex))
            {
                var nextTile = gridGO.transform.GetChild(nextTileIndex).GetComponent<Tile>();
                nextTile.SetWeight(lastTileWeight);
                FillColumnsToUp(nextTile.Weight, nextTileIndex);
            }
        }
    }

    private void FillColumnsToDown(int lastTileWeight, int lastTileIndex)
    {
        var nextTileIndex = lastTileIndex - columns;
        if (IsValidIndex(nextTileIndex))
        {
            if (IsTileWalkable(nextTileIndex))
            {
                var nextTile = gridGO.transform.GetChild(nextTileIndex).GetComponent<Tile>();
                nextTile.SetWeight(lastTileWeight);
                FillColumnsToDown(nextTile.Weight, nextTileIndex);
            }
        }
    }
}

