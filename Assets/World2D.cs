using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World2D : BaseNetworkManager {

    private static World2D singleton;

    public SpriteRenderer grass;
    public SpriteRenderer path;
    public SpriteRenderer finishGraphic;
    public GameObject start;
    public GameObject goal;
    public CameraLocationPicker cameraLocationPicker;
    public GameObject towerPrefab;

    private List<GameObject> towers = new List<GameObject>();
    private Dictionary<int, Tower> idToTower = new Dictionary<int, Tower>();

    public override void init() {
        singleton = this;
    }
    public override void restartGame(RestartMessage restartMessage) {
        // Set correct values for the dimension manager from the restart message.
        DimensionsManager.UpdateFromRestartMessage(restartMessage);
        UpdateDimensions();
    }

    public void UpdateDimensions() {
        grass.size = DimensionsManager.GrassDimensions();
        path.size = DimensionsManager.PathDimensions();
        start.transform.localScale = DimensionsManager.StartAndGoalDimensions();
        start.transform.position = DimensionsManager.StartPosition();
        goal.transform.localScale = DimensionsManager.StartAndGoalDimensions();
        goal.transform.position = DimensionsManager.GoalPosition();
        finishGraphic.transform.position = DimensionsManager.FinishGraphicsPosition();
        finishGraphic.transform.localScale = DimensionsManager.FinishGraphicScale();
        finishGraphic.size = DimensionsManager.finishGraphicSize();
        SpawnTowers();
        cameraLocationPicker.CreateNewLocationsAndButtons(DimensionsManager.GetAllPositions());
    }

    private void SpawnTowers(){
        // Destroy towers
        foreach (GameObject tower in towers)
            Destroy(tower);
        towers.Clear();
        idToTower.Clear();

        for (int i = 0; i < DimensionsManager.roadLength; i++){
            float columnXPosition = DimensionsManager.ColumnXPosition(i);

            // Spawn top towers
            Vector3 topTowerPosition = new Vector3(columnXPosition, 0, DimensionsManager.TopZPosition());
            GameObject spawnedTopTower = Instantiate(towerPrefab, topTowerPosition, Quaternion.Euler(0, 180, 0));
            idToTower.Add(i * 3, spawnedTopTower.GetComponent<Tower>()); // Quick hack 
            towers.Add(spawnedTopTower);

            // Spawn bottom towers
            Vector3 bottomTowerPosition = new Vector3(columnXPosition, 0, DimensionsManager.BottomZPosition());
            GameObject spawnedBottomTower = Instantiate(towerPrefab, bottomTowerPosition, Quaternion.identity);
            idToTower.Add(i * 3 + 2, spawnedTopTower.GetComponent<Tower>()); // Quick hack 
            towers.Add(spawnedBottomTower);
        }
    }

    public static Tower getTowerFromID(int id) {return singleton.idToTower[id];}
}
