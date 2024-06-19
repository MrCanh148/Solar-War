using UnityEngine;
using UnityEngine.UI;

public class Quest2 : MonoBehaviour
{
    [SerializeField] private GameObject Arrow;
    [SerializeField] private float DistanceToTarget = 100f;
    [SerializeField] private Sprite arrow, cross;
    [SerializeField] private float R = 15;
    [SerializeField] private float borderSize = 100;
    [SerializeField] private float moveSpeed = 5f;

    private Vector3 targetPosition;
    private GameObject player;
    private RectTransform PointerRectTransform;
    private Image ArrowImage;

    private void Start()
    {
        Arrow.SetActive(true);
        player = GameObject.FindWithTag("Player");
        PointerRectTransform = Arrow.GetComponent<RectTransform>();
        ArrowImage = Arrow.GetComponent<Image>();

        if (player != null)
        {
            Vector3 direction = new Vector3(player.transform.right.x, player.transform.right.y, 0) * DistanceToTarget;
            targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, 0) + direction;
            QuestEventManager.Instance.NotifyQuestStarted();
        }
        else
            Debug.Log("==== No have Player to do Quest 2 ======");
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 targetPosScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPosScreenPoint.x <= borderSize || targetPosScreenPoint.x > Screen.width - borderSize
            || targetPosScreenPoint.y <= borderSize || targetPosScreenPoint.y > Screen.height - borderSize;

        if (isOffScreen)
        {
            RotatePointerTowardTargetPos(PointerRectTransform, targetPosition);
            ArrowImage.sprite = arrow;

            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(player.transform.position);
            Vector3 direction = (targetPosScreenPoint - playerScreenPos).normalized;
            Vector3 pointerScreenPos = playerScreenPos + direction * R;

            pointerScreenPos.x = Mathf.Clamp(pointerScreenPos.x, borderSize, Screen.width - borderSize);
            pointerScreenPos.y = Mathf.Clamp(pointerScreenPos.y, borderSize, Screen.height - borderSize);

            PointerRectTransform.position = Vector3.Lerp(PointerRectTransform.position, pointerScreenPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            ArrowImage.sprite = cross;
            PointerRectTransform.position = Vector3.Lerp(PointerRectTransform.position, targetPosScreenPoint, Time.deltaTime * moveSpeed);
        }

        // Done quest
        float distanceToTarget = Vector3.Distance(player.transform.position, targetPosition);
        if (distanceToTarget <= 2f)
        {
            QuestEventManager.Instance.NotifyQuestCompleted();
            Destroy(gameObject);
        }
    }

    private void RotatePointerTowardTargetPos(RectTransform pointerRectTransform, Vector3 targetPos)
    {
        Vector3 toPos = targetPos;
        Vector3 fromPos = Camera.main.transform.position;
        fromPos.z = 0f;
        Vector3 dir = (toPos - fromPos).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
