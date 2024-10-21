using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sardinha.Events;

public class DashPowerUp : PickableItem
{
    [SerializeField]
    private float powerUpUpgrade;

    [SerializeField]
    private float dashMaxValue;
    public override void PickItem(GameObject picker)
    {
        base.PickItem(picker);
        gameObject.transform.parent.GetComponent<PowerUpSpawner>().CallOnLevelEnd();
        EventManager.Trigger(GeneralEvents.PowerUpEvents.OnDashUp,powerUpUpgrade, dashMaxValue);
    }

}
