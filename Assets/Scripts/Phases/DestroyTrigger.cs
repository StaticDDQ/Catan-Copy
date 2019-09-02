using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private bool mainBlock = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Block") && !other.GetComponent<DestroyTrigger>().IsMainBlock())
        {
            Destroy(other.gameObject);
            mainBlock = true;
        }
    }

    public bool IsMainBlock()
    {
        return mainBlock;
    }
}
