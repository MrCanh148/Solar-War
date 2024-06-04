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
            /* Vector2 direction = targetObject.transform.position - laserOrigin.transform.position;
             RaycastHit2D hit = Physics2D.Raycast(laserOrigin.transform.position, direction.normalized, direction.magnitude);

             if (hit)
             {
                 laserLineRenderer.SetPosition(1, hit.point);
             }*/
            // Kích hoạt laser
            laserLineRenderer.enabled = true;
        }
        else
        {
            laserLineRenderer.enabled = false;
        }
    }
}
