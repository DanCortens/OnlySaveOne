using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : Interactable
{
    [SerializeField] private string survivorName;
    [SerializeField] private GameObject survivorFollower;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Animator fadeToBlack;

    public override void Interact()
    {
        StartCoroutine(SelectSurvivor());
    }

    private IEnumerator SelectSurvivor()
    {
        GameStateEngine.gse.state = GameStateEngine.GameState.InGameMenu;
        fadeToBlack.SetTrigger("fadeOut");
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetString("survivor", survivorName);
        Vector3 offset = new Vector3(0f, 1f, 1f);
        Instantiate(survivorFollower, spawnLocation.position, Quaternion.identity);
        player.MovePlayer(spawnLocation.position + offset);
        GameStateEngine.gse.state = GameStateEngine.GameState.Play;
        fadeToBlack.SetTrigger("fadeIn");
    }
}
