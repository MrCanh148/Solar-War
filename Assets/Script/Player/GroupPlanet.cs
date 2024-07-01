using System.Collections.Generic;
using UnityEngine;

public class GroupPlanet : MonoBehaviour
{
    public List<Character> characterChilds = new List<Character>();
    List<CharacterType> characterTypes = new List<CharacterType>();
    public Character masterStar;
    CharacterType masterStarType;

    private void Start()
    {
        //masterStarType = masterStar.characterType;
        //foreach (Character c in characterChilds)
        //{
        //    characterTypes.Add(c.characterType);
        //}

        OnInit();
    }

    public void OnInit()
    {
        this.gameObject.GetComponent<GroupPlanet>().enabled = true;
        masterStar.satellites.Clear();
        for (int i = 0; i < characterChilds.Count; i++)
        {
            Character c = characterChilds[i];
            c.isSetup = true;
            masterStar.satellites.Add(c);
            c.gameObject.SetActive(false);
            c.isCapture = true;
            c.host = masterStar;
            c.angle = Random.Range(0f, 360f);
            c.spinSpeed = RandomSpinSpeed(Random.Range(0.5f, 1f));
            c.radius = (c.tf.position - masterStar.tf.position).magnitude;
            //c.rb.mass = (SpawnPlanets.instance.GetRequiredMass(characterTypes[i] + 1) - SpawnPlanets.instance.GetRequiredMass(characterTypes[i])) / 2;
        }
        masterStar.ResetRadiusSatellite(masterStar);
        //masterStar.rb.mass = (SpawnPlanets.instance.GetRequiredMass(masterStarType + 1) - SpawnPlanets.instance.GetRequiredMass(masterStarType)) / 2;
        masterStar.isSetup = true;
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
