using UnityEngine;
using System.Collections.Generic;

public class TestSpawnBot : MonoBehaviour
{
    [SerializeField] private GameObject[] shipSpacePrefab;
    public Transform PlaceSpawn;

    private List<GameObject> spawnedShips = new List<GameObject>();

    [SerializeField] private float spawnInterval = 4f;
    private float spawnTimer;
    [SerializeField] private int maxShips = 4;

    private void Start()
    {
        // Thêm Planet vào danh sách spawnedShips
        if (PlaceSpawn != null)
        {
            spawnedShips.Add(PlaceSpawn.gameObject);
        }
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            if (spawnedShips.Count < maxShips)
            {
                SpawnShip();
            }
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnShip()
    {
        GameObject newShip = Instantiate(shipSpacePrefab[0], PlaceSpawn.position, PlaceSpawn.rotation);
        spawnedShips.Add(newShip);

        // Thiết lập centerPoint cho RandomMovement của tàu mới
        RandomMovement randomMovement = newShip.GetComponent<RandomMovement>();
        if (randomMovement != null)
        {
            randomMovement.SetCenterPoint(PlaceSpawn);
        }

        // Thiết lập spawnedShips cho ShootTarget của tàu mới
        ShootTarget shootTarget = newShip.GetComponent<ShootTarget>();
        if (shootTarget != null)
        {
            shootTarget.SetIgnoredTargets(spawnedShips);
        }
    }
}
