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

        _object.isKinematic = false;
    }
    

    public void ToggleKinematicHandeler()
    {
        StartCoroutine(ToggleKinematicHandelerC());
    }

    IEnumerator ToggleKinematicHandelerC()
    {
        yield return WaitForMine();
        _photonView.RPC("ToggleKinematic", RpcTarget.Others);
    }

    IEnumerator WaitForMine()
    {
        while (!_photonView.IsMine)
        {
            yield return null;
        }
    }

    [PunRPC]
    public void ToggleKinematic()
    {
        if (_object != null)
        {
            print("prima " + _object.isKinematic);
            _object.isKinematic = !_object.isKinematic;
            print("dopo " + _object.isKinematic);
        }
    }
}
