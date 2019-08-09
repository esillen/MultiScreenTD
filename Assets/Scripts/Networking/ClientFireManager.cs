using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ClientFireManager : BaseNetworkManager {

    public GameObject visualPlayerArrow;

    private Dictionary<ProjectileType, GameObject> visualProjectilePrefabs = new Dictionary<ProjectileType, GameObject>();
    private Dictionary<uint, VisualObject> spawnedProjectiles = new Dictionary<uint, VisualObject>();


    public override void init() {
        visualProjectilePrefabs.Add(ProjectileType.PlayerArrow, visualPlayerArrow);
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.FireProjectile, handleFireProjectileMsg);
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.DestroyProjectile, handleDestroyProjectile);
    }

    public override void restartGame() {
        foreach (uint id in spawnedProjectiles.Keys)
            Destroy(spawnedProjectiles[id].gameObject);
        spawnedProjectiles.Clear();
    }

    private void handleDestroyProjectile(NetworkMessage data) {
        uint id = data.ReadMessage<UintMsg>().x;
        if (spawnedProjectiles.ContainsKey(id)) {
            spawnedProjectiles[id].destroyVisualObject();
            spawnedProjectiles.Remove(id);
        }
    }

    private void handleFireProjectileMsg(NetworkMessage data) {
        FireProjectileMsg msg = data.ReadMessage<FireProjectileMsg>();
        VisualObject proj = Instantiate(visualProjectilePrefabs[msg.type], msg.objDetails.pos, Quaternion.identity).GetComponent<VisualObject>(); // Spawn real projectile on only server
        spawnedProjectiles.Add(msg.objDetails.id, proj);
        proj.init(msg.objDetails);
    }

}
