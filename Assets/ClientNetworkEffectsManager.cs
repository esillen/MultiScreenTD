using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class ClientNetworkEffectsManager : MonoBehaviour {

	public void init() {
        NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.EffectsMessage, handleSpawnEffectMsg);
    }



    private void handleSpawnEffectMsg(NetworkMessage data) {

    }
}
