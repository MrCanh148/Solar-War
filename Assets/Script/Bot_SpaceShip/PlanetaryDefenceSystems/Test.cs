using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform tf;
    public Transform centerRotation;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(mousePosition.y - centerRotation.position.y, mousePosition.x - centerRotation.position.x);
        float radius = (centerRotation.position - tf.position).magnitude;
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        tf.position = centerRotation.position + new Vector3(x, y, 0f);
        tf.rotation = new Quaternion(0, 0, angle, 0f);
    }
}
