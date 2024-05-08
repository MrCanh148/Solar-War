using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform center;
    [SerializeField] float radius, rotationSpeed;
    [SerializeField] private LineRenderer lineToCenter;
    [SerializeField] private LineRenderer orbitLine;

    private int segments = 100;
    private float angle;
    private Vector3 orbitPosition;

    private void Start()
    {
        lineToCenter.positionCount = 2;
        orbitLine.positionCount = segments + 1;
    }

    private void FixedUpdate()
    {
        orbitPosition = CalculateOrbitPosition();
        transform.position = orbitPosition;

        lineToCenter.SetPosition(0, center.position);
        lineToCenter.SetPosition(1, transform.position);
        DrawOrbitCircle();
    }

    private Vector3 CalculateOrbitPosition()
    {
        float angleInRadians = (Time.time * rotationSpeed) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleInRadians) * radius;
        float y = Mathf.Sin(angleInRadians) * radius;

        Vector3 newPosition = new Vector3(x, y, 0) + center.position;

        return newPosition;
    }

    private void DrawOrbitCircle()
    {
        angle = 360f / segments;
        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * (angle * i)) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * (angle * i)) * radius;
            orbitLine.SetPosition(i, new Vector3(x, y, 0) + center.position);
        }
    }
}
