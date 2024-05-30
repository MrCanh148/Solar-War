using UnityEngine;
using System.Collections.Generic;

public class ShootTarget : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float radius = 5f; // Bán kính vùng phát hiện
    [SerializeField] private float fireAngle = 20f; // Góc phía trước
    [SerializeField] private float NextShootIn = 1f; // Thời gian bắn đạn
    private float nextFireTime = 0f;

    private List<GameObject> ignoredTargets = new List<GameObject>();

    public void SetIgnoredTargets(List<GameObject> targets)
    {
        ignoredTargets = targets;
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (Collider2D hit in hits)
            {
                if (ignoredTargets.Contains(hit.gameObject)) continue;

                Character target = hit.gameObject.GetComponent<Character>();
                if ((target != null && (target.characterType == CharacterType.Asteroid || target.generalityType == GeneralityType.Planet)) || hit.gameObject.tag == "AirSpace1")
                {
                    Vector2 directionToTarget = hit.transform.position - transform.position;
                    float angle = Vector2.Angle(transform.up, directionToTarget);

                    if (angle <= fireAngle / 2)
                    {
                        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                        nextFireTime = Time.time + NextShootIn;
                        break;
                    }
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
