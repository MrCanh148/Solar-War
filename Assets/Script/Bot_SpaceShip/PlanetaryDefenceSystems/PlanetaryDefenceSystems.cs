using System.Collections.Generic;
using UnityEngine;

public class PlanetaryDefenceSystems : MonoBehaviour
{
    [SerializeField] Character owner;
    [SerializeField] private GameObject[] TurretMiniCam;
    [SerializeField] List<Turret> turrets;

    [Header("AOM")]
    [SerializeField] MissileDef missileDefPrefab;
    [SerializeField] Transform firePoint;
    float timeCoolDownMissile;
    int quantityMissile;
    public List<MissileDef> missiles;
    bool isMissile;
    GameObject targetMissile;

    [Header("AOC")]
    public AntiOrbitalCannon antiOrbitalCannon;
    int quantityAOEC;
    [SerializeField] bool isAOC;
    GameObject targetAOC;
    public float timeAttackAOC;
    public float damageAOC = 10;

    int currentKill;
    void Start()
    {

        OnInit();
    }

    public void OnInit()
    {
        currentKill = -1;

        //Missile
        timeCoolDownMissile = 0;
        quantityMissile = 1;
        isMissile = false;
        for (int i = 0; i < quantityMissile; i++)
        {
            MissileDef missile = Instantiate(missileDefPrefab, firePoint.position, firePoint.rotation);
            missiles.Add(missile);
            missile.gameObject.SetActive(false);
            missile.source = this;
            missile.characterOwner = owner;
        }

        //AntiOrbitalCannon
        isAOC = false;
    }

    void Update()
    {

        OnChangeKill(owner.Kill);
        if (targetAOC != null)
        {
            timeAttackAOC += Time.deltaTime;

        }
    }

    private void FixedUpdate()
    {
        if (isMissile)
        {
            timeCoolDownMissile += Time.deltaTime;

        }
        if (!owner.EvolutionDone)
        {
            currentKill = -1;
        }
    }

    public void OnChangeKill(int newKill)
    {
        if (currentKill != newKill)
        {
            if (owner.EvolutionDone)
            {
                if (newKill == 0)
                {
                    UpdateQuantityTurret(1);
                    isMissile = false;
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
                    isAOC = true;
                }
                currentKill = newKill;
            }
            else
            {
                UpdateQuantityTurret(0);

            }

        }


    }


    public void UpdateQuantityTurret(int quantity)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            if (i < quantity)
            {
                if (TurretMiniCam != null && TurretMiniCam.Length > i && TurretMiniCam[i] != null)
                    TurretMiniCam[i].SetActive(true);

                turrets[i].gameObject.SetActive(true);
                turrets[i].OwnerCharacter = owner;
            }
            else
            {
                turrets[i].gameObject.SetActive(false);
                if (TurretMiniCam != null && TurretMiniCam.Length > i && TurretMiniCam[i] != null)
                    TurretMiniCam[i].SetActive(false);
            }
        }
    }

    public void ShotMissile(GameObject target)
    {
        if (missiles.Count > 0 && targetMissile != null)
        {
            missiles[0].gameObject.SetActive(true);
            missiles[0].SetTarget(target);
            missiles[0].characterOwner = this.owner;
            missiles[0].transform.position = firePoint.transform.position;
            missiles.Remove(missiles[0]);
        }
    }

    public void AvticeAOC(GameObject target)
    {
        antiOrbitalCannon.OwnerCharacter = owner;
        antiOrbitalCannon.targetObject = target;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        /*Test2 test = collision.GetComponent<Test2>();
        if (test != null)
        {
            if (timeCoolDownMissile > 3f)
            {
                targetMissile = test.gameObject;
                ShotMissile(targetMissile);
                timeCoolDownMissile = 0;
            }

            if (targetAOC == null)
            {
                if (isAOC)
                {
                    targetAOC = test.gameObject;
                    AvticeAOC(targetAOC);
                    antiOrbitalCannon.VFXPlay();
                }
            }
        }*/


        ShootTarget shootTarget = Cache.GetShootTargetCollider(collision);
        if (shootTarget != null)
        {
            if (shootTarget.hostAlien != owner)
            {
                if (timeCoolDownMissile > 3f)
                {
                    targetMissile = shootTarget.gameObject;
                    ShotMissile(targetMissile);
                    timeCoolDownMissile = 0;
                }
                if (isAOC && targetAOC == null)
                {
                    targetAOC = shootTarget.gameObject;
                    AvticeAOC(targetAOC);
                    if (timeAttackAOC > 1f)
                    {
                        shootTarget.heart -= damageAOC;
                        if (shootTarget.heart <= 0)
                        {
                            owner.Kill++;
                            shootTarget.gameObject.SetActive(false);
                        }
                        timeAttackAOC = 0;
                    }
                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*Test2 test = collision.GetComponent<Test2>();
        if (test != null)
        {
            if (targetMissile == test.gameObject)
            {
                targetMissile = null;
                ShotMissile(targetMissile);
            }

            if (targetAOC == test.gameObject)
            {
                targetAOC = null;
                AvticeAOC(targetAOC);
                antiOrbitalCannon.VFXStop();
            }
        }*/

        ShootTarget shootTarget = Cache.GetShootTargetCollider(collision);
        if (shootTarget != null)
        {
            Debug.Log("ra ngoai");
            if (targetMissile == shootTarget.gameObject)
            {
                targetMissile = null;
                ShotMissile(targetMissile);
            }
            if (targetAOC == shootTarget.gameObject)
            {
                targetAOC = null;
                AvticeAOC(targetAOC);
                antiOrbitalCannon.VFXStop();
            }
        }

    }
}
