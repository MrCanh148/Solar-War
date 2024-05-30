using System.Collections.Generic;
using UnityEngine;

public class AsteroidGroup : MonoBehaviour
{
    [SerializeField] List<Character> listAsteroid;

    private void Start()
    {
        foreach (Character c in listAsteroid)
        {
            c.velocity = SpawnPlanets.instance.RandomInitialVelocity();
        }
    }
}
