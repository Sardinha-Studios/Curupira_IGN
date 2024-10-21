using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using Sardinha.Events;
using System.Net;

public class PlayerPowerUpController : MonoBehaviour
{

    [SerializeField]
    private PowerUpValues powerUpValues;

    [SerializeField]
    private CharacterDash3D characterDash;

    [SerializeField]
    private CharacterMovement characterMovement;

    [SerializeField]
    private CharacterRun characterRun;

    [SerializeField]
    private SwordPowerUpController swordPowerUpController;


    private void Awake()
    {
        EventManager.Register<float, float>(GeneralEvents.PowerUpEvents.OnDashUp, OnDashUpListener);
        EventManager.Register<float, float>(GeneralEvents.PowerUpEvents.OnVelocityUp, OnVelocityUpListener);
        EventManager.Register<SwordPowerUpController>(GeneralEvents.PowerUpEvents.OnSwordSpawned, OnSwordSpawnedListener);
        EventManager.Register<float, float>(GeneralEvents.PowerUpEvents.OnSwordAttackUp, OnSwordAttackUpListener);
        characterDash = GetComponent<CharacterDash3D>();
        characterRun = GetComponent<CharacterRun>();
        characterMovement = GetComponent<CharacterMovement>();
        swordPowerUpController = GetComponentInChildren<SwordPowerUpController>();
    }

    private void LoadPowerUpsValues()
    {
        if (powerUpValues.dashDistance != 0)
        {
            characterDash.DashDistance = powerUpValues.dashDistance;
        }

        if (powerUpValues.walkVelocity != 0)
        {
           characterMovement.WalkSpeed = powerUpValues.walkVelocity;
        }

        if (powerUpValues.runVelocity != 0)
        {
            characterRun.RunSpeed = powerUpValues.runVelocity;
        }

        if (powerUpValues.swordDamage != 0)
        {
            swordPowerUpController.SetDamageValue(powerUpValues.swordDamage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDashUpListener(float dashDistanceUpdade, float dashMaxDistance)
    {
       
        float newDashvalue = characterDash.DashDistance + dashDistanceUpdade;
        if (newDashvalue >= dashMaxDistance)
        {
            characterDash.DashDistance = dashMaxDistance;
            powerUpValues.dashDistance = dashMaxDistance;
        }
        else
        {
            characterDash.DashDistance = newDashvalue;
            powerUpValues.dashDistance = newDashvalue;
        }
        
    }

    private void OnVelocityUpListener(float velocityUp, float maxVelocity)
    {
        float characerRunValue = characterRun.RunSpeed + velocityUp;
        float characterWalkValue = characterMovement.WalkSpeed + velocityUp;

        if (characterWalkValue >= maxVelocity)
        {
            characterMovement.WalkSpeed = maxVelocity;
            powerUpValues.walkVelocity = maxVelocity;
        }
        else
        {
            characterMovement.WalkSpeed = characterWalkValue;
            powerUpValues.walkVelocity = characterWalkValue;
        }

        if (characerRunValue >= maxVelocity)
        {
            characterRun.RunSpeed = maxVelocity;
            powerUpValues.runVelocity = maxVelocity;
        }
        else
        {
            characterRun.RunSpeed = characerRunValue;
            powerUpValues.runVelocity = characerRunValue;
        }

    }

    private void OnSwordAttackUpListener(float attackToAdd, float maxAttack)
    {
        float newAttack = attackToAdd + swordPowerUpController.GetWeaponDamege();
        if (newAttack >= maxAttack)
        {
            swordPowerUpController.SetDamageValue(maxAttack);
            powerUpValues.swordDamage = newAttack;
        }
        else
        {
            swordPowerUpController.SetDamageValue(newAttack);
            powerUpValues.swordDamage = newAttack;
        }
    }

    private void OnSwordSpawnedListener(SwordPowerUpController swordPowerUpControllerValue)
    {
        swordPowerUpController = swordPowerUpControllerValue;
        LoadPowerUpsValues();
    }
    private void OnDestroy()
    {   
        EventManager.Unregister<float, float>(GeneralEvents.PowerUpEvents.OnDashUp, OnDashUpListener);
        EventManager.Unregister<float, float>(GeneralEvents.PowerUpEvents.OnVelocityUp, OnVelocityUpListener);
        EventManager.Unregister<float, float>(GeneralEvents.PowerUpEvents.OnSwordAttackUp, OnSwordAttackUpListener);
        EventManager.Unregister<SwordPowerUpController>(GeneralEvents.PowerUpEvents.OnSwordSpawned, OnSwordSpawnedListener);
    }
}
