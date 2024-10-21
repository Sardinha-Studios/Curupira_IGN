using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
        [Header("Unity Callback Events")]
        [Space] public UnityEvent OnStartCallback;

        private void Start()
        {
            OnStartCallback?.Invoke();
        }
}
