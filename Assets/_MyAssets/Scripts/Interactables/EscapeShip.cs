using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EscapeShip : Interactable
{
    public GameObject winMenu;
    [SerializeField] private TMP_Text survivorText;
    public override void Interact()
    {
        winMenu.SetActive(true);
        GameStateEngine.gse.state = GameStateEngine.GameState.Menu;
        string name = PlayerPrefs.GetString("survivor");
        survivorText.text = $"You saved {name}";
    }

}
