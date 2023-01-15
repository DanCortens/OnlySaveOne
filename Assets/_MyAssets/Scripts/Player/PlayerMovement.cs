using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public event System.Action modChanged;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 7.5f;
    [SerializeField] private float runSpeed = 11.5f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private bool grounded;
    [SerializeField] private float accel;
    [SerializeField] private float modifier;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private bool canJump;
    [SerializeField] private float airSlowdown;
    [Header("UI")]
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject[] medkitUI;
    [Header("Misc")]
    [SerializeField] private Transform orientation;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int medkits;
    [SerializeField] private int hp;
    private const int MAX_HP = 500;

    public bool aiming = false;
    public bool sprinting = false;

    private float horzInput;
    private float vertInput;

    private Vector3 moveDir;
    private CharacterController controller;
    public AudioSource footstepControl;
    public AudioSource healSound;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        hp = MAX_HP;
    }
    private void Start()
    {
        hpText.text = $"{hp}";
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
        GetInput();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void GetInput()
    {
        if (GameStateEngine.gse.state == GameStateEngine.GameState.Play)
        {
            horzInput = Input.GetAxisRaw("Horizontal");
            vertInput = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(horzInput) > 0f || Mathf.Abs(vertInput) > 0f)
                footstepControl.Play();
            else
                footstepControl.Stop();

            if (Input.GetKeyDown(KeyCode.E) && medkits > 0)
            {
                SelfHeal();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                sprinting = true;
                aiming = false;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                sprinting = false;
        }
    }
    private void Movement()
    {
        Vector3 forward = orientation.right;
        Vector3 right = orientation.forward; 
        float curSpeedX = (sprinting ? runSpeed : walkSpeed) * horzInput;
        float curSpeedY = (sprinting ? runSpeed : walkSpeed) * vertInput;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = movementDirectionY;
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            moveDirection.y = jumpForce;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }
        if (!grounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        controller.Move(moveDirection * Time.deltaTime);
    }
    private void MoveMod()
    {
        modifier = (sprinting) ? 1f : (aiming)? -0.8f : 0f;
    }
    public void TakeDamage(Vector3 attackerPos, int damage)
    {
        Vector3 ajusted = new Vector3(attackerPos.x, transform.position.y, attackerPos.z);
        Vector3 dir = (transform.position - ajusted).normalized;
        //float knockback = jumpForce * 1.2f;
        //controller.Move(dir * knockback);
        ChangeHealth(-damage);
    }
    private void SelfHeal()
    {
        healSound.PlayOneShot(healSound.clip);
        UseMedkit();
        ChangeHealth(100);
    }
    public void UseMedkit()
    {
        medkits--;
        medkitUI[medkits].SetActive(false);
    }
    public void PickupMedkit()
    {
        medkitUI[medkits].SetActive(true);
        medkits++;
    }
    private void ChangeHealth(int amount)
    {
        hp = Mathf.Clamp(hp + amount, 0, MAX_HP);
        hpText.text = $"{hp}";
        if (hp == 0)
        {
            GameOver();
        }
    }
    private void ToggleMenu()
    {
        if (menuScreen.activeInHierarchy)
        {
            GameStateEngine.gse.state = GameStateEngine.GameState.Play;
            menuScreen.SetActive(false);
        }
        else
        {
            GameStateEngine.gse.state = GameStateEngine.GameState.Menu;
            menuScreen.SetActive(true);
        }
        

    }
    private void GameOver()
    {
        GameStateEngine.gse.state = GameStateEngine.GameState.Menu;
        gameOverScreen.SetActive(true);
    }
    public void MovePlayer(Vector3 location)
    {
        controller.enabled = false;
        transform.position = location;
        controller.enabled = true;
    }
}
