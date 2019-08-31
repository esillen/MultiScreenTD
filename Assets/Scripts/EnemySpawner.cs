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
    private Dictionary<int, Color> enemyLevelColors = new Dictionary<int, Color>() {
        {1, Color.white }, {2, Color.cyan },  {3, Color.green }, {4, Color.gray },  {5, Color.red },
    };

    public override void init() {}
    public override void restartGame(RestartMessage restartMessage) {
        StopAllCoroutines();

        foreach (uint id in spawnedEnemies.Keys) {
            try {Destroy(spawnedEnemies[id].gameObject);} 
            catch { }
        }
        spawnedEnemies.Clear();

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
        for (int i = 0; i < enemiesToSpawn; i++)
            SpawnEnemy();
    }

    private void SpawnEnemy() {
        Vector3 randomPositionInSpawnArea = new Vector3(
                Random.Range(spawnAreaBoxCollider.bounds.min.x, spawnAreaBoxCollider.bounds.max.x),
                0,
                Random.Range(spawnAreaBoxCollider.bounds.min.z, spawnAreaBoxCollider.bounds.max.z));
        Enemy newEnemy = Instantiate(enemyPrefab, randomPositionInSpawnArea, Quaternion.identity).GetComponent<Enemy>();
        newEnemy.initEnemyTarget(goal, gameManager, idCounter++);
        newEnemy.transform.LookAt(goal.transform);
        spawnedEnemies.Add(newEnemy.id, newEnemy);

        int enemyLevel = DifficultySettings.singleton.getNewSpawnedEnemyLevel();
        initEnemyStats(newEnemy, enemyLevel);
        SpawnedObject visualAtributes = calculateVisualAtributes(newEnemy, enemyLevel);
        newEnemy.init(visualAtributes);

        SpawnEnemyMsg msg = new SpawnEnemyMsg() { type = EnemyType.Soldier, details = visualAtributes };
        NetworkServer.SendToAll((short)CustomProtocol.SpawnEnemyMsg, msg);
    }


    private SpawnedObject calculateVisualAtributes(Enemy newEnemy, int enemyLevel) {
        Transform trans = newEnemy.transform;
        Color c = enemyLevel < enemyLevelColors.Keys.Count ? enemyLevelColors[enemyLevel] : Color.red;
        return new SpawnedObject() {
            id = newEnemy.id, pos = trans.position, rot = trans.eulerAngles, speed = newEnemy.speed, color = c,
            scale = Vector3.one * (1 + 0.4f * enemyLevel),
        };
    }

    private void initEnemyStats(Enemy theEnemy, int level) {
        theEnemy.damage = 3 + level * 2;
        theEnemy.health = 3 + level * 2;
        theEnemy.rewardLevel = level;
    }
}
