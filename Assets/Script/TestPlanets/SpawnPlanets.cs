using UnityEngine;

public class SpawnPlanets : MonoBehaviour
{
    [SerializeField] private GameObject[] Planets;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float FarFromPlayerMin, FarFromPlayerMax;
    [SerializeField] private float destroyDistance;
    private GameObject lastSpawnedPlanet;
    private GameObject currentArrow;

    private void Update()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                SpawnPlanet(i - 1);
        }

        if (currentArrow != null)
            UpdateArrow();
    }

    private void SpawnPlanet(int index)
    {
        float xPos = Random.Range(player.position.x + FarFromPlayerMin, player.position.x + FarFromPlayerMax);
        float yPos = Random.Range(player.position.y + FarFromPlayerMin, player.position.y + FarFromPlayerMax);
        Vector3 placeSpawn = new Vector3(xPos, yPos, 0);

        if (index >= 0 && index < Planets.Length)
        {
            GameObject newPlanet = Instantiate(Planets[index], placeSpawn, Quaternion.identity);
            lastSpawnedPlanet = newPlanet;
            UpdateArrow();
        }
    }

    private void UpdateArrow()
    {
        if (currentArrow != null)
            Destroy(currentArrow);

        if (lastSpawnedPlanet != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 targetPosition = lastSpawnedPlanet.transform.position;
            Vector3 direction = (targetPosition - playerPosition).normalized;

            currentArrow = Instantiate(arrowPrefab, playerPosition, Quaternion.identity);

            currentArrow.transform.up = direction;

            float distance = Vector3.Distance(playerPosition, targetPosition);
            if (distance < destroyDistance)
                Destroy(currentArrow);
        }
    }
}
