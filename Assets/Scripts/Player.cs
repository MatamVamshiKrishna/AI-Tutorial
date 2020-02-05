using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject gridGO = null;
    private int currentIndex = 0;
    private int rows = 0;
    private int columns = 0;
    private int destinationIndex = 0;

    public void StartGame(GameObject gridGO, int startIndex, int destinationIndex, int rows, int columns)
    {
        this.gridGO = gridGO;
        currentIndex = startIndex;
        this.destinationIndex = destinationIndex;
        this.rows = rows;
        this.columns = columns;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            InvokeRepeating("Move", 0.0f, 1.0f);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            FillColumnsToRight(1, destinationIndex);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            FillColumnsToLeft(1, destinationIndex);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            FillColumnsToUp(1, destinationIndex);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            FillColumnsToDown(1, destinationIndex);

        if (Input.GetKeyDown(KeyCode.O))
        {
            var gridT = gridGO.transform;
            for (int i = 0; i < gridT.childCount; ++i)
            {
                if (i == currentIndex || i == destinationIndex)
                    continue;

                if (gridT.GetChild(i).GetComponent<Tile>().Weight == 1)
                {
                    int minWeight = rows * columns; // max number
                    if (IsValidRight(i) && IsTileWalkable(i + 1))
                    {
                        if (gridT.GetChild(i + 1).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i + 1).GetComponent<Tile>().Weight);
                    }
                    if (IsValidLeft(i) && IsTileWalkable(i - 1))
                    {
                        if (gridT.GetChild(i - 1).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i - 1).GetComponent<Tile>().Weight);
                    }
                    if (IsValidUp(i) && IsTileWalkable(i + columns))
                    {
                        if (gridT.GetChild(i + columns).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i + columns).GetComponent<Tile>().Weight);
                    }
                    if (IsValidDown(i) && IsTileWalkable(i - columns))
                    {
                        if (gridT.GetChild(i - columns).GetComponent<Tile>().Weight != 1)
                            minWeight = Mathf.Min(minWeight, gridT.GetChild(i - columns).GetComponent<Tile>().Weight);
                    }

                    if (minWeight != rows * columns)
                    {
                        gridT.GetChild(i).GetComponent<Tile>().SetWeight(minWeight);
                    }
                }
            }
        }
    }

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

 





    

    private bool IsTileWalkable(int index)
    {
        return gridGO.transform.GetChild(index).GetComponent<Tile>().IsWalkable();
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

