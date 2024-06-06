
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public Character owner;
    int hp;
    // Start is called before the first frame update
    private void Start()
    {
        hp = 5;
        Debug.Log(owner.tf.localScale.x);
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
