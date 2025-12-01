using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float shootInterval = 1.5f;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 15f;

    Transform player;
    NavMeshAgent agent;
    float shootTimer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (player == null) return;

        agent.SetDestination(player.position);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Projectile proj = bullet.GetComponent<Projectile>();
        if (proj != null) proj.damage = 2; // boss bullet does 2 dmg

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = shootPoint.forward * bulletSpeed;
    }
}
