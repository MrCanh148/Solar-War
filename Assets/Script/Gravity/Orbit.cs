using System.Collections;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] float radius, rotationSpeed;
    [SerializeField] private LineRenderer lineToCenter;

    private Transform center;
    private Character planet1;
    private Vector3 orbitPosition;
    public float TimeConnect;
    [SerializeField] private float timeLimit = 2;
    private bool isConnecting;

    private void Start()
    {
        lineToCenter.positionCount = 2;
        lineToCenter.enabled = false;
        planet1 = GetComponent<Character>();
    }

    private void FixedUpdate()
    {
        if (isConnecting)
        {
            TimeConnect += Time.deltaTime;
        }

        if (TimeConnect >= timeLimit)
        {
            orbitPosition = CalculateOrbitPosition();

            if (center != null) 
            {
                transform.position = orbitPosition;

                lineToCenter.SetPosition(0, center.position);
                lineToCenter.SetPosition(1, transform.position);
                lineToCenter.enabled = true;
            }
        }
        else
        {
            lineToCenter.enabled = false;
        }
    }

    private Vector3 CalculateOrbitPosition()
    {
        StartCoroutine(SpawnNewPlayer(3f));
        if (center == null)
        {
            return transform.position;
        }

        float angleInRadians = (Time.time * rotationSpeed) * Mathf.Deg2Rad;
        float x = Mathf.Cos(angleInRadians) * radius;
        float y = Mathf.Sin(angleInRadians) * radius;

        Vector3 newPosition = new Vector3(x, y, 0) + center.position;

        return newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character planet2 = collision.GetComponent<Character>();

        if (planet2 == null)
            return;

        if (planet2.rb.mass >= planet1.rb.mass)
        {
            center = planet2.transform;
            isConnecting = true;
        }
        else
        {
            center = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isConnecting = false;
        TimeConnect = 0;
        lineToCenter.enabled = false;
    }

    private IEnumerator SpawnNewPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        center = null; 
        isConnecting = false;
    }
}
