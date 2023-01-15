using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float sensX; //horizontal mouse sensitivity
    [SerializeField] private float sensY; //vertical    ^^     ^^ 

    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerPos;
    [SerializeField] private GameObject reticle;
    private float xRot;
    private float yRot;

    // Update is called once per frame
    void Update()
    {
        if (GameStateEngine.gse.state == GameStateEngine.GameState.Play)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            reticle.SetActive(true);

            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensX;

            yRot += mouseX;
            xRot = Mathf.Clamp(xRot - mouseY, -90f, 90f);
            transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
            orientation.rotation = Quaternion.Euler(0f, yRot, 0f);
            transform.position = playerPos.position + new Vector3(0f, 0.25f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            reticle.SetActive(false);
        }
    }
}
