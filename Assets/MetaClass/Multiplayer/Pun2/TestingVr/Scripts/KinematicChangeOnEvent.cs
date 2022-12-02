using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class KinematicChangeOnEvent : MonoBehaviour
{
    private Rigidbody _object;

    private PhotonView _photonView;
    // Start is called before the first frame update
    void Start()
    {
        _object = GetComponent<Rigidbody>();
        _photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleKinematicHandeler()
    {
        if (_photonView.IsMine)
        {
            _photonView.RPC("ToggleKinematic", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void ToggleKinematic()
    {
        if (_object)
        {
            _object.isKinematic = !_object.isKinematic;
        }
    }
}
