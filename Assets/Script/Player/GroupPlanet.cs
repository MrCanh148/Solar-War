using System.Collections.Generic;
using UnityEngine;

public class GroupPlanet : MonoBehaviour
{
    public List<Character> characterChilds = new List<Character>();
    public Character masterStar;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        masterStar.satellites.Clear();
        foreach (Character c in characterChilds)
        {
            masterStar.satellites.Add(c);
            c.gameObject.SetActive(false);
            c.isCapture = true;
            c.host = masterStar;
            c.angle = Random.Range(0f, 360f);
            c.spinSpeed = RandomSpinSpeed(Random.Range(0.5f, 1f));
            c.radius = (c.tf.position - masterStar.tf.position).magnitude;
        }
        masterStar.ResetRadiusSatellite(masterStar);
        masterStar.gameObject.SetActive(true);

        foreach (Character c in characterChilds)
        {
            c.gameObject.SetActive(true);
        }
    }

    public float RandomSpinSpeed(float n)
    {
        int randomValue = Random.Range(0, 2);
        return randomValue == 0 ? -n : n;
    }
}
