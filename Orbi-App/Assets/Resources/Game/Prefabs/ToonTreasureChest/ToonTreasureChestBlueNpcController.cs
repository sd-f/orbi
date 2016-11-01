using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class ToonTreasureChestBlueNpcController : ThirdPersonCharacter {

    float m_restTime = 0f;

    public override void Start()
    {
        base.Start();
    }

    public override void UpdateAnimator(Vector3 move)
    {

        m_Animator.SetBool("Walk", (m_ForwardAmount > 0) && (m_ForwardAmount <= 0.5f));
        m_Animator.SetBool("Run", (m_ForwardAmount > 0.5f));
        m_Animator.SetBool("Rest", (m_ForwardAmount == 0) && (m_restTime > 0));
        if ((m_ForwardAmount > 0.5f) && (m_restTime <= 3f))
            m_restTime = m_restTime + Time.fixedDeltaTime;
        else if (m_restTime > 0f)
            m_restTime = m_restTime - Time.fixedDeltaTime;
 
    }
}
