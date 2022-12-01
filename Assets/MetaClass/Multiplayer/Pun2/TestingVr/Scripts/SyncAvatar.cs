using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Avatar2;
using Photon.Pun;
using UnityEngine;

public class SyncAvatar : MonoBehaviour
{
    [SerializeField]
    private SampleAvatarEntity SampleAvatarEntity;

    

    private PhotonView photonView;
    
    // Start is called before the first frame update
   

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            SampleAvatarEntity.SetBodyTracking(null);
            SampleAvatarEntity.SetLipSync(null);
        }
        else
        {
            OvrAvatarBodyTrackingBehavior trackingBehavior =
                GameObject.FindWithTag("AvatarManager").GetComponent<OvrAvatarBodyTrackingBehavior>();
            
            OvrAvatarLipSyncBehavior syncBehavior = GameObject.FindWithTag("LipSync").GetComponent<OvrAvatarLipSyncBehavior>();
            
            SampleAvatarEntity.SetBodyTracking(trackingBehavior);
            SampleAvatarEntity.SetLipSync(syncBehavior);
            
        }
        
    }

    // Update is called once per frame
}
