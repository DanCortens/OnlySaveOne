using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private bool shooting;
    private bool[] hasGun = { true, false, false };
    private int currWeapon;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private AudioSource[] sounds;
    [SerializeField] private AudioSource swapSound;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private GameObject impact;
    [SerializeField] private GameObject bloodSplatter;
    [SerializeField] private float[] damage;
    [SerializeField] private float[] range;
    [SerializeField] private float[] shotCD;
    [SerializeField] private Camera mainCam;
    private PlayerMovement playerMove;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
        currWeapon = 0;
        weapons[currWeapon].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateEngine.gse.state == GameStateEngine.GameState.Play)
        {
            GetInput();
        }
    }
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currWeapon != 0)
        {
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasGun[1] && currWeapon != 1)
        {
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasGun[2] && currWeapon != 2)
        {
            SwitchWeapon(2);
        }
        if (Input.GetMouseButton(0))
        {
            if (!shooting)
            {
                shooting = true;
                Shoot();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            playerMove.aiming = true;
            playerMove.sprinting = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            playerMove.aiming = false;
        }
    }
    public void SwitchWeapon(int newWeapon)
    {
        swapSound.PlayOneShot(swapSound.clip);
        weapons[currWeapon].SetActive(false);
        currWeapon = newWeapon;
        weapons[currWeapon].SetActive(true);
    }
    private void Shoot()
    {
        StartCoroutine(ShotCD(shotCD[currWeapon]));
        particles[currWeapon].Play();
        sounds[currWeapon].PlayOneShot(sounds[currWeapon].clip);
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, range[currWeapon]))
        {
            Debug.Log($"{hit.transform.name} hit!");
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                GameObject imp = Instantiate(bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(imp, 2f);
                enemy.TakeDamage(damage[currWeapon]);
            }
            else
            {
                GameObject imp = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(imp, 2f);
            } 
                
            
        }
    }
    private IEnumerator ShotCD(float t)
    {
        yield return new WaitForSeconds(t);
        shooting = false;
    }
    public void PickUpGun(int gun)
    {
        hasGun[gun] = true;
        SwitchWeapon(gun);
    }
}
