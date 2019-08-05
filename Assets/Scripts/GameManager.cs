using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public World2D world2D;

    private float lifeLeft = 100;

    public void TakeDamage(float damage) {
        lifeLeft -= damage;
        if (lifeLeft <= 0) {
            GameOver();
        }
    }

    private void GameOver() {
        Debug.Log("you got served");
    }

   
}
