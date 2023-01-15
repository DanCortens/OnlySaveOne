using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool angry;
    [SerializeField] private bool attacking;
    private bool flinch;
    [SerializeField] private float aggroRange;
    [SerializeField] private float meleeRange;
    [SerializeField] private float health;
    private Rigidbody rb;
    private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    private PlayerMovement player;
    [SerializeField] private AudioSource scream;
    [SerializeField] private AudioSource groan;
    [SerializeField] private AudioSource dead;
    public Enemy[] troup;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        attacking = false;
        angry = false;
        health = 80f;
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        agent.isStopped = true;
    }
    private void Update()
    {
        if (GameStateEngine.gse.state == GameStateEngine.GameState.Play)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (!angry)
            {
                if (dist <= aggroRange)
                {
                    //add vision cone if have time
                    scream.PlayOneShot(scream.clip);
                    GetAngry();
                    foreach (Enemy t in troup)
                        t.GetAngry();
                    
                }
            }
            else if (!flinch && health > 0f)
            {
                if (!groan.isPlaying && !scream.isPlaying)
                    groan.Play();
                agent.SetDestination(player.transform.position);
                if (dist <= meleeRange)
                {
                    Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                    Vector3 direction = (playerPos - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = lookRotation;
                    anim.SetBool("isRunning", false);
                    if (!attacking)
                    {
                        attacking = true;
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    anim.SetBool("isRunning", true);
                }
            }
        }
    }
    private IEnumerator Attack()
    {
        int r = Random.Range(0, 2);
        string attack = (r == 0) ? "strike1" : "strike2";
        anim.SetTrigger(attack);

        yield return new WaitForSeconds(0.8f);

        float maxAngle = 30f;
        Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 diff = playerPos - transform.position;
        Debug.Log($"Distance: {diff.magnitude}, x: {diff.x} z: {diff.z}");
        if (diff.magnitude < meleeRange)
        {
            float angle = Vector3.Angle(diff, transform.forward);
            Debug.Log($"Angle: {angle}");
            if (angle < maxAngle)
                player.TakeDamage(transform.position, 25);
            
        }

        yield return new WaitForSeconds(0.8f);
        attacking = false;
    }
    public void TakeDamage(float damage)
    {
        StopAllCoroutines();
        attacking = false;
        if (!angry)
        {
            scream.PlayOneShot(scream.clip);
            GetAngry();
            foreach (Enemy t in troup)
                GetAngry();
        }
        flinch = true;
        agent.isStopped = true;
        anim.SetTrigger("damaged");
        Invoke("StopFlinching", 0.4f);
        health -= damage;
        if (health <= 0f)
            Dies();
    }
    private void StopFlinching()
    {
        flinch = false;
        agent.isStopped = false;
    }
    protected void Dies()
    {
        anim.SetTrigger("dies");
        agent.isStopped = true;
        Destroy(gameObject, 3f);
    }
    public void GetAngry()
    {
        angry = true;
        agent.isStopped = false;
    }
}
