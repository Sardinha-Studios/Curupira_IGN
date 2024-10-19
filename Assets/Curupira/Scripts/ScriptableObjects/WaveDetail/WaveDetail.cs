using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveDetail", menuName = "ScriptableObjects/WaveDetail")]
public class WaveDetail : ScriptableObject
{
    public GameObject [] waveEnemies;
    public float waveMaxTime;
    public int enemieQuantity;
}
