using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualArrow : VisualProjectile {

    // Update is called once per frame
    void Update () {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    public override void destroyProjectile() {
        Destroy(this.gameObject);
    }

}
