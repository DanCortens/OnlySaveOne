using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEngine : MonoBehaviour
{
    public enum GameState
    {
        Play,
        InGameMenu,
        Menu
    }
    public static GameStateEngine gse { get; private set; }
    public GameState state;
    // Start is called before the first frame update
    void Start()
    {
        if (gse != null && gse != this)
            Destroy(this);
        else
            gse = this;
        gse.state = GameState.Play;
    }

    // Update is called once per frame
    void Update()
    {
        if (gse.state == GameState.Menu)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
