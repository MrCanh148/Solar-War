using UnityEngine;


[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Status")]
public class Status : ScriptableObject
{
    public float acceleration; // Tốc độ tăng tốc
    public float deceleration; // Tốc độ giảm tốc
    public const float gravitationalConstant = 6.674f;
    public float GravitationalConstant => gravitationalConstant;
    public float minimumMergeForce;

    public float coefficientActiveGameObject;  //hệ số khoảng cách gameobject hoạt động
    public float coefficientAttractive;   //hệ số hấp dẫn

    public float timeToCapture;
}