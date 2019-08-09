using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManagerHUD))]
public class CustomNetworkManager : NetworkManager {

    public static CustomNetworkManager singleton;
    public static bool isServer = false, isConnected = false;
    public GameObject universalManagers, clientManagers, serverManagers;


    #region Server
    public override void OnServerConnect(NetworkConnection conn) {
        if (isConnected)
            return;

        isServer = true;
        singleton = this;
        isConnected = true;
        base.OnServerConnect(conn);

        GetComponent<NetworkManagerHUD>().showGUI = false;
        initManagers(new GameObject[] { universalManagers, serverManagers });
    }

    public static void restartGame() {
        if (isServer == false)
            return;
        NetworkServer.SendToAll((short)CustomProtocol.RestartGame, new UintMsg());
        singleton.restartManagers(new GameObject[] { singleton.universalManagers, singleton.serverManagers });
    }
    #endregion

    #region Client
    public override void OnClientConnect(NetworkConnection conn) {
        if (isConnected)
            return;

        singleton = this;
        isConnected = true;
        base.OnClientConnect(conn);

        GetComponent<NetworkManagerHUD>().showGUI = false;
        initManagers(new GameObject[] { universalManagers, clientManagers });
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.RestartGame, handleRestartGameMsg);
    }

    private void handleRestartGameMsg(NetworkMessage msg) {
        restartManagers(new GameObject[] { singleton.universalManagers, singleton.clientManagers });
    }
    #endregion

    #region Utils
    private void initManagers(GameObject[] objs) {iterateManagers(objs, (BaseNetworkManager b) =>  b.init());}
    private void restartManagers(GameObject[] objs) { iterateManagers(objs, (BaseNetworkManager b) => b.restartGame()); }
    private void iterateManagers(GameObject[] managerParents, Action<BaseNetworkManager> a) {
        foreach (GameObject g in managerParents)
            foreach (BaseNetworkManager manager in g.transform.GetComponentsInChildren<BaseNetworkManager>())
                a.Invoke(manager);
    }
    #endregion
}
