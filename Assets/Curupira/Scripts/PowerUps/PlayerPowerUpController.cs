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
        }
        
    }

    private void OnDestroy()
    {
        EventManager.Unregister<float, float>(GeneralEvents.PowerUpEvents.OnDashUp, OnDashUpListener);
    }
}
