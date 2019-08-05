using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(DifficultySettings))]
public class EnemySpawner : NetworkBehaviour {

    public BoxCollider spawnAreaBoxCollider;
    public GameObject enemyPrefab;
    public Transform goal;
    public GameManager gameManager;

    private DifficultySettings difficultySettings;

    // Only called when everybode has been connected
    public void startGame() {
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
        GameObject enemyGameObject = Instantiate(enemyPrefab, randomPositionInSpawnArea, Quaternion.identity);
        enemyGameObject.GetComponent<Enemy>().Initialize(goal, gameManager);

        NetworkServer.Spawn(enemyGameObject); // Spawn it on the network
    }

}
