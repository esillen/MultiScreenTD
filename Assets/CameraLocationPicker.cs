using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLocationPicker : BaseNetworkManager {

    public List<GameObject> buttons = new List<GameObject>();
    public Transform buttonsParent;
    public GameObject buttonPrefab;

    private TabletPosition[] tabletPositions;
    private Vector3 targetLocation = Vector3.up * 10;

    public override void init() {}
    public override void restartGame(RestartMessage restartMessage) { }

    private void Update() {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetLocation, 0.999f * Time.deltaTime);
        if ((Camera.main.transform.position - targetLocation).magnitude < 0.01f) {
            Camera.main.transform.position = targetLocation;
        }
    }

    public void CreateNewLocationsAndButtons(List<Vector3> newPositions) {
        foreach (GameObject button in buttons)
            Destroy(button);
        buttons.Clear();

        tabletPositions = new TabletPosition[newPositions.Count];
        for(int i = 0; i < newPositions.Count; i++) {
            if (i % 3 == 0)tabletPositions[i] = TabletPosition.Top;
            if (i % 3 == 1)tabletPositions[i] = TabletPosition.Middle;
            if (i % 3 == 2)tabletPositions[i] = TabletPosition.Bot;
        }
        //Hardcode Left & Right
        tabletPositions[1] = TabletPosition.Left;
        tabletPositions[tabletPositions.Length - 2] = TabletPosition.Left;

        for(int i = 0; i < newPositions.Count; i++){
            GameObject cameraLocation = new GameObject("Camera Location");
            GameObject buttonGameObject = Instantiate(buttonPrefab, buttonsParent);
            Button button = buttonGameObject.GetComponent<Button>();
            SetActionForButton(button, newPositions[i] + Vector3.up * 10, tabletPositions[i], i);
            buttons.Add(buttonGameObject);
        }
    }


    private void SetActionForButton(Button button, Vector3 newCameraLocation, TabletPosition tabPos, int id) {
        button.onClick.AddListener(() => {
            targetLocation = newCameraLocation;
            UIManager.singleton.setPosition(tabPos);
            if(tabPos == TabletPosition.Bot || tabPos == TabletPosition.Top)
                NetworkEconomySystem.setCurrentTower(World2D.getTowerFromID(id));
            });
    }



}
