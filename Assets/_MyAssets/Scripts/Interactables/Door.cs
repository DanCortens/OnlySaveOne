using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open;
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        if (open)
        {
            open = false;
            anim.SetTrigger("close");
        }
        else
        {
            open = true;
            anim.SetTrigger("open");
        }
    }
}
