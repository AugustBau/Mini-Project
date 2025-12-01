using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float fieldOfView = 120f;
    public LayerMask visionMask;   // set to include Player and Walls
    public float roamRadius = 8f;
    public int touchDamage = 1;

    Transform player;
    NavMeshAgent agent;
    Vector3 roamPoint;

    bool chasing;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PickNewRoamPoint();

        // Notify GameManager when this enemy dies
        Health hp = GetComponent<Health>();
        if (hp != null)
        {
            hp.OnDeath += () => GameManager.Instance.OnEnemyKilled();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (CanSeePlayer())
        {
            chasing = true;
            agent.SetDestination(player.position);
        }
        else if (chasing)
        {
            // lost sight – go back to roaming
            chasing = false;
            PickNewRoamPoint();
        }

        if (!chasing && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            PickNewRoamPoint();
        }
    }

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = (player.position - transform.position);
        float dist = dirToPlayer.magnitude;
        if (dist > detectionRange) return false;

        dirToPlayer.Normalize();
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > fieldOfView * 0.5f) return false;

        // Raycast to check if walls block view
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToPlayer, out RaycastHit hit, detectionRange, visionMask))
        {
            return hit.collider.CompareTag("Player");
        }
        return false;
    }

    void PickNewRoamPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * roamRadius;
        randomDir += GameManager.Instance.enemySpawnCenter.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, roamRadius, NavMesh.AllAreas))
        {
            roamPoint = hit.position;
            agent.SetDestination(roamPoint);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health hp = other.GetComponent<Health>();
            if (hp == null)
                hp = other.GetComponentInParent<Health>();

            if (hp != null)
            {
                hp.TakeDamage(touchDamage);
                // Optional: knockback, sound, etc.
            }
        }
    }
}
