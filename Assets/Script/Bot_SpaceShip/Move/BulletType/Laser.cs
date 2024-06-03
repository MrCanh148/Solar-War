using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float TimeLimit = 0.5f;
    [SerializeField] private float damage = 5;
    private float timeAppear = 0f;
    private GameObject target;
    private Transform firePoint;
    [HideInInspector] public Character characterOwner;
    private bool isShoot = false;

    private void Start()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        timeAppear += Time.deltaTime;
        if (timeAppear > TimeLimit)
            Destroy(gameObject);

        DrawLaser();
    }

    private void DrawLaser()
    {
        if (target != null && firePoint != null && !isShoot)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, target.transform.position);

            RaycastHit2D[] hits = Physics2D.RaycastAll(firePoint.position, target.transform.position - firePoint.position, Mathf.Infinity);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Planet")
                    {
                        Character targetCharacter = hit.collider.gameObject.GetComponent<Character>();
                        Rigidbody2D rbTarget = hit.collider.gameObject.GetComponent<Rigidbody2D>();

                        if (targetCharacter == null) continue;

                        if (targetCharacter.generalityType == GeneralityType.Asteroid || targetCharacter.generalityType == GeneralityType.Planet)
                        {
                            if (targetCharacter != characterOwner)
                            {
                                rbTarget.mass -= damage;
                                if (rbTarget.mass < 1 || (targetCharacter.characterType == CharacterType.SmallPlanet && rbTarget.mass < 20))
                                {
                                    characterOwner.Kill++;

                                    if (hit.collider.gameObject.tag == "Player")
                                        ReSpawnPlayer.Instance.ResPlayer();
                                    else
                                    {
                                        if (targetCharacter.host != null)
                                        {
                                            targetCharacter.host.satellites.Remove(targetCharacter);
                                        }
                                        Destroy(hit.collider.gameObject);
                                    }
                                }
                            }
                        }

                        isShoot = true;
                        break;
                    }

                    if (hit.collider.gameObject.tag == "AirSpace1")
                    {
                        ShootTarget enermy = hit.collider.gameObject.GetComponent<ShootTarget>();
                        if (enermy != null)
                        {
                            enermy.heart -= damage;
                            if (enermy.heart < 0)
                                characterOwner.Kill++;
                        }

                        isShoot = true;
                        break;
                    }
                }
            }
        }
    }

    public void SetTarget(GameObject newTarget, Transform newFirePoint)
    {
        target = newTarget;
        firePoint = newFirePoint;
    }
}
