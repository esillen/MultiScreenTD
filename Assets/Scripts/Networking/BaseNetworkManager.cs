using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNetworkManager : MonoBehaviour {

    public virtual void init() { }
    public virtual void restartGame(RestartMessage restartMessage) { }
}
