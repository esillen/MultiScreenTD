using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : VisualObject {

    public ProjectileDetails details;

    public virtual void initProjectile(SpawnedObject objDetails, ProjectileDetails projDetails) {
        base.init(objDetails);
        details = projDetails;
        StartCoroutine(rangeTime(projDetails.range));
    }

    IEnumerator rangeTime(float range) {
        yield return new WaitForSeconds(range);
        destroyProjectileOnClients();
    }

    protected void destroyProjectileOnClients() {
        ServerProjectileManager.onProjectileDestroyed(id);
    }
}
