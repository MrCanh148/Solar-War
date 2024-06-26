using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteFprmAtlas : MonoBehaviour
{
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] string spriteName;

    private void Start()
    {
        GetComponent<Image>().sprite = atlas.GetSprite(spriteName);
    }

}
