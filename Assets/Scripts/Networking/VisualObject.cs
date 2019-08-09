using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualObject : MonoBehaviour {

    public SpriteRenderer theSprite;
    public float speed;
    public uint id;

    void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    public virtual void init(SpawnedObject details) {
        this.speed = details.speed; this.id = details.id;
        transform.position = details.pos;
        transform.localScale = details.scale;
        transform.eulerAngles = details.rot;

        if (theSprite != null)
            theSprite.color = details.color;
    }


    public abstract void destroyVisualObject();
}
