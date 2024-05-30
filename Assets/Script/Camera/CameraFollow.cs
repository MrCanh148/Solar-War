using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Character plant;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 0.125f;
    private Vector3 targetPosition;
    [SerializeField] private Camera m_Camera;


    void FixedUpdate()
    {
        if (plant.canControl)
        {
            Vector3 desiredPosition = plant.tf.position + offset;
            targetPosition = desiredPosition;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (m_Camera != null)
        {
            m_Camera.orthographicSize = 6f + (int)plant.characterType * 0.2f;
        }
    }
}
