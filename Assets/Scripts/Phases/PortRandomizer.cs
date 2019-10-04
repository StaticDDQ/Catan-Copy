using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PortRandomizer : MonoBehaviour
{
    [SerializeField] private List<PortManager> ports = null;
    private int resourceIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
            StartCoroutine(AssignRandomPorts());
    }

    private IEnumerator AssignRandomPorts()
    {
        resourceIndex = 0;
        while(resourceIndex < 5)
        {
            yield return new WaitForSeconds(0.5f);

            int randomIndex = Random.Range(0, ports.Count);

            ports[randomIndex].SetResourceID(resourceIndex);
            resourceIndex++;
            ports.Remove(ports[randomIndex]);

        }

        foreach(PortManager port in ports)
        {
            port.SetResourceID(5);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
