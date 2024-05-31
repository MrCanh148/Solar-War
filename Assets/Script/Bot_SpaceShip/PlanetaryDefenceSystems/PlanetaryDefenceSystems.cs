using System.Collections.Generic;
using UnityEngine;

public class PlanetaryDefenceSystems : MonoBehaviour
{
    [SerializeField] Character owner;
    [SerializeField] List<Turret> turrets;
    // Start is called before the first frame update
    void Start()
    {
        if (owner.characterType == CharacterType.SmallPlanet)
        {
            foreach (Turret t in turrets)
            {
                t.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Turret t in turrets)
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
