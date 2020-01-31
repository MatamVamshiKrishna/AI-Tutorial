using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject destinationGO = null;
    private GameObject gridGO = null;
    private int index = 0;
    private int destinationRow = 0;
    private int destinationColumn = 0;
    private int currentRow = 0;
    private int currentColumn = 0;
   
    public void StartGame(GameObject destinationGO, GameObject gridGO, int rows, int columns)
    {
        this.destinationGO = destinationGO;
        this.gridGO = gridGO;
        destinationRow = rows;
        destinationColumn = columns;
        currentRow = 1;
        currentColumn = 1;
        InvokeRepeating("Move", 0.0f, 1.0f);
    }

    private void Move()
    {
        var currentIndex = ((currentRow - 1) * destinationColumn) + (currentColumn - 1);
        if(MoveUp(currentIndex))
        {

        }
        else if(MoveRight(currentIndex))
        {

        }
        else if(MoveDown(currentIndex))
        {

        }
        else if(MoveLeft(currentIndex))
        {

        }
        

        if(IsDestinationReached())
        {
            Debug.Log("Reached goal");
        }
    }

    private bool MoveRight(int currentIndex)
    {
        if (currentIndex % destinationColumn != destinationColumn -1 )
        {
            var nextIndex = currentIndex + 1;
            var tile = gridGO.transform.GetChild(nextIndex).GetComponent<Tile>();
            if (tile.IsWalkable())
            {
                transform.position = tile.transform.position;
                currentColumn++;
                return true;
            }
        }

        return false;
    }

    private bool MoveLeft(int currentIndex)
    {
        if (currentIndex % destinationColumn != 0)
        {
            var nextIndex = currentIndex - 1;
            var tile = gridGO.transform.GetChild(nextIndex).GetComponent<Tile>();
            if (tile.IsWalkable())
            {
                transform.position = tile.transform.position;
                currentColumn--;
                return true;
            }
        }
        
        return false;
    }

    private bool MoveUp(int currentIndex)
    {
        var nextIndex = currentIndex + destinationColumn;
        if (nextIndex < destinationColumn * destinationRow)
        {
            var tile = gridGO.transform.GetChild(nextIndex).GetComponent<Tile>();
            if (tile.IsWalkable())
            {
                transform.position = tile.transform.position;
                currentRow++;
                return true;
            }
        }

        return false;
    }

    private bool MoveDown(int currentIndex)
    {
        var nextIndex = currentIndex - destinationColumn;
        if(nextIndex < destinationColumn * destinationRow)
        {
            var tile = gridGO.transform.GetChild(nextIndex).GetComponent<Tile>();
            if (tile.IsWalkable())
            {
                transform.position = tile.transform.position;
                currentRow--;
                return true;
            }
        }
        return false;
    }

    private bool IsDestinationReached()
    {
        if (currentRow == destinationRow && currentColumn == destinationColumn)
        {
            return true;
        }

        return false;
    }
}

