using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SyncSphere : MonoBehaviour, IPunObservable
{

    public Transform sphereTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(sphereTransform.localPosition);
        }
        else
        {
            Vector3 localPosition = (Vector3)stream.ReceiveNext();
            sphereTransform.localPosition = localPosition;
        }
    }
}
