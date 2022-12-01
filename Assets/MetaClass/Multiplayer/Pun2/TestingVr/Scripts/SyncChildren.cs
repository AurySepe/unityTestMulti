using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class SyncChildren : MonoBehaviour,IPunObservable
{
    // Start is called before the first frame update

    private Transform[] childrens;
   

    private bool ischildrenSet = false;
    


    private void Update()
    {
        if (!ischildrenSet && transform.childCount != 10)
        {
            childrens = new Transform[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
            {
                childrens[i] = child;
                i++;
            }
        
            ischildrenSet = true;
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (childrens == null)
        {
            return;
        }
        
        if (stream.IsWriting) {
            print("i am writing");
            print(childrens.Length);
            for (int i = 0; i < childrens.Length; i++) {
                print(childrens[i].name);
                
                if (childrens[i] != null) {
                    stream.SendNext(childrens[i].localPosition);
                    stream.SendNext(childrens[i].localRotation);
                    stream.SendNext(childrens[i].localScale);
                    
                }
            }
        } else {
            for (int i = 0; i < childrens.Length; i++) {
                if (childrens[i] != null) {
                    childrens[i].localPosition = (Vector3)stream.ReceiveNext();
                    childrens[i].localRotation = (Quaternion)stream.ReceiveNext();
                    childrens[i].localScale = (Vector3)stream.ReceiveNext();
                    
                }
            }
        }
    }
}


