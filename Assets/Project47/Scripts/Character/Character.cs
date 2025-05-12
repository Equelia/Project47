using UnityEngine;

namespace Project47
{
    [AddComponentMenu("Project47/Character/Character")]
	public partial class Character : ExecuteBehaviour
	{
		[Header("References")]
		[SerializeField()] public Animator characterAnimator;
		[SerializeField()] public Transform characterTransform;
		[SerializeField()] public Rigidbody characterRigibody;

		[Header("References - Rig")]
		[SerializeField()] public Transform rootAnchor;

		[Header("Settings")]
		[SerializeField()] public CharacterSettings characterSettings;

		public virtual bool IsGrounded()
		{
			var hits = Physics.RaycastAll(rootAnchor.position, Vector3.down, 0.05f);

			if (hits.Length != 0)
			{
				foreach (var hit in hits)
				{
					if (hit.transform.gameObject.TryGetComponent<Ground>(out Ground ground))
						return true;
				}
			}

			return false;
		}

		public virtual void Move(Transform orientation, float dx, float dz, bool shift, float deltaTime)
		{
			var orientationForward = Vector3.Scale(orientation.forward, new Vector3(1.0f, 0.0f, 1.0f)).normalized;
			var orientationRight = orientation.right;
			var movementDirection = orientationRight * dx + orientationForward * dz;

			if (shift)
			{
				characterAnimator.speed = characterSettings.movementSpeedShift;
				characterAnimator.SetBool("Walk", false);
				characterAnimator.SetFloat("WalkH", 0.0f);
				characterAnimator.SetFloat("WalkV", 0.0f);
				characterAnimator.SetBool("Run", true);
				characterAnimator.SetFloat("RunH", dx);
				characterAnimator.SetFloat("RunV", dz);
				characterRigibody.position += movementDirection * (characterSettings.movementSpeedShift * deltaTime);
				return;
			}

			characterAnimator.speed = characterSettings.movementSpeed;
			characterAnimator.SetBool("Run", false);
			characterAnimator.SetFloat("RunH", 0.0f);
			characterAnimator.SetFloat("RunV", 0.0f);
			characterAnimator.SetBool("Walk", true);
			characterAnimator.SetFloat("WalkH", dx);
			characterAnimator.SetFloat("WalkV", dz);
			characterRigibody.position += movementDirection * (characterSettings.movementSpeed * deltaTime);
		}

		public virtual void Idle(Transform orientation)
		{
			var rotationDirection = orientation.forward;
			rotationDirection.y = 0.0f;
			rotationDirection = rotationDirection.normalized;
			characterRigibody.rotation = Quaternion.LookRotation(rotationDirection);

			characterAnimator.speed = 1.0f;
			characterAnimator.SetBool("Walk", false);
			characterAnimator.SetFloat("WalkH", 0.0f);
			characterAnimator.SetFloat("WalkV", 0.0f);
			characterAnimator.SetBool("Run", false);
			characterAnimator.SetFloat("RunH", 0.0f);
			characterAnimator.SetFloat("RunV", 0.0f);
		}

		public virtual void Jump()
		{
			characterRigibody.AddForce(new Vector3(0.0f, characterSettings.jumpForce, 0.0f), ForceMode.VelocityChange);
		}

		public virtual void Rotate(Transform orientation)
		{
			var rotationDirection = orientation.forward;
			rotationDirection.y = 0.0f;
			rotationDirection = rotationDirection.normalized;
			characterRigibody.rotation = Quaternion.LookRotation(rotationDirection);
		}
	}
}