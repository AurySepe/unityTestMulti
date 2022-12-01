using UnityEngine;
using System.Collections;
using Photon.Pun;


public class PlayerAnimatorManager : MonoBehaviourPun
{
    
    #region Private Serialized Fields


    [SerializeField]
    private float directionDampTime = 0.25f;


    #endregion

    #region Private Fields

    private Animator animator;

    #endregion
    
    
    #region MonoBehaviour Callbacks

// Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        if (!animator)
        {
            Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
        }
    }


    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }
        if (!animator)
        {
            return;
        }
        
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
        {
            v = 0;
        }
        animator.SetFloat("Speed", h * h + v * v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        
        
        // deal with Jumping
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
// only allow jumping if we are running.
        if (stateInfo.IsName("Base Layer.Run"))
        {
            // When using trigger parameter
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    #endregion
}