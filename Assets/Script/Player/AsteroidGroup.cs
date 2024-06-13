using System.Collections.Generic;
using UnityEngine;

public class AsteroidGroup : MonoBehaviour
{
    [SerializeField] List<Character> listAsteroid;
    List<Vector3> AsteroidPosition = new List<Vector3>();

    private void Start()
    {
        foreach (Character c in listAsteroid)
        {
            AsteroidPosition.Add(c.tf.localPosition);
            c.velocity = SpawnPlanets.instance.RandomInitialVelocity(1.5f);
        }
    }


    public void OnInit()
    {
        for (int i = 0; i < listAsteroid.Count; i++)
        {
            if (listAsteroid[i] != null && !listAsteroid[i].gameObject.activeSelf)
            {
                SpawnPlanets.instance.ActiveCharacter(listAsteroid[i]);
                listAsteroid[i].transform.localPosition = AsteroidPosition[i];

            }
        }
    }

    public bool AllChildrenDeActive()
    {
        foreach (Character c in listAsteroid)
        {
            if (c.gameObject.activeSelf)
                return false;
        }
        return true;
    }
}
