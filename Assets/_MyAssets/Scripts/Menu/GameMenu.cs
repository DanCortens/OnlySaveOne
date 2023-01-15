using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject winScreen;
    
    public void RestartGame()
    {
        StartCoroutine(LoadAsynch("Level1"));
    }
    public void BackToMain()
    {
        StartCoroutine(LoadAsynch("Menu"));
    }
    public void Resume()
    {
        menuScreen.SetActive(false);
        GameStateEngine.gse.state = GameStateEngine.GameState.Play;
    }
    private IEnumerator LoadAsynch(string level)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);
        while (!operation.isDone)
        {
            yield return null;
        }

    }
}
