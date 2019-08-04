using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public float strength = 5;
    public Transform goal;
    private NavMeshAgent navMeshAgent;
    private GameManager gameManager;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(new Vector3(goal.position.x, 0, transform.position.z));
    }

    public void Initialize(Transform goal, GameManager gameManager) {
        this.goal = goal;
        this.gameManager = gameManager;
    }

    private void Update() {
        if (navMeshAgent.remainingDistance < 3) {
            gameManager.TakeDamage(strength);
            Destroy(gameObject);
        }
    }

    public void TakeDamage() {
        Destroy(gameObject);
    }

}
