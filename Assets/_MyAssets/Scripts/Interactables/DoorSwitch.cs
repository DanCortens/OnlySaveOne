using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : Interactable
{
    [SerializeField] private Door connectedDoor;
    private float cooldown;
    private bool canUse;
    public override void Interact()
    {
        if (canUse)
        {
            canUse = false;
            connectedDoor.ToggleDoor();
            Invoke("Cooldown", cooldown);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canUse = true;
        cooldown = 0.5f;
    }
    private void Cooldown()
    {
        canUse = true;
    }

}
