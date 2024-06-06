
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public GameObject owner;
    int hp;
    public float angle = 0;
    public float spinSpeed = 1;
    public float radius = 3;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        hp = 5;
        if (owner != null)
        {
            transform.SetParent(owner.transform);
        }
    }

    private void Update()
    {
        if (owner != null)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // Cập nhật vị trí của đối tượng
            transform.position = owner.transform.position + new Vector3(x, y, 0f);
            // Tăng góc quay theo tốc độ
            angle += spinSpeed * Time.deltaTime;
        }
        else
        {
            transform.SetParent(null);
        }

        if (owner != null && lineRenderer != null && lineRenderer.enabled == true)
        {
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.SetPosition(0, owner.transform.position);
        }
    }


    public void OnHit()
    {
        //hp--;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
