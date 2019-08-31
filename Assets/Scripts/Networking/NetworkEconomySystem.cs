using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkEconomySystem : BaseNetworkManager {

    private static NetworkEconomySystem singleton;

    public int startCurrency = 100;

    private int currency = 0;
    private Tower currentTower;
    private Action buyCallback;
    private Dictionary<PurchaseType, uint> purchuaseCosts = new Dictionary<PurchaseType, uint>();

    public override void init() {
        singleton = this;
        if (CustomNetworkManager.isServer)
            NetworkServer.RegisterHandler((short)CustomProtocol.PurchuaseMsg, handleTradeMsg);
        else {
            NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.CurrencyMessage, handleCurrencyMsg);
            NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.PurchuaseMsg, handlePurchuasecceptedMsg);
        }
    }

    #region Client
    public void handleCurrencyMsg(NetworkMessage data) {
        int currency = (int)data.ReadMessage<UintMsg>().x;
        UIManager.singleton.updateCurrency(currency);
    }


    public void handlePurchuasecceptedMsg(NetworkMessage data) {buyCallback.Invoke();}
    private void sendPurchuaseMsg(PurchaseType type, Action buyFunc) {
        buyCallback = buyFunc;
        NetworkManager.singleton.client.Send((short)CustomProtocol.PurchuaseMsg, new UintMsg() { x = purchuaseCosts[type]});
    }
    public void buyDamage() { sendPurchuaseMsg(PurchaseType.Damage, () => DifficultySettings.incrementTowerDamage(currentTower)); }
    public void buyRange() { sendPurchuaseMsg(PurchaseType.Range, () => DifficultySettings.incrementTowerRange(currentTower)); }
    public void buyCooldown() { sendPurchuaseMsg(PurchaseType.Cooldown, () => DifficultySettings.incrementRespawnRate(currentTower)); }
    public void buyMagazine() { sendPurchuaseMsg(PurchaseType.Magazine, () => DifficultySettings.incrementTowerMaxArrows(currentTower)); }
   
    public static void setCurrentTower(Tower tower) { singleton.currentTower = tower; }
    #endregion

    #region Server
    public override void restartGame(RestartMessage restartMessage) {
        purchuaseCosts.Clear();
        purchuaseCosts.Add(PurchaseType.Damage, DifficultySettings.dmgUpgradeCost);
        purchuaseCosts.Add(PurchaseType.Range, DifficultySettings.rangeUpgradeCost);
        purchuaseCosts.Add(PurchaseType.Cooldown, DifficultySettings.cooldownUpgradeCost);
        purchuaseCosts.Add(PurchaseType.Magazine, DifficultySettings.maxArrowsUpgradeCost);

        currency = startCurrency;
        broadcastCurrentCurrencyLevel();
    }

    private void handleTradeMsg(NetworkMessage msg) {
        int cost = (int)msg.ReadMessage<UintMsg>().x;
        if (currency - cost >= 0) {
            currencyEvent(-cost);
            msg.conn.Send((short)CustomProtocol.PurchuaseMsg, new UintMsg()); // Send an acceptance msg
        }
    }

    private void broadcastCurrentCurrencyLevel() {
        NetworkServer.SendToAll((short)CustomProtocol.CurrencyMessage, new UintMsg() { x = (uint)currency });
    }

    public void currencyEvent(int delta) {
        currency += delta;
        UIManager.singleton.updateCurrency(currency);
        broadcastCurrentCurrencyLevel();
    }
    #endregion


}
