using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidPickup : Interactable
{
    private PlayerMovement player;
    [SerializeField] private AudioSource pickupSound;
    public override void Interact()
    {
        pickupSound.PlayOneShot(pickupSound.clip);
        player.PickupMedkit();
        Destroy(gameObject,0.25f);
        Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

}
