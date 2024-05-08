using UnityEngine;


[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Status")]
public class Status : ScriptableObject
{
    public float acceleration; // Tốc độ tăng tốc
    public float deceleration; // Tốc độ giảm tốc
    public double gravitationalConstant = 6.674f;
}
