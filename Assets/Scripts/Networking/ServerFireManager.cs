using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerFireManager : MonoBehaviour {

    public GameObject playerArrow;
    private static uint projectileIDCounter = 0;

    private Dictionary<ProjectileType, GameObject> projectilePrefabs = new Dictionary<ProjectileType, GameObject>();

    public void init() {
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
        GameObject obj = Instantiate(projectilePrefabs[msg.type], msg.startPos, Quaternion.LookRotation(msg.direction)); // Spawn real projectile on only server
        msg.id = getNewProjectileID(); //Give the new projectile an ID, so we can locate and destroy it later
        obj.GetComponent<ProjectileBase>().init(msg.id);
    }
    #endregion

    public static uint getNewProjectileID() { return projectileIDCounter++;}
}
