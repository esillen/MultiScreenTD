using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.AI;

public class Enemy : VisualObject {

    public Transform goal;
    public int damage = 5;
    public int rewardLevel = 1;
    public int health = 5;

    private GameManager gameManager;

    private void Start() {
        if(CustomNetworkManager.isServer == false) { // Should not be needed in the future
            this.enabled = false;
            return;
        }
    }

    public void initEnemyTarget(Transform goal, GameManager gameManager, uint id) {
       this.goal = goal; this.gameManager = gameManager; this.id = id;
    }

    private void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
        if ((transform.position - goal.position).magnitude < 3)
            enterGoal();
    }

    public void enterGoal() {
        gameManager.TakeDamage(damage);
        destroyVisualObject();
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        if(health <= 0) {
            RewardSystem.getReward(rewardLevel);
            destroyVisualObject();
        }
    }

    public override void destroyVisualObject() {
        NetworkServer.SendToAll((short)CustomProtocol.DestroyEnemyMsg, new UintMsg() { x = id });
        Destroy(gameObject);
    }
}
