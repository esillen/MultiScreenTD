using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public Transform goal;
    public float speed = 5;
    public int damage = 5;
    public int reward = 1;
    public int health = 5;
    public uint id;

    private GameManager gameManager;

    private void Start() {
        if(CustomNetworkManager.isServer == false) { // Should not be needed in the future
            this.enabled = false;
            return;
        }
    }

    public void Initialize(Transform goal, GameManager gameManager, uint id) {
        this.id = id; this.goal = goal; this.gameManager = gameManager;
    }

    private void Update() {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
        if ((transform.position - goal.position).magnitude < 3)
            enterGoal();
    }

    public void enterGoal() {
        gameManager.TakeDamage(damage);
        removeFromServer();
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        if(health <= 0) {
            RewardSystem.getReward(reward);
            removeFromServer();
        }
    }

    public void removeFromServer() {
        NetworkServer.SendToAll((short)CustomProtocol.DestroyEnemyMsg, new UintMsg() { x = id });
        Destroy(gameObject);
    }

}
