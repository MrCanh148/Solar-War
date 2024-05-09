using TMPro;
using UnityEngine;

public class ShowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Guide;
    [SerializeField] private TextMeshProUGUI MassText;
    [SerializeField] private Player player;

    const string Guide1 = "Press <Space> to see more";
    const string Guide2 = "Asteroid :1  SmallPlanet :2  LifePlanet :3  GasGaintPlanet :4  SmallStar :5  MediumStar :6  BigStar :7  NeutronStar :8  BlackHole :9";
    private int countPush = 0;

    private void Update()
    {
        MassText.text = player.rb.mass.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
            countPush++;
        
        if (countPush % 2 == 0)
            Guide.text = Guide1;
        else
            Guide.text = Guide2;
    }
}
