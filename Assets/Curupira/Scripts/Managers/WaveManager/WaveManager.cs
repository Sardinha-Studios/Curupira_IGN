using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using Sardinha.Events;
using UnityEngine.Events;
public class WaveManager : MonoBehaviour, MMEventListener<MMLifeCycleEvent>
{
    [SerializeField]
    private WaveDetail[] waveDetail;

    [SerializeField]
    private List<GameObject> spawnPoints;
    
    [SerializeField]
    private List<GameObject> enemiesInGame;

    private int currentWave = 0;
    private int maxWaveNumbers;
    private bool currentWaveStart = false;
    private bool startWaveTimer = false;
    private bool waveMangerStarted = false;
    private float currentWaveTime;

    public UnityEvent onRoomEnds;
    
    private void OnEnable()
    {
        this.MMEventStartListening<MMLifeCycleEvent>();

    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMLifeCycleEvent>();
    }
    
    private void Start()
    {
        maxWaveNumbers = waveDetail.Length - 1;
        enemiesInGame = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!waveMangerStarted) return;

        if (startWaveTimer)
        {
            currentWaveTime -= Time.deltaTime;
            CheckWaveTimer();
        }
    }

    private Vector3 SelectSpawnPointsBetween()
    {
        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = Random.Range(spawnPoints[0].transform.position.x, spawnPoints[1].transform.position.x);
        spawnPosition.z = Random.Range(spawnPoints[0].transform.position.z, spawnPoints[2].transform.position.z);
        return spawnPosition;
    }

    private Transform SelectSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
    }

    private GameObject SelectEnemyToSpawn()
    {
        GameObject[] waveEnemies = waveDetail[currentWave].waveEnemies;
        return waveEnemies[Random.Range(0, waveEnemies.Length - 1)];
    }

    public void SpawnWave()
    {
        Debug.Log(gameObject.transform.parent);

        if (waveDetail.Length == 0)
            return;

        if (!waveMangerStarted)
            waveMangerStarted = true;

        if (currentWaveStart)
            return;

        List<GameObject> availableSpawnPoints = new List<GameObject>(spawnPoints);
        for (int i = 0; i < waveDetail[currentWave].enemieQuantity; i++)
        {
            GameObject enemyToSpawn = SelectEnemyToSpawn();

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex].transform;
            availableSpawnPoints.RemoveAt(randomIndex);

            GameObject enemy = Instantiate(enemyToSpawn, spawnPoint.position, spawnPoint.rotation);
            enemiesInGame.Add(enemy);

            if (availableSpawnPoints.Count == 0)
            {
                availableSpawnPoints = new List<GameObject>(spawnPoints);
            }
        }

        currentWaveTime = waveDetail[currentWave].waveMaxTime;
        currentWaveStart = true;
        startWaveTimer = true;
    }

    public void OnMMEvent(MMLifeCycleEvent lifeCycleEvent)
    {
        if (!waveMangerStarted) return;

        if (lifeCycleEvent.MMLifeCycleEventType == MMLifeCycleEventTypes.Death)
        {
            RemoveEnemy(lifeCycleEvent.AffectedHealth.gameObject);
            CheckWaveConclusion();
        }
    }

    private void RemoveEnemy(GameObject enemy)
    {
        if (enemiesInGame.Contains(enemy))
        {
            enemiesInGame.Remove(enemy);
        }
    }

    private void CheckWaveConclusion()
    {
        if (enemiesInGame.Count == 0)
        {
            EventManager.Trigger(GeneralEvents.LevelControllerEvents.OnWaveEnds);
            StartWave();
        } 
    }

    private void CheckWaveConclusionOnTime()
    {
        if (startWaveTimer)
        {
            startWaveTimer = false;
            currentWaveStart = false;
            currentWave++;
            if (currentWave <= maxWaveNumbers)
            {
                EventManager.Trigger(GeneralEvents.LevelControllerEvents.OnWaveEnds);
                SpawnWave();
            }
        }
    }

    private void StartWave()
    {
        currentWaveStart = false;
        currentWave++;
        if (currentWave <= maxWaveNumbers)
        {
            SpawnWave();
        }
        else
        {
            EventManager.Trigger(GeneralEvents.LevelControllerEvents.OnLevelEnd);
            waveMangerStarted = false;
            onRoomEnds?.Invoke();
            onRoomEnds.RemoveAllListeners();
        }
    }

    private void CheckWaveTimer()
    {
        if (currentWaveTime <= 0 && startWaveTimer)
        {
            CheckWaveConclusionOnTime();
        }
    }
}
