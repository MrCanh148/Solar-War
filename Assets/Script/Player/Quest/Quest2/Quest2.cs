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

    private float initialDistance;
    private int distanceTravelledPercentage;

    private void Start()
    {
        Arrow.SetActive(true);
        player = GameObject.FindWithTag("Player");
        PointerRectTransform = Arrow.GetComponent<RectTransform>();
        ArrowImage = Arrow.GetComponent<Image>();

        Vector3 direction = new Vector3(player.transform.right.x, player.transform.right.y, 0) * DistanceToTarget;
        targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, 0) + direction;
        initialDistance = Vector3.Distance(player.transform.position, targetPosition);
        QuestEventManager.Instance.NotifyQuestStarted();
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

        // Calculate distance travelled percentage
        float currentDistance = Vector3.Distance(player.transform.position, targetPosition);
        distanceTravelledPercentage = Mathf.Max(0, Mathf.RoundToInt(100 * (1 - currentDistance / initialDistance)));
        QuestEventManager.Instance.NotifyQuestProgressUpdated(distanceTravelledPercentage);

        // Done quest
        if (currentDistance <= 2f)
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
