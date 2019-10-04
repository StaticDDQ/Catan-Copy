using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private GameObject resourcePanel = null;

    public void SelectResource()
    {
        resourcePanel.SetActive(true);
    }

    public void AssignImage(int imgIndex)
    {
        GetComponent<Image>().sprite = SpriteIndex.instance.GetSprite(imgIndex);
        resourcePanel.SetActive(false);
    }
}
