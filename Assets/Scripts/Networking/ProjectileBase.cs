using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour {

    public uint id;

    public void init(uint id) {
        this.id = id;
    }

    protected void destroyProjectileOnClients() {
        ServerFireManager.onProjectileDestroyed(id);
    }
}
