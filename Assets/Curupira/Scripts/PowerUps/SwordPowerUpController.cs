using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sardinha.Events;
using MoreMountains.TopDownEngine;

public class SwordPowerUpController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private MeleeWeapon[] swordHits;
    void Start()
    {
        EventManager.Trigger(GeneralEvents.PowerUpEvents.OnSwordSpawned, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDamageValue(float damageToAdd)
    {
        foreach (MeleeWeapon weapon in swordHits)
        {
            if (weapon.enabled)
            {
                weapon.MaxDamageCaused = damageToAdd;
                weapon.MinDamageCaused = damageToAdd;
            }
        }

    }

    public float GetWeaponDamege()
    {
        return swordHits[0].MaxDamageCaused;
    }
}
