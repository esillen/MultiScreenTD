﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class ClientEnemyManager : BaseNetworkManager {

    public GameObject soldierPrefab;
    private Dictionary<EnemyType, GameObject> typeToObjects = new Dictionary<EnemyType, GameObject>();
    private Dictionary<uint, VisualObject> spawnedEnemies = new Dictionary<uint, VisualObject>();


	public override void init() {
        typeToObjects.Add(EnemyType.Soldier, soldierPrefab);
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.SpawnEnemyMsg, handleEnemySpawnMsg);
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.DestroyEnemyMsg, handleEnemyDestroyedMsg);
    }

    public override void restartGame(RestartMessage restartMessage) {
        foreach (uint id in spawnedEnemies.Keys)
            Destroy(spawnedEnemies[id].gameObject);
        spawnedEnemies.Clear();
    }

    #region Handlers
    private void handleEnemySpawnMsg(NetworkMessage msg) {
        SpawnEnemyMsg data = msg.ReadMessage<SpawnEnemyMsg>();
        VisualObject enemy = Instantiate(typeToObjects[data.type], data.details.pos, Quaternion.identity, null).GetComponent<VisualObject>();
        enemy.init(data.details);
        spawnedEnemies.Add(data.details.id, enemy);
    }

    private void handleEnemyDestroyedMsg(NetworkMessage msg) {
        uint id = msg.ReadMessage<UintMsg>().x;
        if (spawnedEnemies.ContainsKey(id)) {
            spawnedEnemies[id].destroyVisualObject();
            spawnedEnemies.Remove(id);
        }
    }
    #endregion
}
