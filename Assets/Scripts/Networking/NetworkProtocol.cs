using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkUtils : MonoBehaviour {
    public static FireProjectileMsg cFireProjectileMsg(ProjectileType type, Vector3 startPos, Vector3 dir, float speed, uint id=0) {
        return new FireProjectileMsg() {
            type = type, startPos = startPos, direction = dir, speed = speed, id = id
        };
    }


}

public class FireProjectileMsg : MessageBase {
    public ProjectileType type;
    public Vector3 startPos, direction;
    public float speed;
    public uint id;
}

public class UintMsg : MessageBase {
    public uint x;
}


public enum CustomProtocol : short{
    FireProjectile = 5000,
    DestroyProjectile = 5001,
    CameraMessage = 5002,
}

public enum ProjectileType {
    PlayerArrow
}