using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Project47
{
    [AddComponentMenu("Project47/GameLoader/GameLoader")]
	public partial class GameLoader : ExecuteBehaviour, IInitializable
	{
#if UNITY_EDITOR
		[Header("Building Properties")]
		[SerializeField()] public BuildTarget bundleBuildTarget;
#endif

		[Header("Bundle Properties")]
		[SerializeField()] public string sceneBundlePath;
		[SerializeField()] public string sceneBundleName;

		[Header("Singleton")]
		[SerializeField()] public bool initStart;
		[SerializeField()] public bool singleton;

		public static byte[] saltBytes = new byte[] {
			0x20, 0x06, 0x26
		};

		public static GameLoader instance;

#if UNITY_EDITOR
		[ContextMenu("Bundle Builder/Build Scene Bundle")]
		protected virtual void BuildSceneBundle()
		{
			var bundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles", sceneBundlePath);

			if (Directory.Exists(bundlePath))
			{
                var files = Directory.GetFiles(bundlePath, "*.*", SearchOption.AllDirectories);

                for (int i = 0, n = files.Length; i != n; i++)
                    File.Delete(files[i]);

                var directories = Directory.GetDirectories(bundlePath, "*.*", SearchOption.AllDirectories);

                for (int i = 0, n = directories.Length; i != n; i++)
                    Directory.Delete(directories[i]);
			}
			else
				Directory.CreateDirectory(bundlePath);

			BuildPipeline.BuildAssetBundles(bundlePath, BuildAssetBundleOptions.None, bundleBuildTarget);

			foreach (var file in Directory.GetFiles(bundlePath))
			{
				if (!file.EndsWith(".meta") && !file.EndsWith(".manifest"))
				{
					var bytes = File.ReadAllBytes(file);

					for (int i = 0, n = bytes.Length; i != n; i++)
					{
						for (int k = 0, kn = saltBytes.Length; k != kn; k++)
							bytes[i] = (byte) (bytes[i] ^ saltBytes[k]);
					}

					File.WriteAllBytes(file, bytes);
				}
			}
		}
#endif

		protected virtual AssetBundle LoadSceneBundle()
		{
			var bundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles", sceneBundlePath, sceneBundleName);

			if (File.Exists(bundlePath))
			{
				var bytes = File.ReadAllBytes(bundlePath);

				for (int i = 0, n = bytes.Length; i != n; i++)
				{
					for (int k = 0, kn = saltBytes.Length; k != kn; k++)
						bytes[i] = (byte) (bytes[i] ^ saltBytes[k]);
				}

				var bundle = AssetBundle.LoadFromMemory(bytes);

				if (!bundle.isStreamedSceneAssetBundle)
					bundle.LoadAllAssets();

				return bundle;
			}

			return null;
		}

        public virtual void InitStart()
        {
			var sceneBundle = LoadSceneBundle();
			if (sceneBundle != null)
			{
				var scenes = sceneBundle.GetAllScenePaths();

				if (scenes != null && scenes.Length == 1)
					SceneManager.LoadScene(scenes[0]);
			}
        }

		protected override void Awake()
		{
			if (singleton)
				instance = this;
			base.Awake();
		}

        protected virtual void OnStart()
        {
            if (initStart)
                InitStart();
        }
	}
}