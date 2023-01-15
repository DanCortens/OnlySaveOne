using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    public GameObject interactPrompt;
    public LayerMask interactableMask;
    // Start is called before the first frame update
    void Start()
    {
        interactPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4, interactableMask))
        {
            interactPrompt.SetActive(true);
            if (Input.GetKeyDown("f"))
            {
                hit.transform.gameObject.GetComponent<Interactable>().Interact();
            }
        }
        else
        {
            interactPrompt.SetActive(false);
        }
    }
}
