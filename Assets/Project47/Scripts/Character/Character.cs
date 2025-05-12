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

		[Header("Settings")]
		[SerializeField()] public CharacterSettings characterSettings;

		public virtual void Move(Transform orientation, float dx, float dz, bool shift)
		{
			var orientationForward = Vector3.Scale(orientation.forward, new Vector3(1.0f, 0.0f, 1.0f)).normalized;
			var orientationRight = orientation.right;

			var movementDirection = orientationRight * dx + orientationForward * dz;

			if (shift)
			{
				characterRigibody.position += movementDirection * characterSettings.movementSpeedShift;
				return;
			}

			characterRigibody.position += movementDirection * characterSettings.movementSpeed;
		}

		public virtual void Idle(Transform orientation)
		{
			var rotationDirection = orientation.forward;
			rotationDirection.y = 0.0f;
			rotationDirection = rotationDirection.normalized;
			characterRigibody.rotation = Quaternion.LookRotation(rotationDirection);
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