using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject Player;
    public float Xspawn, Yspawn;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float XPos = Random.Range(-Xspawn, Xspawn);
            float YPos = Random.Range(-Yspawn, Yspawn);

            Player.transform.position = new Vector3(XPos, YPos);
        }
    }
}
