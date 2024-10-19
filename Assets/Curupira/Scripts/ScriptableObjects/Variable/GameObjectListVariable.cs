using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameObjectListVariable", menuName = "ScriptableObjectsVariable/GameObjectListVariable")]
public class GameObjectListVariable : ScriptableObject
{
    public List<GameObject> currentValue;
}
