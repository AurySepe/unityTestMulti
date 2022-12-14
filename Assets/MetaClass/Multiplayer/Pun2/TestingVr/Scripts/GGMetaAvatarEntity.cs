using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar2;
using Oculus.Platform;
using Photon.Pun;
using System;

public class GGMetaAvatarEntity : OvrAvatarEntity
{
    private const string logScope = "GGMetaAvatarEntity";
    [SerializeField] int m_avatarToUseInZipFolder = 2;
    PhotonView m_photonView;
    List<byte[]> m_streamedDataList = new List<byte[]>();
    int m_maxBytesToLog = 15;
    [SerializeField] ulong m_instantiationData;
    float m_cycleStartTime = 0;
    float m_intervalToSendData = 0.08f;
    [Tooltip("Adds an underscore between the path and the postfix.")]
    [SerializeField]
    private bool _underscorePostfix = true;
    private bool _highQuality = false;
    private string _overridePostfix = String.Empty;
    private bool HasLocalAvatarConfigured => _assets.Count > 0;
    
    private List<AssetData> _assets = new List<AssetData> { new AssetData { source = SampleAvatarEntity.AssetSource.Zip, path = "0" } };

    private struct AssetData
    {
        public SampleAvatarEntity.AssetSource source;
        public string path;
    }

    protected override void Awake()
    {
        ConfigureAvatarEntity();
        base.Awake();
    }

    private void Start()
    {
        //m_instantiationData = GetUserIdFromPhotonInstantiationData();
        //_userId = m_instantiationData;
        //_userId = 1;

        //StartCoroutine(TryToLoadUser());
        LoadLocalAvatar();
    }

    void ConfigureAvatarEntity()
    {
        m_photonView = GetComponent<PhotonView>();
        if (m_photonView.IsMine)
        {
            SetIsLocal(true);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Default;
            SampleInputManager sampleInputManager = OvrAvatarManager.Instance.gameObject.GetComponent<SampleInputManager>();
            SetBodyTracking(sampleInputManager);
            OvrAvatarLipSyncContext lipSyncInput = GameObject.FindObjectOfType<OvrAvatarLipSyncContext>();
            SetLipSync(lipSyncInput);
            gameObject.name = "MyAvatar";
        }
        else
        {
            SetIsLocal(false);
            _creationInfo.features = Oculus.Avatar2.CAPI.ovrAvatar2EntityFeatures.Preset_Remote;
            gameObject.name = "OtherAvatar";
        }
    }

    IEnumerator TryToLoadUser()
    {
        var hasAvatarRequest = OvrAvatarManager.Instance.UserHasAvatarAsync(_userId);
        while (hasAvatarRequest.IsCompleted == false)
        {
            yield return null;
        }
        LoadUser();
    }

    private void LateUpdate()
    {
        float elapsedTime = Time.time - m_cycleStartTime;
        if (elapsedTime > m_intervalToSendData)
        {
            RecordAndSendStreamDataIfMine();
            m_cycleStartTime = Time.time;
        }
        
    }

    void RecordAndSendStreamDataIfMine()
    {
        if (m_photonView.IsMine)
        {
            byte[] bytes = RecordStreamData(activeStreamLod);
            m_photonView.RPC("RecieveStreamData", RpcTarget.Others, bytes);
        }
    }

    [PunRPC]
    public void RecieveStreamData(byte [] bytes)
    {
        m_streamedDataList.Add(bytes);
    }

    void LogFirstFewBytesOf(byte [] bytes)
    {
        for (int i = 0; i < m_maxBytesToLog; i++)
        {
            string bytesString = Convert.ToString(bytes[i], 2).PadLeft(8, '0');
        }
    }

    private void Update()
    {
        if (m_streamedDataList.Count > 0)
        {
            if (IsLocal == false)
            {
                byte[] firstBytesInList = m_streamedDataList[0];
                if (firstBytesInList != null)
                {
                    ApplyStreamData(firstBytesInList);
                }
                m_streamedDataList.RemoveAt(0);
            }
        }
    }

    ulong GetUserIdFromPhotonInstantiationData()
    {
        PhotonView photonView = GetComponent<PhotonView>();
        object [] instantiationData = photonView.InstantiationData;
        Int64 data_as_int = (Int64)instantiationData[0];
        return Convert.ToUInt64(data_as_int);
    }
    
    private void LoadLocalAvatar()
    {
        if (!HasLocalAvatarConfigured)
        {
            OvrAvatarLog.LogInfo("No local avatar asset configured", logScope, this);
            return;
        }

        // Zip asset paths are relative to the inside of the zip.
        // Zips can be loaded from the OvrAvatarManager at startup or by calling OvrAvatarManager.Instance.AddZipSource
        // Assets can also be loaded individually from Streaming assets
        var path = new string[1];
        foreach (var asset in _assets)
        {
            bool isFromZip = (asset.source == SampleAvatarEntity.AssetSource.Zip);

            string assetPostfix = (_underscorePostfix ? "_" : "")
                                  + OvrAvatarManager.Instance.GetPlatformGLBPostfix(isFromZip)
                                  + OvrAvatarManager.Instance.GetPlatformGLBVersion(_highQuality, isFromZip)
                                  + OvrAvatarManager.Instance.GetPlatformGLBExtension(isFromZip);
            if (!String.IsNullOrEmpty(_overridePostfix))
            {
                assetPostfix = _overridePostfix;
            }

            path[0] = asset.path + assetPostfix;
            if(isFromZip)
            {
                LoadAssetsFromZipSource(path);
            }
            else
            {
                LoadAssetsFromStreamingAssets(path);
            }
        }
    }

}