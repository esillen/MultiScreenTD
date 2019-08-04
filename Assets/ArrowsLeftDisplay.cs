using System.Collections.Generic;
using UnityEngine;

public class ArrowsLeftDisplay : MonoBehaviour {

    public float distanceBetweenArrows = 0.2f;
    public GameObject arrowIndicatorPrefab;

    private List<GameObject> arrows;

    private void Start() {
        arrows = new List<GameObject>();
    }

    public void DisplayArrows(int arrowsToDisplay) {
        int arrowDifference = arrowsToDisplay - arrows.Count;
        if (arrowDifference > 0) {
            AddArrows(arrowDifference);
        } else if (arrowDifference < 0) {
            RemoveArrows(Mathf.Abs(arrowDifference));
        }
    }

    private void AddArrows(int arrowsToAdd) {
        for (int i = 0; i < arrowsToAdd; i++) {
            Vector3 newPosition = transform.position + transform.right * distanceBetweenArrows * (arrows.Count + 1);
            GameObject newArrow = Instantiate(arrowIndicatorPrefab, newPosition, Quaternion.identity);
            arrows.Add(newArrow);
        }
    }

    private void RemoveArrows(int arrowsToRemove) {
        for (int i = 0; i < arrowsToRemove; i++) {
            int arrowIndex = arrows.Count - 1 - i;
            Destroy(arrows[arrowIndex]);
            arrows.RemoveAt(arrowIndex);
        }
    }

}
