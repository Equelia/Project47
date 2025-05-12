using UnityEngine;

namespace Project47
{
    [AddComponentMenu("Project47/Player/Player")]
	public partial class Player : ExecuteBehaviour
	{
		[Header("References")]
		[SerializeField()] public GameManager gameManager;
		[SerializeField()] public PlayerInput playerInput;
		[SerializeField()] public Character character;
		[SerializeField()] public Camera renderingCamera;
		[SerializeField()] public CameraFollow cameraFollow;

        [Header("Singleton")]
        [SerializeField()] public bool singleton;

        public static Player instance;

        protected override void Awake()
        {
            if (singleton)
                instance = this;
			base.Awake();
		}
	}
}