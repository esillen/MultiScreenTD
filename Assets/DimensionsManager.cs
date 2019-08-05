using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionsManager : MonoBehaviour {

    public int roadLength = 5;

    // All measurements are in Unity units.
    // These parameters determine the aspect ratio (and how "big" the game feels). In Unity units
    private const float screenWidth = 6;
    private const float screenHeight = 4;

    // Margins, in Unity units. Must do a real-world measure to figure out what works best.
    private float marginTop = 0.1f;
    private float marginBottom = 0.1f;
    private float marginLeft = 0.1f;
    private float marginRight = 0.1f;


    public float Width => (screenWidth + marginLeft + marginRight) * roadLength;
    public float Height => (screenHeight + marginTop + marginBottom) * 3;
    public Vector2 GrassDimensions => new Vector2(Width, Height);
    public Vector2 PathDimensions => new Vector2(Width, Height / 3f);

    // Column starting from 0.
    public float ColumnXPosition(int column) {
        int columnFromCenter = column - (roadLength / 2);
        float xPos = (marginLeft + marginRight + screenWidth) * columnFromCenter;
        return xPos;
    }

    public float TopZPosition => screenHeight + marginTop + marginBottom;
    public float BottomZPosition => -TopZPosition;

    public Vector3 StartAndGoalDimensions => new Vector3(Height / 3f, 1, Height / 3f);
    public Vector3 StartPosition => new Vector3(-(Width / 2f + StartAndGoalDimensions.x / 2f), 0, 0);
    public Vector3 GoalPosition => new Vector3(Width / 2f + StartAndGoalDimensions.x / 2f, 0, 0);

    // just determines how the finish graphic should look like
    public Vector2 finishGraphicSize => new Vector2(4, 10);
    public Vector3 FinishGraphicScale => new Vector3(screenHeight / finishGraphicSize.y, screenHeight / finishGraphicSize.y, 1);
    public Vector3 FinishGraphicsPosition => new Vector3((Width / 2f) - (finishGraphicSize.x / 4) - marginRight, 0, 0);
    
}
