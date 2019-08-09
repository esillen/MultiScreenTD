using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerProjectileManager : BaseNetworkManager {

    public GameObject playerArrow;
    private static uint projectileIDCounter = 0;

    private Dictionary<ProjectileType, GameObject> projectilePrefabs = new Dictionary<ProjectileType, GameObject>();
    private Dictionary<uint, ProjectileBase> spawnedProjectiles = new Dictionary<uint, ProjectileBase>();

    public override void init() {
        projectilePrefabs.Add(ProjectileType.PlayerArrow, playerArrow);
        NetworkServer.RegisterHandler((short)CustomProtocol.FireProjectile, handleFireProjectileMsg);
    }

    #region Server events
    public static void onProjectileDestroyed(uint id) { // Broadcast to clients to remove visual projectile
        NetworkServer.SendToAll((short)CustomProtocol.DestroyProjectile, new UintMsg() { x = id });
    }
    #endregion


    #region Client Communication
    private void handleFireProjectileMsg(NetworkMessage data) {
        FireProjectileMsg msg = data.ReadMessage<FireProjectileMsg>();
        ProjectileBase obj = Instantiate(projectilePrefabs[msg.type], msg.objDetails.pos, Quaternion.identity).GetComponent<ProjectileBase>(); // Spawn real projectile on only server
        msg.objDetails.id = getNewProjectileID(); //Give the new projectile an ID, so we can locate and destroy it later
        spawnedProjectiles.Add(msg.objDetails.id, obj);
        obj.initProjectile(msg.objDetails, msg.projDetails);

        NetworkServer.SendToAll((short)CustomProtocol.FireProjectile, msg);
    }
    #endregion

    public static uint getNewProjectileID() { return projectileIDCounter++;}

    public override void restartGame() {
        foreach (uint id in spawnedProjectiles.Keys)
            Destroy(spawnedProjectiles[id].gameObject);
        spawnedProjectiles.Clear();
    }
}
