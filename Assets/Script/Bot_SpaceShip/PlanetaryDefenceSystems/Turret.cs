using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public ShootTarget target1;
    public Test2 target;
    public List<BulletTurret> bullets;
    [SerializeField] Transform shotPoint;
    [SerializeField] BulletTurret bulletPrefab;
    public float timeColdown = 1;
    public Character OwnerCharacter;

    [SerializeField] private GameObject GunMiniCam;
    public Transform gun;
    public Transform baseGun;
    Vector3 correctPos;
    float time = 0;


    private void Start()
    {
        target = null;
        target1 = null;
        correctPos = gun.up;
    }


    /*    private void OnTriggerStay2D(Collider2D collision)
        {
            Test2 test = collision.GetComponent<Test2>();
            if (target == null)
            {
                if (test != null)
                {

                    if (test.CompareTag(Constant.TAG_AirSpace1))
                    {
                        //target = collision.gameObject.GetComponent<ShootTarget>();                   
                        target = test;
                    }
                }
                else
                {
                    target = null;
                }
            }

            if (target != null && time >= timeColdown && target.gameObject.activeSelf)
            {
                Shot(target);
                time = 0;
            }

        }*/

    /* private void Update()
     {
         time += Time.deltaTime;
         if (target != null)
         {
             Vector3 directionGun = (target.transform.position - baseGun.position).normalized;
             //gun.up = directionGun;

             DOTween.To(() => gun.up, x => gun.up = x, directionGun, 0.5f);

         }
         else
         {
             //gun.up = correctPos;
             DOTween.To(() => gun.up, x => gun.up = x, correctPos, 0.5f);
         }
     }

     private void OnTriggerExit2D(Collider2D collision)
     {
         Test2 test = collision.GetComponent<Test2>();
         if (target != null)
         {
             if (target == test)
             {
                 target = null;
             }
         }

     }

     public void Shot(Test2 tg)
     {
         if (bullets.Count >= 1)
         {
             bullets[0].gameObject.SetActive(true);
             bullets[0].transform.position = shotPoint.position;
             bullets[0].target = tg.transform;
             bullets[0].OnInit();
             bullets.Remove(bullets[0]);

             Debug.Log(bullets.Count);

         }
         else
         {
             BulletTurret bullet = Instantiate(bulletPrefab);
             bullet.transform.position = shotPoint.position;
             bullet.target = tg.transform;
             bullet.source = this;
         }
     }*/



    private void OnTriggerStay2D(Collider2D collision)
    {
        ShootTarget test = collision.GetComponent<ShootTarget>();
        if (target1 == null)
        {
            if (test != null)
            {

                if (test.CompareTag(Constant.TAG_AirSpace1) && test.host != OwnerCharacter)
                {
                    //target = collision.gameObject.GetComponent<ShootTarget>();                   
                    target1 = test;
                }
            }
            else
            {
                target1 = null;
            }
        }

        if (target1 != null && time >= timeColdown && target1.gameObject.activeSelf)
        {
            Shot(target1);
            time = 0;
        }

    }

    private void Update()
    {
        time += Time.deltaTime;
        if (target1 != null)
        {
            Vector3 directionGun = (target1.transform.position - baseGun.position).normalized;
            //gun.up = directionGun;

            DOTween.To(() => gun.up, x => gun.up = x, directionGun, 0.5f);
            if (GunMiniCam != null)
                DOTween.To(() => GunMiniCam.transform.up, x => GunMiniCam.transform.up = x, directionGun, 0.5f);

        }
        else
        {
            //gun.up = correctPos;
            DOTween.To(() => gun.up, x => gun.up = x, correctPos, 0.5f);
            if (GunMiniCam != null)
                DOTween.To(() => GunMiniCam.transform.up, x => GunMiniCam.transform.up = x, correctPos, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShootTarget test = collision.GetComponent<ShootTarget>();
        if (target1 != null)
        {
            if (target1 == test)
            {
                target1 = null;
            }
        }

    }

    public void Shot(ShootTarget tg)
    {
        if (bullets.Count >= 1)
        {
            bullets[0].gameObject.SetActive(true);
            bullets[0].transform.position = shotPoint.position;
            bullets[0].target = tg.transform;
            bullets[0].OnInit();
            bullets.Remove(bullets[0]);

            Debug.Log(bullets.Count);

        }
        else
        {
            BulletTurret bullet = Instantiate(bulletPrefab);
            bullet.transform.position = shotPoint.position;
            bullet.target = tg.transform;
            bullet.source = this;
        }
    }
}
