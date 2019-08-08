using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World2D : BaseNetworkManager {

    public SpriteRenderer grass;
    public SpriteRenderer path;
    public SpriteRenderer finishGraphic;
    public GameObject start;
    public GameObject goal;
    public DimensionsManager dimensionsManager;
    public CameraLocationPicker cameraLocationPicker;
    public GameObject towerPrefab;

    private List<GameObject> towers = new List<GameObject>();

    public override void init() {}
    public override void restartGame(){UpdateDimensions();}

    public void UpdateDimensions() {
        grass.size = dimensionsManager.GrassDimensions();
        path.size = dimensionsManager.PathDimensions();
        start.transform.localScale = dimensionsManager.StartAndGoalDimensions();
        start.transform.position = dimensionsManager.StartPosition();
        goal.transform.localScale = dimensionsManager.StartAndGoalDimensions();
        goal.transform.position = dimensionsManager.GoalPosition();
        finishGraphic.transform.position = dimensionsManager.FinishGraphicsPosition();
        finishGraphic.transform.localScale = dimensionsManager.FinishGraphicScale();
        finishGraphic.size = dimensionsManager.finishGraphicSize();
        SpawnTowers();
        cameraLocationPicker.CreateNewLocationsAndButtons(dimensionsManager.GetAllPositions());
    }

    private void SpawnTowers(){
        // Destroy towers
        foreach (GameObject tower in towers)
            Destroy(tower);
        towers.Clear();

        for (int i = 0; i < dimensionsManager.roadLength; i++){
            float columnXPosition = dimensionsManager.ColumnXPosition(i);

            // Spawn top towers
            Vector3 topTowerPosition = new Vector3(columnXPosition, 0, dimensionsManager.TopZPosition());
            GameObject spawnedTopTower = Instantiate(towerPrefab, topTowerPosition, Quaternion.Euler(0, 180, 0));
            towers.Add(spawnedTopTower);

            // Spawn bottom towers
            Vector3 bottomTowerPosition = new Vector3(columnXPosition, 0, dimensionsManager.BottomZPosition());
            GameObject spawnedBottomTower = Instantiate(towerPrefab, bottomTowerPosition, Quaternion.identity);
            towers.Add(spawnedBottomTower);
        }
    }
}
