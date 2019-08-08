using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DifficultySettings))]
public class EnemySpawner : BaseNetworkManager {

    public BoxCollider spawnAreaBoxCollider;
    public GameObject enemyPrefab;
    public Transform goal;
    public GameManager gameManager;

    private uint idCounter = 0;
    private DifficultySettings difficultySettings;
    private Dictionary<uint, Enemy> spawnedEnemies = new Dictionary<uint, Enemy>();

    public override void init() {}
    public override void restartGame(RestartMessage restartMessage) {
        foreach (uint id in spawnedEnemies.Keys)
            Destroy(spawnedEnemies[id]);
        spawnedEnemies.Clear();
        StopAllCoroutines();

        idCounter = 0;
        difficultySettings = GetComponent<DifficultySettings>();
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnEnemyGroupRoutine());
    }

    private IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(Mathf.Max(0, Random.Range(
                difficultySettings.tricklingSpawnDelayMean - difficultySettings.tricklingSpawnDelayVariance,
                difficultySettings.tricklingSpawnDelayMean + difficultySettings.tricklingSpawnDelayVariance
                )));
        SpawnEnemy();
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyGroupRoutine() {
        yield return new WaitForSeconds(difficultySettings.groupSpawnDelay);
        SpawnEnemyGroup((int)difficultySettings.groupSpawnSize);
        StartCoroutine(SpawnEnemyGroupRoutine());
    }

    private void SpawnEnemyGroup(int enemiesToSpawn) {
        for (int i = 0; i < enemiesToSpawn; i++) {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        Vector3 randomPositionInSpawnArea = new Vector3(
                Random.Range(spawnAreaBoxCollider.bounds.min.x, spawnAreaBoxCollider.bounds.max.x),
                0,
                Random.Range(spawnAreaBoxCollider.bounds.min.z, spawnAreaBoxCollider.bounds.max.z));
        Enemy newEnemy = Instantiate(enemyPrefab, randomPositionInSpawnArea, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.Initialize(goal, gameManager, idCounter++);
        newEnemy.transform.LookAt(goal.transform);

        SpawnEnemyMsg msg = NetworkUtils.cSpawnEnemyMsg(EnemyType.Soldier, randomPositionInSpawnArea, newEnemy.transform.localEulerAngles, newEnemy.speed, newEnemy.id);
        NetworkServer.SendToAll((short)CustomProtocol.SpawnEnemyMsg, msg);
    }

}
