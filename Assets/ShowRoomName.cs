using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ShowRoomName : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] 
    private TextMeshProUGUI text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            String s = "";
            
            
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                s += player.NickName + "\n";
            }

            text.text = s;
        }
        else
        {
            text.text = "No current Room";
        }
    }
}
