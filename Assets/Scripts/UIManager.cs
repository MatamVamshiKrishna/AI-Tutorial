using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager GameManager;
    public Dropdown SpawnTypes;

    private void Start()
    {
        SpawnTypes.onValueChanged.AddListener(delegate {
            DropdownValueChanged();
        });
        GameManager.SetSpawnType(SpawnTypes.options[SpawnTypes.value].text);
    }

    public void StartGame()
    {
        GameManager.StartGame();
    }

    public void DropdownValueChanged()
    {
        GameManager.SetSpawnType(SpawnTypes.options[SpawnTypes.value].text);
    }
}
