using System.Collections.Generic;
using UnityEngine;

public class PlanetaryDefenceSystems : MonoBehaviour
{
    [SerializeField] Character owner;
    [SerializeField] List<Turret> turrets;

    [Header("Missile")]
    [SerializeField] MissileDef missileDefPrefab;
    [SerializeField] Transform firePoint;
    float timeCoolDownMissile;
    int quantityMissile;
    GameObject targetMissile;
    public List<MissileDef> missiles;
    bool isMissile;


    int currentKill;
    // Start is called before the first frame update
    void Start()
    {

        OnInit();
    }

    public void OnInit()
    {
        currentKill = -1;
        timeCoolDownMissile = 0;
        quantityMissile = 1;
        isMissile = false;
        for (int i = 0; i < quantityMissile; i++)
        {
            MissileDef missile = Instantiate(missileDefPrefab, firePoint.position, firePoint.rotation);
            missiles.Add(missile);
            missile.gameObject.SetActive(false);
            missile.source = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (owner.EvolutionDone)
        {
            OnChangeKill(owner.Kill);
        }
    }

    private void FixedUpdate()
    {
        if (isMissile)
        {
            timeCoolDownMissile += Time.deltaTime;

        }
    }

    public void OnChangeKill(int newKill)
    {
        if (currentKill != newKill)
        {

            if (newKill == 0)
            {
                UpdateQuantityTurret(1);
            }
            else if (newKill == 6)
            {
                UpdateQuantityTurret(2);
            }
            else if (newKill == 12)
            {
                UpdateQuantityTurret(2);
                isMissile = true;
            }
            else if (newKill == 24)
            {
                UpdateQuantityTurret(3);
            }
            else if (newKill == 36)
            {
                UpdateQuantityTurret(4);
            }

        }

        currentKill = newKill;
    }


    public void UpdateQuantityTurret(int quantity)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            if (i < quantity)
            {
                turrets[i].gameObject.SetActive(true);
            }
            else
            {
                turrets[i].gameObject.SetActive(false);

            }
        }
    }

    public void ShotMissile(GameObject target)
    {
        if (missiles.Count > 0)
        {
            missiles[0].gameObject.SetActive(true);
            missiles[0].SetTarget(target);
            missiles[0].characterOwner = this.owner;
            missiles[0].transform.position = firePoint.transform.position;
            missiles.Remove(missiles[0]);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Test2 test = collision.GetComponent<Test2>();
        if (test != null)
        {

            if (timeCoolDownMissile > 3f)
            {
                ShotMissile(test.gameObject);
                timeCoolDownMissile = 0;
            }
        }
    }
}
