using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminStartManager : BaseNetworkManager {

    public GameObject settingsUI;

    public Button restartButton;
    public InputField roadLengthInput;
    public InputField screenWidthInput;
    public InputField screenHeightInput;
    public InputField marginTopInput;
    public InputField marginBottomInput;
    public InputField marginLeftInput;
    public InputField marginRightInput;

    private void Start() {
        RecoverPersistedValues();
        
    }

    private void RecoverPersistedValues()  {
        if (PlayerPrefs.HasKey(Constants.PlayerPrefsKeys.ROAD_LENGTH)) {
            roadLengthInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsKeys.ROAD_LENGTH);
        }
        if (PlayerPrefs.HasKey(Constants.PlayerPrefsKeys.SCREEN_WIDTH)) {
            screenWidthInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsKeys.SCREEN_WIDTH);
        }
        if (PlayerPrefs.HasKey(Constants.PlayerPrefsKeys.SCREEN_HEIGHT)) {
            screenHeightInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsKeys.SCREEN_HEIGHT);
        }
        if (PlayerPrefs.HasKey(Constants.PlayerPrefsKeys.MARGIN_TOP)) {
            marginTopInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsKeys.MARGIN_TOP);
        }
        if (PlayerPrefs.HasKey(Constants.PlayerPrefsKeys.MARGIN_BOTTOM)) {
            marginBottomInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsKeys.MARGIN_BOTTOM);
        }
        if (PlayerPrefs.HasKey(Constants.PlayerPrefsKeys.MARGIN_LEFT)) {
            marginLeftInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsKeys.MARGIN_LEFT);
        }
        if (PlayerPrefs.HasKey(Constants.PlayerPrefsKeys.MARGIN_RIGHT)) {
            marginRightInput.text = PlayerPrefs.GetString(Constants.PlayerPrefsKeys.MARGIN_RIGHT);
        }
    }

    public override void init() {
        if (CustomNetworkManager.isServer == true) {
            settingsUI.SetActive(true);
            restartButton.onClick.AddListener(() => onRestartGamePressed());
        }
        else {
            settingsUI.SetActive(false);
        }
    }

    public void onRestartGamePressed() {
        if (CustomNetworkManager.isConnected) {
            PersistValues();
            RestartMessage restartMessage = GetDimensionsFromUI();
            CustomNetworkManager.restartGame(restartMessage);
        }
    }

    private void PersistValues() {
        PlayerPrefs.SetString(Constants.PlayerPrefsKeys.ROAD_LENGTH, roadLengthInput.text);
        PlayerPrefs.SetString(Constants.PlayerPrefsKeys.SCREEN_WIDTH, screenWidthInput.text);
        PlayerPrefs.SetString(Constants.PlayerPrefsKeys.SCREEN_HEIGHT, screenHeightInput.text);
        PlayerPrefs.SetString(Constants.PlayerPrefsKeys.MARGIN_TOP, marginTopInput.text);
        PlayerPrefs.SetString(Constants.PlayerPrefsKeys.MARGIN_BOTTOM, marginBottomInput.text);
        PlayerPrefs.SetString(Constants.PlayerPrefsKeys.MARGIN_LEFT, marginLeftInput.text);
        PlayerPrefs.SetString(Constants.PlayerPrefsKeys.MARGIN_RIGHT, marginRightInput.text);
    }

    private RestartMessage GetDimensionsFromUI() {
        RestartMessage restartMessage = new RestartMessage();
        restartMessage.roadLength = int.Parse(roadLengthInput.text);
        restartMessage.screenWidth = float.Parse(screenWidthInput.text);
        restartMessage.screenHeight = float.Parse(screenHeightInput.text);
        restartMessage.marginTop = float.Parse(marginTopInput.text);
        restartMessage.marginBottom = float.Parse(marginBottomInput.text);
        restartMessage.marginLeft = float.Parse(marginLeftInput.text);
        restartMessage.marginRight = float.Parse(marginRightInput.text);
        return restartMessage;
    }



    public override void restartGame(RestartMessage restartMessage) {}
}
