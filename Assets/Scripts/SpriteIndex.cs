using UnityEngine;

public class SpriteIndex : MonoBehaviour
{
    public static SpriteIndex instance;

    [SerializeField] private Sprite[] resourceSprites = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public Sprite GetSprite(int index)
    {
        // assumes valid index
        return resourceSprites[index];
    }
}
