using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float TimeLimit = 0.5f;
    private float timeAppear = 0f;
    private GameObject target;
    private Transform firePoint;
    [HideInInspector] public Character characterOwner;

    private void Start()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        DrawLaser();
    }

    private void DrawLaser()
    {
        if (target != null && firePoint != null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.transform.position);

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, target.transform.position - firePoint.position, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                    ReSpawnPlayer.Instance.ResPlayer();
                else
                    Destroy(hit.collider.gameObject); 
            }
        }
    }

    public void SetTarget(GameObject newTarget, Transform newFirePoint)
    {
        target = newTarget;
        firePoint = newFirePoint;
    }
}
