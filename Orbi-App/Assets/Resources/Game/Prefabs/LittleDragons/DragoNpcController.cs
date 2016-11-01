using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using GameController;

public class DragoNpcController : ThirdPersonCharacter {

    private DragonController controller;

    // Use this for initialization
    public override void Start () {
        base.Start();
        controller = GetComponent<DragonController>();
    }

    public override void Move(Vector3 move, bool crouch, bool jump)
    {
        controller.CameraMove(move);
        //m_Animator.SetBool(HashIDsDragons.groundedHash, m_IsGrounded);
        //m_Animator.SetFloat(HashIDsDragons.horizontalHash, m_ForwardAmount);
        //controller.for = m_ForwardAmount;
       // m_Animator.SetBool("Walk", (m_ForwardAmount > 0) && (m_ForwardAmount <= 0.5f));
       // m_Animator.SetBool("Run", (m_ForwardAmount > 0.5f));

    }
}
