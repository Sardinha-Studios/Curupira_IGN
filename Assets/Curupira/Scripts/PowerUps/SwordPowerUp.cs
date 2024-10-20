using MoreMountains.TopDownEngine;
using Sardinha.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPowerUp : PickableItem
{


    public override void PickItem(GameObject picker)
    {
        base.PickItem(picker);
        gameObject.transform.parent.GetComponent<PowerUpSpawner>().CallOnLevelEnd();
    }


}
