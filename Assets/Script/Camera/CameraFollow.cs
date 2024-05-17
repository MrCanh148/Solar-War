using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Character plant;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 0.125f;
    private Vector3 targetPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        plant = player.GetComponent<Character>();
        targetPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (plant.canControl)
        {
            Vector3 desiredPosition = player.transform.position + offset;
            targetPosition = desiredPosition;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}
