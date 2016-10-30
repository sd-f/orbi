using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using GameController;

public class ToonTreasureChestBlueNpcController : ThirdPersonCharacter {

    private AICharacterControl ai;
    private ThirdPersonCharacter thirdPersonController;
    private GameObject target;
    private Vector3 targetVector = new Vector3();
    private static float MOVE_RADIUS = 10f;
    float m_restTime = 0f;

    // Use this for initialization
    public override void Start () {
        base.Start();
        target = new GameObject("target_" + gameObject.name);
        target.transform.SetParent(transform.parent);
        ai = GetComponent<AICharacterControl>();
        thirdPersonController = GetComponent<ThirdPersonCharacter>();
        ai.SetTarget(target.transform);
        InvokeRepeating("UpdateTarget", 2, 2);
    }
	

    void UpdateTarget()
    {
        Vector3 newTarget = Game.Instance.GetWorld().GetTerrainService().ClampPosition(targetVector);

        // todo check nav mesh
        /*
        NavMeshHit hit;
        if (NavMesh.SamplePosition(newTarget, out hit, MOVE_RADIUS, 1))
        {
            Vector3 finalPosition = Game.Instance.GetWorld().GetTerrainService().ClampPosition(hit.position);
            target.transform.position = finalPosition;
        }
        */
        target.transform.position = newTarget;
    }

    void OnDestroy()
    {
        CancelInvoke();
    }


    public void SetTarget(Vector3 newTargetPosition)
    {
        targetVector = newTargetPosition;
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
