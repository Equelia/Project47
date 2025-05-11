using TMPro;

using UnityEngine;

namespace Project47
{
    [AddComponentMenu("Project47/Version/Version")]
    public partial class Version : ExecuteBehaviour
    {
        [Header("Version Labels")]
        [SerializeField()] public TextMeshProUGUI[] labels;

        protected override void Awake()
        {
            for (int i = 0, n = labels.Length; i != n; i++)
                labels[i].text = Application.version;
            base.Awake();
        }
    }
}