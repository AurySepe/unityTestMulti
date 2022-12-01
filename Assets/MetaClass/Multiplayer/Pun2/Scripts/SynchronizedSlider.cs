using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SynchronizedSlider : MonoBehaviour,IPunObservable
{
    // Start is called before the first frame update

    [SerializeField] private Slider _slider;
    
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
            stream.SendNext(_slider.value);
        }
        else
        {
            float value = (float)stream.ReceiveNext();
            _slider.value = value;
        }
    }


    public void getOwnerShip()
    {
        PhotonView photonView = transform.GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            this.transform.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        }

    }
    
}
