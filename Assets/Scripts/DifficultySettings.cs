using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySettings : BaseNetworkManager {

    // "Trickling" behaviour. Enemies spawn constantly with a bit of variance. 
    public float tricklingSpawnDelayMean = 2;
    public float tricklingSpawnDelayMeanTimeModifier = -0.05f;
    public float tricklingSpawnDelayVariance = 1;

    // "Gang of enemies spawn at once" behaviour
    public float groupSpawnSize = 5;
    public float groupSpawnSizeTimeModifier = 0.1f;
    public float groupSpawnDelay = 8;
    public float groupSpawnDelayTimeModifier = -0.1f;

    void Start() {
        this.enabled = false;
    }

    public override void init() {}
    public override void restartGame(RestartMessage restartMessage) {
        this.enabled = true;
        groupSpawnSize = 5;
    }

    private void Update() {
        tricklingSpawnDelayMean = Mathf.Max(tricklingSpawnDelayMean + tricklingSpawnDelayMeanTimeModifier * Time.deltaTime, 0);
        groupSpawnSize += groupSpawnSizeTimeModifier * Time.deltaTime;
        groupSpawnDelay = Mathf.Max(groupSpawnDelay + groupSpawnDelayTimeModifier * Time.deltaTime);
    }

}
