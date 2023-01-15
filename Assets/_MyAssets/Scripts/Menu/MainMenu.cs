using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text loading;
    public void ToggleInfo()
    {
        infoPanel.SetActive(!infoPanel.activeInHierarchy);
    }
    public void StartGame()
    {
        StartCoroutine(LoadAsynch());
        loading.gameObject.SetActive(true);
    }

    private IEnumerator LoadAsynch()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Level1");
        while (!operation.isDone)
        {
            loading.text = $"Loading {operation.progress}";
            yield return null;
        }
        
    }
}
