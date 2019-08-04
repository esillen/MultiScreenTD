using UnityEngine;

public class GameManager : MonoBehaviour {

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
