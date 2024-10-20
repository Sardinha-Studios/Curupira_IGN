using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class UpdateBossHealthBar : MonoBehaviour
{
    private Health Health;

    private void Start()
    {
        Health = GetComponent<Health>();
    }

    private void Update()
    {
        if (!GUIManager.HasInstance) return;
        GUIManager.Instance.UpdateBossHealthBar(Health.CurrentHealth, 0f, Health.MaximumHealth);
    }

    public void ActiveHealthBar(bool active)
    {
        GUIManager.Instance.bossHealthBar.gameObject.SetActive(active);
    }
}
