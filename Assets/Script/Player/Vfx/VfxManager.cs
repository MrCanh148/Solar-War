using UnityEngine;

public class VfxManager : MonoBehaviour, IVfx
{
    [Header("0:AlienDestroy - 1:PlanetDestroy - 2:PlanetHit")]
    [SerializeField] private GameObject[] listVfx;

    public static VfxManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void AlienDestroyVfx(Vector3 position, Quaternion rotation)
    {
        Instantiate(listVfx[0], position, rotation);
    }

    public void PlanetDestroyVfx(Vector3 position, Quaternion rotation)
    {
        Instantiate(listVfx[1], position, rotation);
    }

    public void PlanetHitVfx(Vector3 position, Quaternion rotation)
    {
        Instantiate(listVfx[2], position, rotation);
    }
}
