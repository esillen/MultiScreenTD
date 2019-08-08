using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualArrow : VisualObject {

    // Update is called once per frame
    void Update () {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    public override void destroyVisualObject() {
        Destroy(this.gameObject);
    }

}
