using UnityEngine;

namespace Project47
{
    [AddComponentMenu("Project47/Player/PlayerInput")]
	public partial class PlayerInput : ExecuteBehaviour
	{
		[Header("References")]
		[SerializeField()] public Player player;
		[SerializeField()] public Character character;
		[SerializeField()] public Transform orientation;
		[SerializeField()] public Camera renderingCamera;
		[SerializeField()] public CameraFollow cameraFollow;

		[Header("Properties - Input (Keyboard)")]
		[SerializeField()] public string inputMoveHorizontal;
		[SerializeField()] public string inputMoveVertical;
		[SerializeField()] public float inputMoveSensitivity;

		[Header("Properties - Input (Mouse)")]
		[SerializeField()] public string inputMouseX;
		[SerializeField()] public string inputMouseY;
		[SerializeField()] public float inputMouseSensitivity;

		protected virtual void MovementProcess(float deltaTime)
		{
			var moveSensitivity = inputMoveSensitivity * deltaTime;

			var h = Mathf.Clamp(Input.GetAxis(inputMoveHorizontal) * moveSensitivity, -1.0f, 1.0f);
			var v = Mathf.Clamp(Input.GetAxis(inputMoveVertical) * moveSensitivity, -1.0f, 1.0f);

			if (!Mathf.Approximately(h, 0.0f) || !Mathf.Approximately(v, 0.0f))
			{
				character.Move(orientation, h, v, Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
				return;
			}

			character.Idle(orientation);
		}

		protected virtual void RotationProcess(float deltaTime)
		{
			var mouseSensitivity = inputMouseSensitivity * deltaTime;

			var mouseX = Input.GetAxis(inputMouseX) * mouseSensitivity;
			var mouseY = Input.GetAxis(inputMouseY) * mouseSensitivity;

			cameraFollow.Rotate(mouseX, mouseY);
			character.Rotate(orientation);
		}

		protected virtual void Update()
		{
			MovementProcess(Time.deltaTime);
			RotationProcess(Time.deltaTime);
		}
	}
}