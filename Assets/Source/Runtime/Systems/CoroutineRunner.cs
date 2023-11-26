using System;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Sample.Systems
{
    public class CoroutineRunner : MonoBehaviour
    {
        public void Wait(float duration, Action callback)
        {
            StartCoroutine(WaitCoroutine(duration, callback));
        }

        public void WaitForJobToComplete(JobHandle handle, Action callback)
        {
            StartCoroutine(WaitForJobToCompleteCoroutine(handle, callback));
        }

        private static IEnumerator WaitCoroutine(float duration, Action callback)
        {
            yield return new WaitForSeconds(duration);
            callback.Invoke();
        }

        private static IEnumerator WaitForJobToCompleteCoroutine(JobHandle handle, Action callback)
        {
            yield return new WaitUntil(() => handle.IsCompleted);
            handle.Complete();
            callback.Invoke();
        }
    }
}
