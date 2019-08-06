using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLocationPicker : MonoBehaviour {

    public List<GameObject> buttons;
    public Transform buttonsParent;
    public GameObject buttonPrefab;

    private Vector3 targetLocation = Vector3.up * 10;

    public void CreateNewLocationsAndButtons(List<Vector3> newPositions) {
        foreach (GameObject button in buttons) {
            Destroy(button);
        }
        buttons.Clear();

        foreach (Vector3 newPosition in newPositions) {
            GameObject cameraLocation = new GameObject("Camera Location");
            GameObject buttonGameObject = Instantiate(buttonPrefab, buttonsParent);
            Button button = buttonGameObject.GetComponent<Button>();
            SetActionForButton(button, newPosition + Vector3.up * 10);
            buttons.Add(buttonGameObject);
        }
    }

    private void Start() {
        buttons = new List<GameObject>();
    }

    private void SetActionForButton(Button button, Vector3 newCameraLocation) {
        button.onClick.AddListener(() => targetLocation = newCameraLocation);
    }

    private void Update() {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetLocation, 0.999f * Time.deltaTime);
        if ((Camera.main.transform.position - targetLocation).magnitude < 0.01f) {
            Camera.main.transform.position = targetLocation;
        }
    }

}
