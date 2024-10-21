using MoreMountains.TopDownEngine;
using Sardinha.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPowerUp : PickableItem
{
    [SerializeField]
    private float velocityUpdate;

    [SerializeField]
    private float velocitymaxValue;
    public override void PickItem(GameObject picker)
    {
        base.PickItem(picker);
        gameObject.transform.parent.GetComponent<PowerUpSpawner>().CallOnLevelEnd();
        Debug.Log("VelocityUp Trigger");
        EventManager.Trigger(GeneralEvents.PowerUpEvents.OnVelocityUp, velocityUpdate, velocitymaxValue);

    }
}
