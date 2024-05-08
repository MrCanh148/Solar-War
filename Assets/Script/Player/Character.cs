using UnityEngine;

public enum CharacterType
{
    Asteroid,
    Planet,
    Star,
    BlackHole,
    SmallPlanet,
    LifePlanet,
    GasGiantPlanet,
    SmallStar,
    MediumStar,
    BigStar,
    NeutronStar
}

public class Character : MonoBehaviour
{

    public CharacterType characterType;
    public Rigidbody2D rb;
    public Player host;
    public Transform tf;
    public float radius; // Bán kính quỹ đạo
    public float spinSpeed; // Tốc độ quay
    public float angle;
    public bool isCapture;
    public float captureZoneRadius;
    [SerializeField] public LineRenderer lineRenderer;
    [SerializeField] protected LayerMask characterLayer;


    protected virtual void Start()
    {
        OnInit();
    }
    protected virtual void OnInit()
    {
        radius = 5f;
        spinSpeed = 1f;
        angle = 0f;
        isCapture = false;
    }

    private void Update()
    {
        if (isCapture)
        {
            // Tính toán vị trí mới dựa trên góc quay và bán kính
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // Cập nhật vị trí của đối tượng
            transform.position = host.tf.position + new Vector3(x, y, 0f);
            //transform.RotateAround(host.tf.position, Vector3.forward, angle);

            // Tăng góc quay theo tốc độ
            angle += spinSpeed * Time.deltaTime;
            if (host != null && tf != null)
            {
                lineRenderer.SetPosition(1, tf.position);
                lineRenderer.SetPosition(0, host.tf.position);
            }


        }


    }

    private void FixedUpdate()
    {
        /* Character[] characters = FindObjectsOfType<Character>();
         foreach (Character c in characters)
         {
             if (c != this)
             {
                 Attract(c);
             }
         }*/
    }

    public void Attract(Character character)
    {
        Rigidbody2D rigidbody = character.rb;
        Vector3 direction = tf.position - character.tf.position;
        float distance = direction.magnitude;
        if (distance <= 0.01f)
            return;

        double forceMagnitude = (GameManager.instance.status.gravitationalConstant * rigidbody.mass * rb.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * (float)forceMagnitude;
        rigidbody.AddForce(force);
    }



}