using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkUtils : MonoBehaviour {
    public static FireProjectileMsg cFireProjectileMsg(ProjectileType type, ProjectileDetails projDetails, Transform trans, float speed, Color color, uint id=0) {
        return cFireProjectileMsg(type, projDetails, trans.position, trans.localEulerAngles, trans.localScale, speed, color, id);
    }

    public static FireProjectileMsg cFireProjectileMsg(ProjectileType type, ProjectileDetails projDetails, Vector3 pos, Vector3 rot, Vector3 scale, float speed, Color color, uint id = 0) {
        return new FireProjectileMsg() {
            type = type, objDetails = new SpawnedObject() {
                pos = pos, rot = rot, scale = scale, speed = speed, id = id, color = color
            },
            projDetails = projDetails
        };
    }

    public static SpawnEnemyMsg cSpawnEnemyMsg(EnemyType type, Transform trans, float speed, uint id=0){
        return cSpawnEnemyMsg(type, trans.position, trans.localEulerAngles, trans.localScale, speed, id);
    }

    public static SpawnEnemyMsg cSpawnEnemyMsg(EnemyType type, Vector3 pos, Vector3 rot, Vector3 scale, float speed, uint id = 0) {
        return new SpawnEnemyMsg() {
            type = type, details = new SpawnedObject() {
                pos = pos, rot = rot, scale = scale, speed = speed, id = id
            }
        };
    }
}


#region Messages
public class FireProjectileMsg : MessageBase {
    public ProjectileType type;
    public SpawnedObject objDetails;
    public ProjectileDetails projDetails;
}

public class SpawnEnemyMsg : MessageBase {
    public EnemyType type;
    public SpawnedObject details;
}

public class TradeMsg : MessageBase {
    public PurchaseType type;
    public int cost;
}

public class EffectMsg : MessageBase {
    public EffectType type;
    public string textData;
    public Vector3 position;
}

public class UintMsg : MessageBase { public uint x; }

public class RestartMessage : MessageBase {
    public int roadLength;
    public float screenWidth;
    public float screenHeight;
    public float marginTop;
    public float marginBottom;
    public float marginLeft;
    public float marginRight;
}

#endregion

#region Structs
public struct PlayerID {public int col, pos;}
public struct SpawnedObject {
    public uint id;
    public float speed;
    public Vector3 pos, rot, scale;
    public Color color;
}
public struct ProjectileDetails {
    public float range;
    public int dmg;
}
#endregion

#region Enums
public enum PurchaseType {
    Damage, Cooldown, Range, Magazine
}

public enum CustomProtocol : short{
    FireProjectile = 5000,
    DestroyProjectile = 5001,
    CameraMessage = 5002,
    CurrencyMessage = 5003,
    EffectsMessage = 5004,
    SpawnEnemyMsg = 5005,
    DestroyEnemyMsg = 5006,
    RestartGame = 5007,
    PurchuaseMsg = 5008,
}

public enum ProjectileType {
    PlayerArrow
}

public enum EffectType {
    FloatingText
}

public enum EnemyType {
    Soldier
}

#endregion