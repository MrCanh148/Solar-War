using UnityEngine;

public class AntiOrbitalCannon : MonoBehaviour
{
    public GameObject laserOrigin; // GameObject A, nguồn phát laser
    public GameObject targetObject; // GameObject B, đích nhắm  
    public LineRenderer laserLineRenderer;

    private void Update()
    {
        ShootLaser();
    }

    private void ShootLaser()
    {
        if (targetObject != null)
        {
            // Cập nhật vị trí đầu và cuối của laser
            laserLineRenderer.SetPosition(0, laserOrigin.transform.position);
            laserLineRenderer.SetPosition(1, targetObject.transform.position);
            Vector2 direction = (Vector2)(targetObject.transform.position - laserOrigin.transform.position);
            RaycastHit2D[] hits = Physics2D.RaycastAll(laserOrigin.transform.position, direction.normalized, direction.magnitude);

            foreach (RaycastHit2D h in hits)
            {
                if (h.collider.CompareTag(Constant.TAG_AirSpace1))
                {
                    Debug.Log("hit");
                    laserLineRenderer.SetPosition(1, h.point);
                }
            }


            // Kích hoạt laser
            laserLineRenderer.enabled = true;
        }
        else
        {
            laserLineRenderer.enabled = false;
        }
    }
}
