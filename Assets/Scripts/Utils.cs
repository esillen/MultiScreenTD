using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

	public static void destroySpawnedItemsAndClearDictionary(Dictionary<uint, GameObject> spawnedObjects) {
        foreach (uint id in spawnedObjects.Keys)
            Destroy(spawnedObjects[id].gameObject);
        spawnedObjects.Clear();
    }
}
