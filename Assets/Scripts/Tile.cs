using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isWalkable = true;
    public int Index = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click()
    {
        isWalkable = !isWalkable;
        if(!isWalkable)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            // TODO change color to red
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            // Change color to white
        }
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }
}
