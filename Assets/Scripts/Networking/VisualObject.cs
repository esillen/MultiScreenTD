using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualObject : MonoBehaviour {

    public float speed;
    public uint id;

    public virtual void init(float speed, uint id) {
        this.speed = speed; this.id = id;
    }

    public abstract void destroyVisualObject();
}
