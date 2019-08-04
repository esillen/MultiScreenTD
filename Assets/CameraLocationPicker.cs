using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLocationPicker : MonoBehaviour {

    public List<Transform> cameraLocations;
    public List<Button> buttons;

    private bool currentlyMoving = false;
    private Vector3 targetLocation;

    private void Start() {
        targetLocation = Camera.main.transform.position;
        if (buttons.Count != cameraLocations.Count) {
            Debug.LogError("Important! there should be as many buttons as camera locations!");
        } else {
            for (int i = 0; i < buttons.Count; i++) {
                SetActionForButton(buttons[i], cameraLocations[i].position);
            }
        }
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
