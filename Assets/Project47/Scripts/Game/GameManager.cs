using UnityEngine;

namespace Project47
{
    [AddComponentMenu("Project47/Game/GameManager")]
	public partial class GameManager : ExecuteBehaviour
	{
        [Header("References")]
        [SerializeField()] public Player player;

        [Header("Singleton")]
        [SerializeField()] public bool singleton;

        public static GameManager instance;

        protected virtual void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void Awake()
        {
            if (singleton)
                instance = this;
			base.Awake();
		}

        protected virtual void OnStart()
        {
            Initialize();
        }
	}
}