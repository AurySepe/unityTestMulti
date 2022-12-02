using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;

    public PhotonView PhotonView;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonView.IsMine)
        {
            transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);

        }

    }
}
