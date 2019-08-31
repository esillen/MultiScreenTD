using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySettings : BaseNetworkManager {

    public static DifficultySettings singleton;

    // "Trickling" behaviour. Enemies spawn constantly with a bit of variance. 
    public float tricklingSpawnDelayMean = 2;
    public float tricklingSpawnDelayMeanTimeModifier = -0.05f;
    public float tricklingSpawnDelayVariance = 1;


    // "Gang of enemies spawn at once" behaviour
    public float groupSpawnSize = 5;
    public float groupSpawnSizeTimeModifier = 0.1f;
    public float groupSpawnDelay = 8;
    public float groupSpawnDelayTimeModifier = -0.1f;

    #region Enemy Level System
    private int currentWaveLevel =0;
    private float waveDuration = 30;
    private float nextWaveStartTime;
    private List<WaveLevelGenerator> waveLevels = new List<WaveLevelGenerator>();

    public int getNewSpawnedEnemyLevel() {
        return currentWaveLevel >= waveLevels.Count ? currentWaveLevel : waveLevels[currentWaveLevel].sampleLevel();
    }
    #endregion

    #region Start Stats
    public static int startGameHealth = 100;
    public static int startGroupSpawnSize = 10;
    public static int startGroupSpawnDelay = 8;

    public static int startTowerDamage = 5;
    public static int startMaxArrows = 4;
    public static float startRange = 1.7f;
    public static float startSpeed = 5;
    public static float startArrowRespawnRate = 1.5f;
    #endregion

    #region Economy System
    public static uint dmgUpgradeCost = 40, maxArrowsUpgradeCost = 50, rangeUpgradeCost = 20, cooldownUpgradeCost = 30;
    public static int maxDmgUpgrade = 100, maxMaxArrowUpgrade = 20, maxRangeUpgrade = 100, maxCooldownUpgrade = 13;

    public static void incrementTowerDamage(Tower tower) { tower.damage += 1; }
    public static void incrementTowerMaxArrows(Tower tower) { tower.maxArrows += 1; }
    public static void incrementTowerRange(Tower tower) { tower.range += 0.5f; }
    public static void incrementRespawnRate(Tower tower) {tower.arrowRespawnRate = Mathf.Max(0.2f, tower.arrowRespawnRate - 0.1f); }
    #endregion

    void Start() {
        singleton = this;
        this.enabled = false;
    }

    public override void init() {
        waveLevels.Add(new WaveLevelGenerator(new List<LevelProbability>() { //Level-1
            new LevelProbability(1, 100)
        }));
        waveLevels.Add(new WaveLevelGenerator(new List<LevelProbability>() { //Level-2
            new LevelProbability(1, 50), new LevelProbability(2, 50)
        }));
        waveLevels.Add(new WaveLevelGenerator(new List<LevelProbability>() { //Level-3
            new LevelProbability(1, 10), new LevelProbability(2, 50), new LevelProbability(3, 40)
        }));
        waveLevels.Add(new WaveLevelGenerator(new List<LevelProbability>() { //Level-4
            new LevelProbability(2, 10), new LevelProbability(3, 40), new LevelProbability(4, 40), new LevelProbability(5, 10)
        }));
        waveLevels.Add(new WaveLevelGenerator(new List<LevelProbability>() { //Level-5
            new LevelProbability(3, 10), new LevelProbability(4, 40), new LevelProbability(5, 40), new LevelProbability(6, 10)
        }));
    }
    public override void restartGame(RestartMessage restartMessage) {
        this.enabled = true;
        groupSpawnSize = startGroupSpawnSize;
        groupSpawnDelay = startGroupSpawnDelay;
        currentWaveLevel = 0;
        nextWaveStartTime = Time.time + waveDuration;
    }

    private void Update() {
        tricklingSpawnDelayMean = Mathf.Max(tricklingSpawnDelayMean + tricklingSpawnDelayMeanTimeModifier * Time.deltaTime, 0);
        groupSpawnSize += groupSpawnSizeTimeModifier * Time.deltaTime;
        groupSpawnDelay = Mathf.Max(groupSpawnDelay + groupSpawnDelayTimeModifier * Time.deltaTime);

        if(Time.time > nextWaveStartTime) {
            currentWaveLevel += 1;
            nextWaveStartTime = Time.time + waveDuration;
        }
    }

}

public class WaveLevelGenerator {

    private int maxValue = 0;
    private List<LevelProbability> levels;

    public WaveLevelGenerator(List<LevelProbability> levels) {
        this.levels = levels;
        levels.ForEach(x => maxValue += x.weight);
    }

    public int sampleLevel() {
        int x = Random.RandomRange(0, maxValue);
        int weightCounter = 0;
        foreach(LevelProbability prob in levels) {
            weightCounter += prob.weight;
            if (weightCounter > x)
                return prob.level;
        }
        return levels[levels.Count - 1].level;
    }
}

public class LevelProbability {
    public int level, weight;
    public LevelProbability(int level, int weight) { this.level = level; this.weight = weight; }
}