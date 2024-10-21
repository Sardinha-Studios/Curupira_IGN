using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using Sardinha.Events;

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


    private void Awake()
    {
        EventManager.Register<float, float>(GeneralEvents.PowerUpEvents.OnDashUp, OnDashUpListener);
        EventManager.Register<float, float>(GeneralEvents.PowerUpEvents.OnVelocityUp, OnVelocityUpListener);
        characterDash = GetComponent<CharacterDash3D>();
        characterRun = GetComponent<CharacterRun>();
        characterMovement = GetComponent<CharacterMovement>();
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
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPowerUpsValues();
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
        }
        else
        {
            characterMovement.WalkSpeed = characterWalkValue;
            powerUpValues.walkVelocity = characterWalkValue;
        }

        if (characerRunValue >= maxVelocity)
        {
            characterRun.RunSpeed = maxVelocity;
        }
        else
        {
            characterRun.RunSpeed = characerRunValue;
            powerUpValues.runVelocity = characerRunValue;
        }

    }

    private void OnDestroy()
    {
        EventManager.Unregister<float, float>(GeneralEvents.PowerUpEvents.OnDashUp, OnDashUpListener);
        EventManager.Unregister<float, float>(GeneralEvents.PowerUpEvents.OnVelocityUp, OnVelocityUpListener);
    }
}
