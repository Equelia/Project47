using TMPro;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Project47
{
    [AddComponentMenu("Project47/Execution/Executor")]
    public partial class Executor : MonoBehaviour
    {
        [Header("Loading State (UI)")]
        [SerializeField()] public Slider loadingScreenSlider;
        [SerializeField()] public TextMeshProUGUI loadingScreenAmountLabel;
        [SerializeField()] public int loadingScreenAmountZeroDigits;
        [SerializeField()] public TextMeshProUGUI loadingScreenProcessLabel;

        [Header("Loading Callback")]
        [SerializeField()] public UnityEvent onLoadCallback;
        [SerializeField()] public float onLoadCallbackDelay;

        [Header("Singleton")]
        [SerializeField()] public bool singleton;

        [NonSerialized()] [HideInInspector()] public float loadingAmount;
        [NonSerialized()] [HideInInspector()] public string loadingState;

        [NonSerialized()] [HideInInspector()] public List<ExecuteBehaviour> executeBehaviours;
        [NonSerialized()] [HideInInspector()] public List<MethodInfo> methodInfoStart;
        [NonSerialized()] [HideInInspector()] public List<ExecuteBehaviour> methodInfoStartBehaviours;
        [NonSerialized()] [HideInInspector()] public List<MethodInfo> methodInfoStartDeferred;
        [NonSerialized()] [HideInInspector()] public List<ExecuteBehaviour> methodInfoStartDeferredBehaviours;
        [NonSerialized()] [HideInInspector()] public List<MethodInfo> methodInfoLateUpdate;
        [NonSerialized()] [HideInInspector()] public List<ExecuteBehaviour> methodInfoLateUpdateBehaviours;
        [NonSerialized()] [HideInInspector()] public List<MethodInfo> methodInfoUpdate;
        [NonSerialized()] [HideInInspector()] public List<ExecuteBehaviour> methodInfoUpdateBehaviours;
        [NonSerialized()] [HideInInspector()] public List<MethodInfo> methodInfoFixedUpdate;
        [NonSerialized()] [HideInInspector()] public List<ExecuteBehaviour> methodInfoFixedUpdateBehaviours;

        [NonSerialized()] [HideInInspector()] public bool isInitialized;

        public static Executor instance;

        protected virtual bool UnregisterMethodInfoStart(ExecuteBehaviour executeBehaviour)
        {
            var index = methodInfoStartBehaviours.IndexOf(executeBehaviour);

            if (index != Constants.InvalidIndex && index < methodInfoStart.Count && methodInfoStart.Count != 0)
            {
                methodInfoStartBehaviours.RemoveAt(index);
                methodInfoStart.RemoveAt(index);
                return true;
            }

            index = methodInfoStartDeferredBehaviours.IndexOf(executeBehaviour);

            if (index != Constants.InvalidIndex && index < methodInfoStart.Count && methodInfoStart.Count != 0)
            {
                methodInfoStartDeferredBehaviours.RemoveAt(index);
                methodInfoStartDeferred.RemoveAt(index);
                return true;
            }

            return false;
        }

        protected virtual bool RegisterMethodInfoStart(ExecuteBehaviour executeBehaviour)
        {
            var type = executeBehaviour.GetType();
            var methodInfo = type.GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (methodInfo != null)
            {
                if (!isInitialized && executeBehaviour.enabled && executeBehaviour.gameObject.activeInHierarchy)
                {
                    methodInfoStartBehaviours.Add(executeBehaviour);
                    methodInfoStart.Add(methodInfo);
                }
                else
                {
                    methodInfoStartDeferredBehaviours.Add(executeBehaviour);
                    methodInfoStartDeferred.Add(methodInfo);
                }
                return true;
            }

            return false;
        }

        protected virtual bool UnregisterMethodInfoLateUpdate(ExecuteBehaviour executeBehaviour)
        {
            var index = methodInfoLateUpdateBehaviours.IndexOf(executeBehaviour);

            if (index != Constants.InvalidIndex && index < methodInfoLateUpdate.Count && methodInfoLateUpdate.Count != 0)
            {
                methodInfoLateUpdateBehaviours.RemoveAt(index);
                methodInfoLateUpdate.RemoveAt(index);
                return true;
            }

            return false;
        }

        protected virtual bool RegisterMethodInfoLateUpdate(ExecuteBehaviour executeBehaviour)
        {
            var type = executeBehaviour.GetType();
            var methodInfo = type.GetMethod("OnLateUpdate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (methodInfo != null)
            {
                methodInfoLateUpdateBehaviours.Add(executeBehaviour);
                methodInfoLateUpdate.Add(methodInfo);
                return true;
            }

            return false;
        }

        protected virtual bool UnregisterMethodInfoUpdate(ExecuteBehaviour executeBehaviour)
        {
            var index = methodInfoUpdateBehaviours.IndexOf(executeBehaviour);

            if (index != Constants.InvalidIndex && index < methodInfoUpdate.Count && methodInfoUpdate.Count != 0)
            {
                methodInfoUpdateBehaviours.RemoveAt(index);
                methodInfoUpdate.RemoveAt(index);
                return true;
            }

            return false;
        }

        protected virtual bool RegisterMethodInfoUpdate(ExecuteBehaviour executeBehaviour)
        {
            var type = executeBehaviour.GetType();
            var methodInfo = type.GetMethod("OnUpdate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (methodInfo != null)
            {
                methodInfoUpdateBehaviours.Add(executeBehaviour);
                methodInfoUpdate.Add(methodInfo);
                return true;
            }

            return false;
        }

        protected virtual bool UnregisterMethodInfoFixedUpdate(ExecuteBehaviour executeBehaviour)
        {
            var index = methodInfoFixedUpdateBehaviours.IndexOf(executeBehaviour);

            if (index != Constants.InvalidIndex && index < methodInfoFixedUpdate.Count && methodInfoFixedUpdate.Count != 0)
            {
                methodInfoFixedUpdateBehaviours.RemoveAt(index);
                methodInfoFixedUpdate.RemoveAt(index);
                return true;
            }

            return false;
        }

        protected virtual bool RegisterMethodInfoFixedUpdate(ExecuteBehaviour executeBehaviour)
        {
            var type = executeBehaviour.GetType();
            var methodInfo = type.GetMethod("OnFixedUpdate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (methodInfo != null)
            {
                methodInfoFixedUpdateBehaviours.Add(executeBehaviour);
                methodInfoFixedUpdate.Add(methodInfo);
                return true;
            }

            return false;
        }

        public static void UnregisterExecuteBehaviourSingleton(ExecuteBehaviour executeBehaviour)
        {
            instance.UnregisterExecuteBehaviour(executeBehaviour);
        }

        public virtual void UnregisterExecuteBehaviour(ExecuteBehaviour executeBehaviour)
        {
            UnregisterMethodInfoStart(executeBehaviour);
            UnregisterMethodInfoLateUpdate(executeBehaviour);
            UnregisterMethodInfoUpdate(executeBehaviour);
            UnregisterMethodInfoFixedUpdate(executeBehaviour);
            executeBehaviours.Remove(executeBehaviour);
        }

        public static void RegisterExecuteBehaviourSingleton(ExecuteBehaviour executeBehaviour)
        {
            instance.RegisterExecuteBehaviour(executeBehaviour);
        }

        public virtual void RegisterExecuteBehaviour(ExecuteBehaviour executeBehaviour)
        {
            RegisterMethodInfoStart(executeBehaviour);
            RegisterMethodInfoLateUpdate(executeBehaviour);
            RegisterMethodInfoUpdate(executeBehaviour);
            RegisterMethodInfoFixedUpdate(executeBehaviour);
            executeBehaviours.Add(executeBehaviour);
        }

        protected virtual void Awake()
        {
            if (singleton)
                instance = this;

            executeBehaviours = new List<ExecuteBehaviour>();
            methodInfoStart = new List<MethodInfo>();
            methodInfoStartBehaviours = new List<ExecuteBehaviour>();
            methodInfoStartDeferred = new List<MethodInfo>();
            methodInfoStartDeferredBehaviours = new List<ExecuteBehaviour>();
            methodInfoLateUpdate = new List<MethodInfo>();
            methodInfoLateUpdateBehaviours = new List<ExecuteBehaviour>();
            methodInfoUpdate = new List<MethodInfo>();
            methodInfoUpdateBehaviours = new List<ExecuteBehaviour>();
            methodInfoFixedUpdate = new List<MethodInfo>();
            methodInfoFixedUpdateBehaviours = new List<ExecuteBehaviour>();
        }

        protected virtual IEnumerator Start()
        {
            var cachedTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;

            for (int i = 0; i != methodInfoStart.Count; i++)
            {
                var executeBehaviour = methodInfoStartBehaviours[i];

                loadingAmount = (float) i / methodInfoStart.Count;

                if (executeBehaviour != null)
                    loadingState = executeBehaviour.loadingState;

                if (loadingScreenAmountLabel != null)
                    loadingScreenAmountLabel.text = (loadingAmount * 100.0f).ToString("F" + loadingScreenAmountZeroDigits) + "%";

                if (loadingScreenProcessLabel != null)
                    loadingScreenProcessLabel.text = loadingState;

                if (loadingScreenSlider != null)
                    loadingScreenSlider.value = loadingAmount;

                if (executeBehaviour != null)
                {
                    if (executeBehaviour.enabled && executeBehaviour.gameObject.activeInHierarchy)
                        methodInfoStart[i].Invoke(executeBehaviour, null);
                }
                else
                {
                    methodInfoStartBehaviours.RemoveAt(i);
                    methodInfoStart.RemoveAt(i);
                    i--;
                }

                yield return new WaitForEndOfFrame();
            }

            loadingAmount = 1.0f;

            if (loadingScreenAmountLabel != null)
                loadingScreenAmountLabel.text = (loadingAmount * 100.0f).ToString("F" + loadingScreenAmountZeroDigits) + "%";

            if (loadingScreenSlider != null)
                loadingScreenSlider.value = loadingAmount;

            Time.timeScale = cachedTimeScale;
            isInitialized = true;

            yield return new WaitForSeconds(onLoadCallbackDelay);
            onLoadCallback.Invoke();
        }

        protected virtual void LateUpdate()
        {
            if (isInitialized)
            {
                for (int i = 0; i != methodInfoLateUpdate.Count; i++)
                {
                    var executeBehaviour = methodInfoLateUpdateBehaviours[i];

                    if (executeBehaviour != null)
                    {
                        if (executeBehaviour.enabled && executeBehaviour.gameObject.activeInHierarchy)
                            methodInfoLateUpdate[i].Invoke(executeBehaviour, null);
                    }
                    else
                    {
                        methodInfoLateUpdateBehaviours.RemoveAt(i);
                        methodInfoLateUpdate.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        protected virtual void Update()
        {
            if (isInitialized)
            {
                for (int i = 0; i != methodInfoStartDeferred.Count; i++)
                {
                    var executeBehaviour = methodInfoStartDeferredBehaviours[i];

                    if (executeBehaviour != null)
                    {
                        if (executeBehaviour.enabled && executeBehaviour.gameObject.activeInHierarchy)
                        {
                            methodInfoStartDeferred[i].Invoke(executeBehaviour, null);
                            methodInfoStartDeferredBehaviours.RemoveAt(i);
                            methodInfoStartDeferred.RemoveAt(i);
                            i--;
                        }
                    }
                    else
                    {
                        methodInfoStartDeferredBehaviours.RemoveAt(i);
                        methodInfoStartDeferred.RemoveAt(i);
                        i--;
                    }
                }

                for (int i = 0; i != methodInfoUpdate.Count; i++)
                {
                    var executeBehaviour = methodInfoUpdateBehaviours[i];

                    if (executeBehaviour != null)
                    {
                        if (executeBehaviour.enabled && executeBehaviour.gameObject.activeInHierarchy)
                            methodInfoUpdate[i].Invoke(executeBehaviour, null);
                    }
                    else
                    {
                        methodInfoUpdateBehaviours.RemoveAt(i);
                        methodInfoUpdate.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (isInitialized)
            {
                for (int i = 0; i != methodInfoFixedUpdate.Count; i++)
                {
                    var executeBehaviour = methodInfoFixedUpdateBehaviours[i];

                    if (executeBehaviour != null)
                    {
                        if (executeBehaviour.enabled && executeBehaviour.gameObject.activeInHierarchy)
                            methodInfoFixedUpdate[i].Invoke(executeBehaviour, null);
                    }
                    else
                    {
                        methodInfoFixedUpdateBehaviours.RemoveAt(i);
                        methodInfoFixedUpdate.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}