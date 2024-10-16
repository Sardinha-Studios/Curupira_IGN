using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvents : MonoBehaviour
{
    [Header("Layer Settings")]
    public LayerMask targetLayerMask;

    [Header("Event Callbacks")]
    [Space] public UnityEvent onTriggerEnter;
    [Space] public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            onTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((targetLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            onTriggerExit?.Invoke();
        }
    }
}
