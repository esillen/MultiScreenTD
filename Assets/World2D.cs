using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World2D : MonoBehaviour {

    public SpriteRenderer grass;
    public SpriteRenderer path;
    public SpriteRenderer finishGraphic;
    public GameObject start;
    public GameObject goal;
    public DimensionsManager dimensionsManager;
    public GameObject towerPrefab;

    private List<GameObject> towers;

    private void Start()
    {
        towers = new List<GameObject>();
        UpdateDimensions();
    }

    public void UpdateDimensions() {

        grass.size = dimensionsManager.GrassDimensions;
        path.size = dimensionsManager.PathDimensions;
        start.transform.localScale = dimensionsManager.StartAndGoalDimensions;
        start.transform.position = dimensionsManager.StartPosition;
        goal.transform.localScale = dimensionsManager.StartAndGoalDimensions;
        goal.transform.position = dimensionsManager.GoalPosition;
        finishGraphic.transform.position = dimensionsManager.FinishGraphicsPosition;
        finishGraphic.transform.localScale = dimensionsManager.FinishGraphicScale;
        finishGraphic.size = dimensionsManager.finishGraphicSize;
        SpawnTowers();
    }

    private void SpawnTowers()
    {
        // Destroy towers
        foreach (GameObject tower in towers)
        {
            Destroy(tower);
        }
        towers.Clear();
        for (int i = 0; i < dimensionsManager.roadLength; i++)
        {
            float columnXPosition = dimensionsManager.ColumnXPosition(i);

            // Spawn top towers
            Vector3 topTowerPosition = new Vector3(columnXPosition, 0, dimensionsManager.TopZPosition);
            GameObject spawnedTopTower = Instantiate(towerPrefab, topTowerPosition, Quaternion.identity);
            towers.Add(spawnedTopTower);

            // Spawn bottom towers
            Vector3 bottomTowerPosition = new Vector3(columnXPosition, 0, dimensionsManager.BottomZPosition);
            GameObject spawnedBottomTower = Instantiate(towerPrefab, bottomTowerPosition, Quaternion.identity);
            towers.Add(spawnedBottomTower);
        }
    }
}
