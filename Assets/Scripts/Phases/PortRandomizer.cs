using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PortRandomizer : MonoBehaviour
{
    [SerializeField] private List<PortManager> ports = null;
    private int resourceIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
            AssignRandomPorts();
    }

    private void AssignRandomPorts()
    {
        while(resourceIndex != 5)
        {
            int randomIndex = Random.Range(0, ports.Count);

            ports[randomIndex].photonView.RPC("SetResourceID", RpcTarget.All, resourceIndex);
            resourceIndex++;
            ports.Remove(ports[randomIndex]);
        }

        foreach(PortManager port in ports)
        {
            port.photonView.RPC("SetResourceID", RpcTarget.All, 5);
        }
    }
}
