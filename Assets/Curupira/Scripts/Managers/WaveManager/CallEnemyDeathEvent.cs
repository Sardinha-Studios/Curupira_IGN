using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
public class CallEnemyDeathEvent : MonoBehaviour
{
    private Health enemyHealth;

    private void Awake()
    {
       //enemyHealth = GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //enemyHealth.OnDeath += OnDeathListener;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDeathListener()
    {
        //Debug.Log(gameObject.GetInstanceID());
        //Debug.Log("Death");

    }

    private void OnDestroy()
    {
        //enemyHealth.OnDeath -= OnDeathListener;
    }
}
