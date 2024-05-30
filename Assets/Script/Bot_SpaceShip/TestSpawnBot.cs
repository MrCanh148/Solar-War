using UnityEngine;
using System.Collections.Generic;

public class TestSpawnBot : MonoBehaviour
{
    [SerializeField] private GameObject[] shipSpacePrefab;
    [SerializeField] private float spawnInterval = 4f;
    [SerializeField] private int maxShips = 4;

    private float spawnTimer;
    private Transform PlaceSpawn;
    private Character character;
    private List<GameObject> spawnedShips = new List<GameObject>();

    private void Start()
    {
        PlaceSpawn = GetComponent<Transform>();
        character = GetComponent<Character>();
        if (PlaceSpawn != null)
        {
            spawnedShips.Add(PlaceSpawn.gameObject);
        }
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        // Xóa các tàu đã bị hủy khỏi danh sách
        spawnedShips.RemoveAll(ship => ship == null);

        if (spawnTimer <= 0f)
        {
            if (spawnedShips.Count < maxShips && character.characterType == CharacterType.LifePlanet)
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
