using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Character plant;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed = 0.125f;
    private Vector3 targetPosition;
    [SerializeField] private Camera m_Camera;
    float currentSize;

    private void Start()
    {
        currentSize = 6f + (int)plant.characterType * 0.2f;
        if (m_Camera != null)
        {
            m_Camera.orthographicSize = currentSize;
        }
    }


    void FixedUpdate()
    {
        if (plant.canControl)
        {
            Vector3 desiredPosition = plant.tf.position + offset;
            targetPosition = desiredPosition;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;

        OnChangeSize(6f + (int)plant.characterType * 0.2f);
    }

    public void OnChangeSize(float newSize)
    {
        if (currentSize != newSize)
        {
            if (m_Camera != null)
            {
                DOTween.To(() => m_Camera.orthographicSize, x => m_Camera.orthographicSize = x, newSize, 2f);
            }

        }

        currentSize = newSize;
    }
}
