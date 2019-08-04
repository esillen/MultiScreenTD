using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInputController : MonoBehaviour {

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.CompareTag(Constants.Tags.TOWER_AIM_ARCH)) {
                    hit.collider.gameObject.GetComponent<AimArch>().tower.Fire(hit.point);
                }
            }

        }
    }

}
