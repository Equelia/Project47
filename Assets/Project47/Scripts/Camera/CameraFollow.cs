using System;

using UnityEngine;

namespace Project47
{
    [AddComponentMenu("Project47/Camera/CameraFollow")]
	public partial class CameraFollow : ExecuteBehaviour
	{
		[Header("References")]
		[SerializeField()] public Transform followPositionTarget;
		[SerializeField()] public Transform followRotationTarget;

		[NonSerialized()] [HideInInspector()] public Vector3 followPositionOffset;
		[NonSerialized()] [HideInInspector()] public Vector3 followRotationOffset;

		public virtual void Rotate(float dx, float dy)
		{
			followRotationOffset += new Vector3(-dy, dx, 0.0f);
		}

		protected override void Awake()
		{
			followPositionOffset = transform.position - followPositionTarget.position;
			followRotationOffset = transform.eulerAngles - followRotationTarget.eulerAngles;
			base.Awake();
		}

		protected virtual void LateUpdate()
		{
			transform.position = followPositionOffset + followPositionTarget.position;
			transform.eulerAngles = followRotationOffset;
		}
	}
}