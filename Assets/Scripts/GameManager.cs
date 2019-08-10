using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseNetworkManager {

    private float startTime;
    private float lifeLeft = 100;
    private bool isPlaying = true;

    public override void restartGame(RestartMessage restartMessage) {
        base.restartGame(restartMessage);
        lifeLeft = DifficultySettings.singleton.startGameHealth;
        startTime = Time.time;
        isPlaying = true;
}

    public void TakeDamage(float damage) {
        lifeLeft -= damage;
        if (lifeLeft <= 0)
            GameOver();
    }

    private void GameOver() {
        if (isPlaying) {
            isPlaying = false;
            UIManager.singleton.onGameOver(Time.time - startTime);
        }
    }

   
}
