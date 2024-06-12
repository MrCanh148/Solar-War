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

    public float heart; // Máu airspace
    private float nextFireTime = 0f;
     public Character hostAlien;
    private List<GameObject> ignoredTargets = new List<GameObject>();
    private RandomMovement moveRandom;
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        moveRandom = GetComponent<RandomMovement>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetIgnoredTargets(List<GameObject> targets, Character HostBase)
    {
        ignoredTargets = targets;
        hostAlien = HostBase;
    }

    private void Update()
    {
        switch (moveRandom.type)
        {
            case RandomMovement.AirSpaceType.Fighter:
                if (Time.time >= nextFireTime)
                {
                    Shoot(0, bulletFireInterval);
                }
                break;

            case RandomMovement.AirSpaceType.Cruiser:
                if (Time.time >= nextFireTime)
                {
                    Shoot(1, laserFireInterval);
                }
                break;

            case RandomMovement.AirSpaceType.MissileBoat:
                if (Time.time >= nextFireTime)
                {             
                    Shoot(2, missileFireInterval);
                }
                break;
        }

        if (hostAlien == null || !hostAlien.gameObject.activeSelf)
        {
            hostAlien = null;
            SpriteRenderer.color = Color.white;
        }
        else if (hostAlien.isPlayer || (hostAlien.host != null && hostAlien.host.isPlayer))
            SpriteRenderer.color = Color.green;
        else
            SpriteRenderer.color = Color.red;
    }

    private void Shoot(int bulletIndex, float fireInterval)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (ignoredTargets.Contains(hit.gameObject)) continue;

            Character target = hit.gameObject.GetComponent<Character>();
            ShootTarget characterTarget = hit.gameObject.GetComponent<ShootTarget>();
            if (hit.gameObject.tag == "AirSpace1" && characterTarget.hostAlien != null && hostAlien != null && characterTarget.hostAlien.myFamily == hostAlien.myFamily) continue;

            if (target != null && (target.generalityType != GeneralityType.BlackHole) || hit.gameObject.tag == "AirSpace1")
            {                  
                if (target != null && hostAlien != null && (target.myFamily == hostAlien.myFamily)) continue;

                Vector2 directionToTarget = hit.transform.position - transform.position;
                float angle = Vector2.Angle(transform.up, directionToTarget);

                if (angle <= fireAngle / 2)
                {
                    GameObject a = Instantiate(bulletPrefab[bulletIndex], firePoint.position, firePoint.rotation);

                    // Gắn các thuộc tính cần thiết cho các loại đạn
                    Bullet bullet = a.GetComponent<Bullet>();
                    if (bullet != null)
                        bullet.characterOwner = hostAlien;

                    Missile missile = a.GetComponent<Missile>();
                    if (missile != null)
                    {
                        missile.SetTarget(hit.gameObject);
                        missile.characterOwner = hostAlien;
                    }                       

                    Laser laser = a.GetComponent<Laser>();
                    if (laser != null)
                    {
                        laser.SetTarget(hit.gameObject, firePoint);
                        laser.characterOwner = hostAlien;
                    }

                    nextFireTime = Time.time + fireInterval;
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
