using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float lineOfSightDistance = 30f;
    public float chaseSpeed = 5f;
    public float roamSpeed = 12f;
    public float roamRange = 20f;
    public float roamDuration = 999f;

    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 lastKnownPlayerPosition;
    private float roamTimer;
    private bool isChasing;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        lastKnownPlayerPosition = transform.position;
        roamTimer = roamDuration;
        isChasing = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // look for PLAYER
        if (IsPlayerInLineOfSight())
        {
            lastKnownPlayerPosition = player.position;
            isChasing = true;
        }

        // PLAYER is visible, chase
        if (isChasing)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            // PLAYER is lost, go to last seen position
            agent.speed = roamSpeed;
            agent.SetDestination(lastKnownPlayerPosition);

            // roam when PLAYER is lost after brief pause
            roamTimer -= Time.deltaTime;
            if (roamTimer <= 0)
            {
                lastKnownPlayerPosition = GetRandomRoamingPosition();
                roamTimer = roamDuration;
            }
        }
    }

    private bool IsPlayerInLineOfSight()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, lineOfSightDistance))
        {
            if (hit.transform == player)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 GetRandomRoamingPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRange;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, roamRange, UnityEngine.AI.NavMesh.AllAreas);
        return hit.position;
    }
}