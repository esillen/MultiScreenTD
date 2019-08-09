using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : BaseNetworkManager {

    public static UIManager singleton;
    private const float TOP_PANEL_MAX_Y = 345;

    public GameObject topPanelObject, botPanelObject, serverSettingsPanel;
    public RectTransform topPanelRect, botPanelRect;
    public TextMeshProUGUI currencyText;

    private TabletPosition position;
    private bool waitingToShutdownUI = false;
    private float UIShutdownTime;

    #region MiddleScreens UI
    void Update() {
        if (position != TabletPosition.Bot && position != TabletPosition.Top) {
            if (Input.GetMouseButton(0)) {
                UIShutdownTime = Time.time + 3;
                waitingToShutdownUI = true;
                enableUI();
            }
            else if(waitingToShutdownUI && Time.time > UIShutdownTime) {
                waitingToShutdownUI = false;
                disableUI();
            }
        }
    }
    #endregion

    public override void init() {
        base.init();
        singleton = this;
        if (CustomNetworkManager.isServer) {
            botPanelObject.SetActive(false);
            topPanelObject.SetActive(false);
        }
    }

    public override void restartGame(RestartMessage restartMessage) {
        if (CustomNetworkManager.isServer) {
            serverSettingsPanel.SetActive(false);
            topPanelObject.SetActive(false);
            setPosition(TabletPosition.Right);
        }
    }

    #region UI Position
    public void setPosition(TabletPosition pos) {
        print("Setting pos:" + pos);
        botPanelObject.SetActive(false);
        position = pos;

        if (pos == TabletPosition.Bot) setTopPanelUI(-TOP_PANEL_MAX_Y, 0);
        else if (pos == TabletPosition.Top) setTopPanelUI(TOP_PANEL_MAX_Y, 180);
        else disableUI();
    }

    private void setTopPanelUI(float yPos, float zRot) {
        topPanelObject.SetActive(true);
        Vector3 temp = topPanelRect.localPosition;
        temp.y = yPos;
        topPanelRect.localPosition = temp;
        topPanelRect.eulerAngles = new Vector3(0, 0, zRot);
    }

    private void disableUI() {
        serverSettingsPanel.SetActive(false);
        topPanelObject.SetActive(false);
        botPanelObject.SetActive(false);
    }

    private void enableUI() {
        if (CustomNetworkManager.isServer)
            serverSettingsPanel.SetActive(true);
        else
            botPanelObject.SetActive(true);
    }
    #endregion

    public void settingsButtonPressed() {
        topPanelObject.SetActive(false);
        botPanelObject.SetActive(true);
    }


    #region Update Text Events
    public void updateCurrency(int currency) {currencyText.text = "Gold: " + currency.ToString();}
    #endregion
}



public enum TabletPosition {
    Bot, Top, Left, Right, Middle
}