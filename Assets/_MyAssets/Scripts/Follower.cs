using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    private NavMeshAgent agent;
    [SerializeField] private Animator anim;
    [SerializeField] private float followRange;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        agent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateEngine.gse.state == GameStateEngine.GameState.Play)
        {
            agent.SetDestination(player.transform.position);
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= followRange)
            {
                anim.SetBool("isRunning", false);
                Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
                Vector3 direction = (playerPos - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = lookRotation;
            }
            else
            {
                anim.SetBool("isRunning", true);
            }
        }
    }
}
