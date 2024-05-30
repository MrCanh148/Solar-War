using UnityEngine;
using System.Collections.Generic;

public class LogicSpawnAirSpace : MonoBehaviour
{
    [SerializeField] private GameObject[] shipSpacePrefab;
    [SerializeField] private float spawnInterval = 4f;
    private int maxShips0, maxShips1, maxShips2;

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

        // Cập nhật số lượng tàu tối đa dựa trên số lượng kill
        MaxShipBaseKill();

        if (spawnTimer <= 0f)
        {
            if (character.characterType == CharacterType.LifePlanet)
            {
                SpawnShips();
            }
            spawnTimer = spawnInterval;
        }
    }

    private void MaxShipBaseKill()
    {
        if (character.Kill >= 36)
        {
            maxShips0 = 5;
            maxShips1 = 3;
            maxShips2 = 2;
        }
        else if (character.Kill >= 24)
        {
            maxShips0 = 4;
            maxShips1 = 2;
            maxShips2 = 1;
        }
        else if (character.Kill >= 12)
        {
            maxShips0 = 3;
            maxShips1 = 1;
            maxShips2 = 1;
        }
        else if (character.Kill >= 6)
        {
            maxShips0 = 2;
            maxShips1 = 1;
            maxShips2 = 0;
        }
        else
        {
            maxShips0 = 2;
            maxShips1 = 0;
            maxShips2 = 0;
        }
    }

    private void SpawnShips()
    {
        // Đếm số lượng hiện tại của từng loại tàu
        int currentShips0 = CountShipsOfType(0);
        int currentShips1 = CountShipsOfType(1);
        int currentShips2 = CountShipsOfType(2);

        // Spawn tàu nếu số lượng hiện tại nhỏ hơn maxShips tương ứng
        if (currentShips0 < maxShips0)
        {
            SpawnShip(shipSpacePrefab[0]);
        }

        if (currentShips1 < maxShips1)
        {
            SpawnShip(shipSpacePrefab[1]);
        }

        if (currentShips2 < maxShips2)
        {
            SpawnShip(shipSpacePrefab[2]);
        }
    }

    private int CountShipsOfType(int typeIndex)
    {
        int count = 0;
        foreach (GameObject ship in spawnedShips)
        {
            if (ship != null && ship.name.Contains(shipSpacePrefab[typeIndex].name))
            {
                count++;
            }
        }
        return count;
    }

    private void SpawnShip(GameObject shipPrefab)
    {
        GameObject newShip = Instantiate(shipPrefab, PlaceSpawn.position, PlaceSpawn.rotation);
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
            shootTarget.SetIgnoredTargets(spawnedShips, character);
        }
    }
}
