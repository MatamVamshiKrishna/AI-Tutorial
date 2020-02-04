using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isWalkable = true;
    public int Index = -1;
    public int Weight = 1;
    public TextMesh TextMesh;

    // Start is called before the first frame update
    void Start()
    {
        //TextMesh.text = Weight.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TextMesh.text = Weight.ToString();
    }

    public void Click()
    {
        isWalkable = !isWalkable;
        if(!isWalkable)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Weight = 100;
            // TODO change color to red
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            Weight = 1;
            // Change color to white
        }
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void SetWeight(int parentWeight)
    {
        Weight += parentWeight;

        TextMesh.text = Weight.ToString();
    }
}
