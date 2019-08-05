using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ClientFireManager : MonoBehaviour {

    public GameObject visualPlayerArrow;

    private Dictionary<ProjectileType, GameObject> visualProjectilePrefabs = new Dictionary<ProjectileType, GameObject>();
    private Dictionary<uint, VisualProjectile> spawnedProjectiles = new Dictionary<uint, VisualProjectile>();


    public void init() {
        visualProjectilePrefabs.Add(ProjectileType.PlayerArrow, visualPlayerArrow);
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.FireProjectile, handleFireProjectileMsg);
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.DestroyProjectile, handleDestroyProjectile);
    }

    private void handleDestroyProjectile(NetworkMessage data) {
        uint id = data.ReadMessage<UintMsg>().x;
        if (spawnedProjectiles.ContainsKey(id)) {
            spawnedProjectiles[id].destroyProjectile();
            spawnedProjectiles.Remove(id);
        }
    }

    private void handleFireProjectileMsg(NetworkMessage data) {
        FireProjectileMsg msg = data.ReadMessage<FireProjectileMsg>();
        VisualProjectile proj = Instantiate(visualProjectilePrefabs[msg.type], msg.startPos, Quaternion.LookRotation(msg.direction)).GetComponent<VisualProjectile>(); // Spawn real projectile on only server
        spawnedProjectiles.Add(msg.id, proj);
        proj.init(msg.speed, msg.id);
    }

}
