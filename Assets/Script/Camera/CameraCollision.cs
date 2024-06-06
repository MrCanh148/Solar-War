using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Camera _camera;
    [SerializeField] List<CameraWall> cameraWalls;
    SpawnPlanets spawnPlanets;

    private void Start()
    {
        //spawnPlanets = SpawnPlanets.instance;
        _camera = Camera.main;
        float boxY = _camera.orthographicSize * GameManager.instance.status.coefficientActiveGameObject;
        float boxX = boxY * (float)_camera.pixelWidth / _camera.pixelHeight;
        if (cameraWalls.Count >= 4)
        {
            cameraWalls[0].transform.position = new Vector2(_camera.transform.position.x, _camera.transform.position.y + boxY * 2);
            cameraWalls[0].transform.localScale = new Vector2(boxX * 4f, 0.1f);   // Top

            cameraWalls[1].transform.position = new Vector2(_camera.transform.position.x, _camera.transform.position.y - boxY * 2);
            cameraWalls[1].transform.localScale = new Vector2(boxX * 4f, 0.1f);   // Bot

            cameraWalls[2].transform.position = new Vector2(_camera.transform.position.x + boxX * 2, _camera.transform.position.y);
            cameraWalls[2].transform.localScale = new Vector2(0.1f, boxY * 4f);   // Left

            cameraWalls[3].transform.position = new Vector2(_camera.transform.position.x - boxX * 2, _camera.transform.position.y);
            cameraWalls[3].transform.localScale = new Vector2(0.1f, boxY * 4f);   // Right
        }
    }



}
