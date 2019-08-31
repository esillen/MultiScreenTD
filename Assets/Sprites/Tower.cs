using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Tower : NetworkBehaviour {

    #region Tower Stats
    public int damage = 5;
    public int maxArrows = 5;
    public float range = 2;
    public float speed = 5;
    public float arrowRespawnRate = 5;
    #endregion

    public Transform arrowOrigin;
    public GameObject arrowPrefab;
    public ArrowsLeftDisplay arrowsLeftDisplay;

    private int numArrowsLeft;

    private void Start() {
        damage = DifficultySettings.startTowerDamage;
        maxArrows = DifficultySettings.startMaxArrows;
        range = DifficultySettings.startRange;
        speed = DifficultySettings.startSpeed;
        arrowRespawnRate = DifficultySettings.startArrowRespawnRate;

        numArrowsLeft = maxArrows;
        StartCoroutine(RegainArrow());
    }

    private void Update() {
        arrowsLeftDisplay.DisplayArrows(numArrowsLeft);
    }

    public void Fire(Vector3 towards) {
        if (CustomNetworkManager.isServer)
            return;

        if (numArrowsLeft > 0) {
            Vector3 towardsFromTowerNonFlat = towards - arrowOrigin.position;
            Vector3 towardsFromTower = new Vector3(towardsFromTowerNonFlat.x, 0, towardsFromTowerNonFlat.z); // Remove y-component
            Vector3 eulerDir = Quaternion.LookRotation(towardsFromTower).eulerAngles;

            // Send msg to server that we wish to fire an projectile and await broadcast from server before we spawn local copy
            Color objColor = calculateArrowColor();
            Vector3 arrowScale = calculateArrowScale();
            ProjectileDetails pDetails = new ProjectileDetails() { dmg = damage, range = range };
            FireProjectileMsg msg = NetworkUtils.cFireProjectileMsg(ProjectileType.PlayerArrow, pDetails, arrowOrigin.position, eulerDir, arrowScale, speed, objColor, 8);
            NetworkManager.singleton.client.Send((short)CustomProtocol.FireProjectile, msg);

            numArrowsLeft -= 1;
        }
    }

    private Color calculateArrowColor() {
        int upgradeLevel = damage - DifficultySettings.startTowerDamage;
        if (upgradeLevel < 5)
            return new Color(1 - 0.2f * upgradeLevel, 1, 1, 1);
        else if (upgradeLevel < 10)
            return new Color(0, 1 - 0.2f * upgradeLevel, 1, 1);
        else if (upgradeLevel < 15)
            return new Color(0, 0, 1 - 0.2f * upgradeLevel, 1);
        else if (upgradeLevel < 20)
            return new Color(0.2f * upgradeLevel, 0, 0, 1);
        else if (upgradeLevel < 25)
            return new Color(1, 0.2f * upgradeLevel, 0, 1);
        else
            return new Color(1, 1, 0, 1);
    }

    private Vector3 calculateArrowScale() {
        return Vector3.one * (1 + 0.1f * (damage - DifficultySettings.startTowerDamage));
    }

    private IEnumerator RegainArrow() {
        yield return new WaitForSeconds(arrowRespawnRate);
        if (numArrowsLeft < maxArrows)
            numArrowsLeft += 1;
        StartCoroutine(RegainArrow());
    }

}
