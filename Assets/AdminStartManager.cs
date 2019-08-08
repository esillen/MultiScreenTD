using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminStartManager : BaseNetworkManager {

    public GameObject restartButton;

    public override void init() {
        if (CustomNetworkManager.isServer == false)
            restartButton.SetActive(false);
    }

    public void onRestartGamePressed() {
        if(CustomNetworkManager.isConnected)
            CustomNetworkManager.restartGame();
    }


    public override void restartGame() {}
}
