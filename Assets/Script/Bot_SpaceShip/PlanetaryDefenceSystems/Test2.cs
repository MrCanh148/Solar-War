
using UnityEngine;

public class Test2 : MonoBehaviour
{
    int hp;
    // Start is called before the first frame update
    private void Start()
    {
        hp = 5;
    }

    public void OnHit()
    {
        //hp--;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
