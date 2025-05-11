using System;

using UnityEngine;

namespace Project47
{
    [AddComponentMenu("Project47/Execution/ExecuteBehaviour")]
    public partial class ExecuteBehaviour : MonoBehaviour
    {
        [NonSerialized()] [HideInInspector()] public string loadingState;

        protected virtual void Awake()
        {
            if (Application.isPlaying)
                Executor.RegisterExecuteBehaviourSingleton(this);
        }

        protected virtual void OnDestroy()
        {
            if (Application.isPlaying)
                Executor.UnregisterExecuteBehaviourSingleton(this);
        }
    }
}