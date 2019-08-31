using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ProjectileBase {

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Constants.Tags.ENEMY)) {
            Enemy theEnemy = other.GetComponent<Enemy>();
            int enemyStartHP = theEnemy.health;
            theEnemy.TakeDamage(details.dmg);

            details.dmg -= enemyStartHP;
            if(details.dmg <= 0)
                destroyVisualObject();
        }
    }

    public override void destroyVisualObject() {
        destroyProjectileOnClients();
        Destroy(gameObject);
    }
}
