using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pet : MonoBehaviour
{
    public float walkSpeed;

    private NavMeshAgent agent;
    private AIState state;

    private float playerDistance;
    private Animator animator;
    private SkinnedMeshRenderer[] renderers;

    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        Location();

    }

    void Location() 
    {
        agent.SetDestination(new Vector3(CharacterManager.Instance.Player.transform.position.x+2f, CharacterManager.Instance.Player.transform.position.y, CharacterManager.Instance.Player.transform.position.z + 2f));
    }

}
