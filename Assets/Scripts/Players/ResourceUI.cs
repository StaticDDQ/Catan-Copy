using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private GameObject resourcePanel = null;
    [SerializeField] private Sprite[] resourceSprites = null;

    public void SelectResource()
    {
        resourcePanel.SetActive(true);
    }

    public void AssignImage(int imgIndex)
    {
        GetComponent<Image>().sprite = resourceSprites[imgIndex];
        resourcePanel.SetActive(false);
    }
}
