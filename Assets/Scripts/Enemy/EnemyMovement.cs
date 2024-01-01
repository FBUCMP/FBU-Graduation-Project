using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

// navmesh stuff not used in this project FOR NOW
[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private float stillDelay = 1f;
    //private LookAtIK LookAt;
    private NavMeshAgent agent;

    private const string IsWalking = "IsWalking";

    private static NavMeshTriangulation triangulation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        //LookAt = GetComponent<LookAtIK>();
        if (triangulation.vertices == null || triangulation.vertices.Length == 0)
        {
            triangulation = NavMesh.CalculateTriangulation();
        }
    }

    private void Start()
    {
        StartCoroutine(Roam());
    }

    private void Update()
    {
        animator.SetBool(IsWalking, agent.velocity.magnitude > 0.01f);
        /*
        if (LookAt != null)
        {
            LookAt.lookAtTargetPosition = Agent.steeringTarget + transform.forward;
        }
        */
    }

    private IEnumerator Roam()
    {
        WaitForSeconds wait = new WaitForSeconds(stillDelay);

        while (enabled)
        {
            int index = Random.Range(1, triangulation.vertices.Length);
            agent.SetDestination(
                Vector3.Lerp(
                    triangulation.vertices[index - 1],
                    triangulation.vertices[index],
                    Random.value
                )
            );
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
            yield return wait;
        }
    }

    public void StopMoving()
    {
        StopAllCoroutines();
        agent.isStopped = true;
        agent.enabled = false;
    }
}