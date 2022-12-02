using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ChangeOwner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PhotonView _photonView;

    public void GetOwnerShip()
    {
        if (!_photonView.IsMine)
        {
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }
    }
}
