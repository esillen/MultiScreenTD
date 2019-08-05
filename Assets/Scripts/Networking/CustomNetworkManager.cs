using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManagerHUD))]
public class CustomNetworkManager : NetworkManager {

    public static bool isServer = false;

    public ServerFireManager theServerFireManager;
    public ClientFireManager theClientFireManager;
    public EnemySpawner theEnemySpawner;

    public override void OnServerConnect(NetworkConnection conn) {
        isServer = true;
        base.OnServerConnect(conn);

        GetComponent<NetworkManagerHUD>().showGUI = false;
        theServerFireManager.init();

        theEnemySpawner.startGame();
    }


    public override void OnClientConnect(NetworkConnection conn) {
        base.OnClientConnect(conn);
        GetComponent<NetworkManagerHUD>().showGUI = false;
        theClientFireManager.init();
    }

}
