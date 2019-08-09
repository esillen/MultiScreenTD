using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ProjectileBase {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Constants.Tags.ENEMY)) {
            other.GetComponent<Enemy>().TakeDamage(details.dmg);
            destroyVisualObject();
        }
    }

    public override void destroyVisualObject() {
        destroyProjectileOnClients();
        Destroy(gameObject);
    }
}
