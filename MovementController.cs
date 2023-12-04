using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
    private Camera cam;
    private Ray ray;

    private bool isMoving;
    private Animator anim;
    private NavMeshAgent agent;
    private NavMeshPath navMeshPath;
    [SerializeField] private ParticleSystem arrowParticle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        cam = Camera.main;
        navMeshPath = new NavMeshPath();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (CanReachTarget())
            {
                agent.SetPath(navMeshPath);
                isMoving = true;
                anim.SetBool("WALKING", isMoving);
            }
        }
        if (isMoving && agent.remainingDistance == 0)
        {
            isMoving = false;
            anim.SetBool("WALKING", isMoving);
        }
    }
    private bool CanReachTarget()
    {
        if (agent.CalculatePath(GetMouseWorldPosition(), navMeshPath) &&
            navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            return true;
        }
        return false;
    }
    private Vector3 GetMouseWorldPosition()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            arrowParticle.transform.position = hit.point;
            arrowParticle.Play();
            return hit.point;
        }
        return Vector3.zero;
    }
}
