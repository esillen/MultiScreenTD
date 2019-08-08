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


#region Messages
public class FireProjectileMsg : MessageBase {
    public ProjectileType type;
    public Vector3 startPos, direction;
    public float speed;
    public uint id;
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
#endregion

#region Structs
public struct PlayerID {public int col, pos;}
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
}

public enum ProjectileType {
    PlayerArrow
}

public enum EffectType {
    FloatingText
}
#endregion