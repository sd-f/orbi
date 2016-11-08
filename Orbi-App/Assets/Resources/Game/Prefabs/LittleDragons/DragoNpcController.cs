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

        Invoke("RanodmIdle", 0.5f);
    }

    void RanodmIdle()
    {
        if (m_ForwardAmount <= 0f)
        {
            Invoke("RanodmIdle", Random.Range(15f, 90f));
        }
    }


    public override void Move(Vector3 move, bool crouch, bool jump)
    {

        controller.CameraMove(move);
        controller.Jump = m_Rigidbody.velocity.y > 0;
    }
}
