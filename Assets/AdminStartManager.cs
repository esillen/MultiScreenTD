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
            RestartMessage restartMessage = GetDimensionsFromUI();
            CustomNetworkManager.restartGame(restartMessage);
        }
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
