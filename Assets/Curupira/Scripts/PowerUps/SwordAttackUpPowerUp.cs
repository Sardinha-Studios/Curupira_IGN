using MoreMountains.TopDownEngine;
using Sardinha.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackUpPowerUp : PickableItem
{
    [SerializeField]
    private float attackToUp;

    [SerializeField]
    private float maxAttack;

    public override void PickItem(GameObject picker)
    {
        base.PickItem(picker);
        gameObject.transform.parent.GetComponent<PowerUpSpawner>().CallOnLevelEnd();
        EventManager.Trigger(GeneralEvents.PowerUpEvents.OnSwordAttackUp, attackToUp, maxAttack);

    }
}
