using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : Interactable
{
    private PlayerCombat playerCombat;
    [SerializeField] private int gun;

    public override void Interact()
    {
        playerCombat.PickUpGun(gun);
        Destroy(gameObject);
    }

    private void Start()
    {
        playerCombat = FindObjectOfType<PlayerCombat>();
    }
}
