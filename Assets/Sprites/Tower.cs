using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public int maxArrows = 5;
    public float arrowRespawnRate = 5;
    public Transform arrowOrigin;
    public GameObject arrowPrefab;
    public ArrowsLeftDisplay arrowsLeftDisplay;

    private int numArrowsLeft;

    private void Start() {
        numArrowsLeft = maxArrows;
        StartCoroutine(RegainArrow());
    }

    private void Update() {
        arrowsLeftDisplay.DisplayArrows(numArrowsLeft);
    }

    public void Fire(Vector3 towards) {
        if (numArrowsLeft > 0) {
            Vector3 towardsFromTowerNonFlat = towards - arrowOrigin.position;
            Vector3 towardsFromTower = new Vector3(towardsFromTowerNonFlat.x, 0, towardsFromTowerNonFlat.z); // Remove y-component
            Instantiate(arrowPrefab, arrowOrigin.position, Quaternion.LookRotation(towardsFromTower));
            numArrowsLeft -= 1;
        }
    }

    private IEnumerator RegainArrow() {
        yield return new WaitForSeconds(arrowRespawnRate);
        if (numArrowsLeft < maxArrows) {
            numArrowsLeft += 1;
        }
        StartCoroutine(RegainArrow());
    }

}
