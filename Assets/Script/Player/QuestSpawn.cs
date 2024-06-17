using UnityEngine;
using UnityEngine.UI;

public class QuestSpawn : MonoBehaviour
{
    [SerializeField] private Sprite arrow, cross;
    [SerializeField] private GameObject Player;
    [SerializeField] private float R = 15;
    [SerializeField] private float borderSize = 100;

    public Vector3 targetPos;
    public RectTransform pointerRectTransform;
    public Image pointerImage;

    private void Update()
    {
        Vector3 targetPosScreenPoint = Camera.main.WorldToScreenPoint(targetPos);
        bool isOffScreen = targetPosScreenPoint.x <= borderSize || targetPosScreenPoint.x > Screen.width - borderSize
            || targetPosScreenPoint.y <= borderSize || targetPosScreenPoint.y > Screen.height - borderSize;

        if (isOffScreen)
        {
            RotatePointerTowardTargetPos();
            pointerImage.sprite = arrow;

            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(Player.transform.position);
            Vector3 direction = (targetPosScreenPoint - playerScreenPos).normalized;
            Vector3 pointerScreenPos = playerScreenPos + direction * R;

            pointerScreenPos.x = Mathf.Clamp(pointerScreenPos.x, borderSize, Screen.width - borderSize);
            pointerScreenPos.y = Mathf.Clamp(pointerScreenPos.y, borderSize, Screen.height - borderSize);

            pointerRectTransform.position = pointerScreenPos;
        }
        else
        {
            pointerImage.sprite = cross;
            pointerRectTransform.position = targetPosScreenPoint;
        }
    }

    private void RotatePointerTowardTargetPos()
    {
        Vector3 toPos = targetPos;
        Vector3 fromPos = Camera.main.transform.position;
        fromPos.z = 0f;
        Vector3 dir = (toPos - fromPos).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
