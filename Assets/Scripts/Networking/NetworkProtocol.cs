using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkUtils : MonoBehaviour {
    public static FireProjectileMsg cFireProjectileMsg(ProjectileType type, Vector3 startPos, Vector3 dir, float speed, uint id=0) {
        return new FireProjectileMsg() {
            type = type, details = new SpawnedObject() {
                pos = startPos, dir = dir, speed = speed, id = id
            }
        };
    }

    public static SpawnEnemyMsg cSpawnEnemyMsg(EnemyType type, Vector3 position, Vector3 direction, float speed, uint id=0){
        return new SpawnEnemyMsg() {
            type = type, details = new SpawnedObject() {
                pos = position, dir = direction, speed = speed, id = id
            }
        };
    }
}


#region Messages
public class FireProjectileMsg : MessageBase {
    public ProjectileType type;
    public SpawnedObject details;
}


public class SpawnEnemyMsg : MessageBase {
    public EnemyType type;
    public SpawnedObject details;
}

public class TradeMsg : MessageBase {
    public PurchaseType type;
    public PlayerID id;
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
    public Vector3 pos, dir;
}
#endregion

#region Enums
public enum PurchaseType {
    ArrowTower, 
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