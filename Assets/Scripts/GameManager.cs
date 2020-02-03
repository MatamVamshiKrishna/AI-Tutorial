using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Rows;
    public int Columns;
    public GameObject TilePrefab;
    public GameObject PlayerPrefab;
    public GameObject DestinationPrefab;

    private bool isGameStarted = false;
    private GameObject gridGO = null;
    private GameObject playerGO = null;
    private GameObject destinationGO = null;
    private string spawnType = null;
    private int startIndex = -1;
    private int destinationIndex = -1;

    void Start()
    {
        SpawnGrid();
        SpawnPlayer();
        SpawnDestination();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(point, Camera.main.transform.forward);
            if (hit.collider)
            {
                var tile = hit.collider.GetComponent<Tile>();
                if (spawnType == "StartPoint")
                {
                    startIndex = tile.Index;
                    UpdatePlayerPosition();
                }
                else if (spawnType == "EndPoint")
                {
                    destinationIndex = tile.Index;
                    UpdateDestinationPosition();
                }
                else if (spawnType == "Path")
                {
                    hit.collider.gameObject.GetComponent<Tile>().Click();
                }
            }
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
        playerGO.GetComponent<Player>().StartGame(gridGO,startIndex,destinationIndex,Rows,Columns);
    }

    private void SpawnGrid()
    {
        var sprite = TilePrefab.GetComponent<SpriteRenderer>().sprite;
        
        // since we are using square tile
        // width and height will be same
        var width = sprite.rect.width;
        var pixelsPerUnit = sprite.pixelsPerUnit;
        var unitDistance = width / pixelsPerUnit;
        
        // calculate the start position from center (0,0)
        var startX = -(Columns * 0.5f * unitDistance - unitDistance * 0.5f);
        var startY = -(Rows * 0.5f * unitDistance - unitDistance * 0.5f);

        gridGO = new GameObject("Grid"); 
        for (int i = 0; i < Rows; ++i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                var tile = Instantiate(TilePrefab) as GameObject;
                tile.transform.SetParent(gridGO.transform);
                var spawnPos = Vector2.zero;
                spawnPos.x = startX + j * unitDistance;
                spawnPos.y = startY + i * unitDistance;
                tile.transform.position = spawnPos;
                tile.GetComponent<Tile>().Index = i*Columns + j;
            }
        }
    }

    private void SpawnPlayer()
    {
        playerGO = Instantiate(PlayerPrefab) as GameObject;
        startIndex = 0;
        UpdatePlayerPosition();
    }

    private void UpdatePlayerPosition()
    {
        playerGO.transform.position = gridGO.transform.GetChild(startIndex).position;
    }

    private void SpawnDestination()
    {
        destinationGO = Instantiate(DestinationPrefab) as GameObject;
        destinationIndex = Rows * Columns - 1;
        UpdateDestinationPosition();
    }

    private void UpdateDestinationPosition()
    {
        destinationGO.transform.position = gridGO.transform.GetChild(destinationIndex).position;
    }

    public void SetSpawnType(string spawnType)
    {
        this.spawnType = spawnType;
    }
}
