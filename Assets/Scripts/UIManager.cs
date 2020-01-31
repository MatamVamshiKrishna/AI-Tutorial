using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;

    public void StartGame()
    {
        GameManager.StartGame();
    }
}
