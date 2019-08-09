using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DimensionsManager {

    public static int roadLength = 5;

    // All measurements are in Unity units.
    // These parameters determine the aspect ratio (and how "big" the game feels). In Unity units
    public static float screenWidth = 6;
    public static float screenHeight = 4;

    // Margins, in Unity units. Must do a real-world measure to figure out what works best.
    public static float marginTop = 0.4666f;
    public static float marginBottom = 0.46666f;
    public static float marginLeft = 0.5066666f;
    public static float marginRight = 0.5066666f;


    public static float Width() { return (screenWidth + marginLeft + marginRight) * roadLength; }

    public static void UpdateFromRestartMessage(RestartMessage restartMessage) {
        roadLength = restartMessage.roadLength;
        screenWidth = restartMessage.screenWidth;
        screenHeight = restartMessage.screenHeight;
        marginTop = restartMessage.marginTop;
        marginBottom = restartMessage.marginBottom;
        marginLeft = restartMessage.marginLeft;
        marginRight = restartMessage.marginRight;
    }

    public static float Height() { return (screenHeight + marginTop + marginBottom) * 3; }
    public static Vector2 GrassDimensions() { return new Vector2(Width(), Height()); }
    public static Vector2 PathDimensions() { return new Vector2(Width(), screenHeight * 0.8f); }


    public static float TopZPosition() { return screenHeight + marginTop + marginBottom; }
    public static float BottomZPosition() { return -TopZPosition(); }

    public static Vector3 StartAndGoalDimensions() { return new Vector3(PathDimensions().y, 1, PathDimensions().y); }
    public static Vector3 StartPosition() { return new Vector3(-(Width() / 2f + StartAndGoalDimensions().x / 2f), 0, 0); }
    public static Vector3 GoalPosition() {return new Vector3(Width() / 2f + StartAndGoalDimensions().x / 2f, 0, 0);}

    // just determines how the finish graphic should look like
    public static Vector2 finishGraphicSize() {
        return new Vector2(4, 10);
    }
    public static Vector3 FinishGraphicScale() {return new Vector3(screenHeight / finishGraphicSize().y, screenHeight / finishGraphicSize().y, 1);}
    public static Vector3 FinishGraphicsPosition() { return new Vector3((Width() / 2f) - (finishGraphicSize().x / 8) - marginRight / 2, 0, 0); } // Wow my math is not strong today. This is incorrect but works...

    // Column starting from 0.
    public static float ColumnXPosition(int column) {
        int columnFromCenter = column - (roadLength / 2);
        float xPos = (marginLeft + marginRight + screenWidth) * columnFromCenter;
        return xPos;
    }

    public static List<Vector3> GetAllPositions() {
        List<Vector3> allPositions = new List<Vector3>();
        for (int column = 0; column < roadLength; column++) {
            allPositions.Add(new Vector3(ColumnXPosition(column), 0, TopZPosition()));
            allPositions.Add(new Vector3(ColumnXPosition(column), 0, 0));
            allPositions.Add(new Vector3(ColumnXPosition(column), 0, BottomZPosition()));
        }
        return allPositions;
    }

}