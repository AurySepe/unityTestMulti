using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SyncWithPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    private Transform playerTransform;
    private PhotonView photonView;

    
    
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;

        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            MapPosition(transform, playerTransform);
        }
    }

    private void MapPosition(Transform transform1, Transform transform2)
    {
        transform1.position = transform2.position;
        transform1.rotation = transform2.rotation;
    }
}
