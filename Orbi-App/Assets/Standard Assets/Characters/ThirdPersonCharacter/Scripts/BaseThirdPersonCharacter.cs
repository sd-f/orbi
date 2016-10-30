using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public abstract class BaseThirdPersonCharacter : MonoBehaviour
	{
        public abstract void UpdateAnimator(Vector3 move);
	}
}
