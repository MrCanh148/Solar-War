using UnityEngine;
using System.Collections.Generic;

public class ShootTarget : MonoBehaviour
{
    [Header("0-Bullet === 1-Laser === 2-Missile")]
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float radius = 5f; // Bán kính vùng phát hiện
    [SerializeField] private float fireAngle = 20f; // Góc phía trước

    [SerializeField] private float bulletFireInterval = 0.1f; // Thời gian bắn đạn liên tiếp
    [SerializeField] private float laserFireInterval = 2f; // Thời gian bắn laser
    [SerializeField] private float missileFireInterval = 3f; // Thời gian phóng tên lửa

    private float nextFireTime = 0f;
    private Character host;
    private List<GameObject> ignoredTargets = new List<GameObject>();
    private RandomMovement botAirSpace;

    private void Start()
    {
        botAirSpace = GetComponent<RandomMovement>();
    }

    public void SetIgnoredTargets(List<GameObject> targets, Character hostBase)
    {
        ignoredTargets = targets;
        host = hostBase;
    }

    private void Update()
    {
        switch (botAirSpace.type)
        {
            case BotAirSpace.AirSpaceType.Fighter:
                if (Time.time >= nextFireTime)
                {
                    Shoot(0, bulletFireInterval);
                }
                break;

            case BotAirSpace.AirSpaceType.Cruiser:
                if (Time.time >= nextFireTime)
                {
                    Shoot(1, laserFireInterval);
                }
                break;

            case BotAirSpace.AirSpaceType.MissileBoat:
                if (Time.time >= nextFireTime)
                {
                    Shoot(2, missileFireInterval);
                }
                break;
        }
    }

    private void Shoot(int bulletIndex, float fireInterval)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (ignoredTargets.Contains(hit.gameObject)) continue;

            Character target = hit.gameObject.GetComponent<Character>();          

            if ((target != null && (target.characterType == CharacterType.Asteroid || target.generalityType == GeneralityType.Planet)) || hit.gameObject.tag == "AirSpace1")
            {
                if (target.host == host) continue;

                Vector2 directionToTarget = hit.transform.position - transform.position;
                float angle = Vector2.Angle(transform.up, directionToTarget);

                if (angle <= fireAngle / 2)
                {
                    GameObject a = Instantiate(bulletPrefab[bulletIndex], firePoint.position, firePoint.rotation);
                    nextFireTime = Time.time + fireInterval;

                    Missile missile = a.GetComponent<Missile>();
                    if (missile != null)
                        missile.SetTarget(hit.gameObject);

                    Laser laser = a.GetComponent<Laser>();
                    if (laser != null)
                        laser.SetTarget(hit.gameObject, firePoint);

                    break;
                }
            }
        }
    }

    // Vẽ Vùng bắn đạn
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 forward = transform.up * radius;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-fireAngle / 2, Vector3.forward);
        Quaternion rightRayRotation = Quaternion.AngleAxis(fireAngle / 2, Vector3.forward);

        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, leftRayDirection);
        Gizmos.DrawRay(transform.position, rightRayDirection);
    }
}
