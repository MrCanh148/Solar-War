using UnityEngine;

public class ScreenOrientationManager : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Cho phép màn hình tự động xoay dựa trên cảm biến của thiết bị
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    void Update()
    {
        //Chuyển sang LandscapeRight khi nhấn phím R
        if (Input.GetKeyDown(KeyCode.R))
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }
    }
}
