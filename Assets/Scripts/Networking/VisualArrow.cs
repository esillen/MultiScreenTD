﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualArrow : VisualObject {

    public override void destroyVisualObject() {
        Destroy(this.gameObject);
    }

}
