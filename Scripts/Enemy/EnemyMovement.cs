using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private NavMeshAgent agent;
    public float speed = 5.0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            agent.speed = speed;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}