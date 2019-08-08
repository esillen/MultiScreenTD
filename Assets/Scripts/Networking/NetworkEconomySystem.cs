using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkEconomySystem : BaseNetworkManager {

    public UIManager theUIManager;
    public int startCurrency = 100;

    private int currency = 0;
    private Dictionary<PurchaseType, Action<PlayerID>> purchusases = new Dictionary<PurchaseType, Action<PlayerID>>();



    public override void init() {
        purchusases.Add(PurchaseType.ArrowTower, buyArrowTower);
        if (CustomNetworkManager.isServer)
            NetworkServer.RegisterHandler((short)CustomProtocol.CurrencyMessage, handleTradeMsg);
        else
            NetworkManager.singleton.client.RegisterHandler((short)CustomProtocol.CurrencyMessage, handleTradeMsg);
    }


    #region Client
    private void handleCurrencyMsg(NetworkMessage data) {
        int currency = (int)data.ReadMessage<UintMsg>().x;
        theUIManager.updateCurrency(currency);
    }
    #endregion

    #region Server
    public override void restartGame() {
        currency = startCurrency;
        broadcastCurrentCurrencyLevel();
    }

    private void handleTradeMsg(NetworkMessage data) {
        TradeMsg msg = data.ReadMessage<TradeMsg>();
        if (currency - msg.cost >= 0) {
            purchusases[msg.type](msg.id);
            currencyEvent(-msg.cost);
        }
    }

    private void buyArrowTower(PlayerID id) {
        Debug.LogError("Building Tower for player: " + id);
    }
    private void broadcastCurrentCurrencyLevel() {
        NetworkServer.SendToAll((short)CustomProtocol.CurrencyMessage, new UintMsg() { x = (uint)currency });
    }

    public void currencyEvent(int delta) {
        currency += delta;
        theUIManager.updateCurrency(currency);
        broadcastCurrentCurrencyLevel();
    }
    #endregion


}
