using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpValues", menuName = "ScriptableObjects/PowerUpValues")]
public class PowerUpValues : ScriptableObject
{
    public float dashDistance = 0;
    public float walkVelocity = 0;
    public float runVelocity = 0;
    public float swordDamage = 0;
    public int swordCombo = 0;
    
}
