using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is an abstract class to force anything the player needs to interact with to have an Interact function
public abstract class Interactable : MonoBehaviour
{
    abstract public void Interact();
}
