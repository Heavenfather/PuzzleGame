using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMgr
{
    private static LevelMgr _instance;

    public static LevelMgr GetInstance()
    {
        if (_instance == null)
            _instance = new LevelMgr();
        return _instance;
    }

    public Vector2 GetGridCount(int level)
    {
        Vector2 grid = Vector2.one;
        switch (level)
        {
            case 0:
                grid.x = 3;
                grid.y = 3;
                break;
            case 1:
                grid.x = 3;
                grid.y = 4;
                break;
            case 2:
            case 3:
                grid.x = 4;
                grid.y = 4;
                break;
            case 4:
            case 5:
                grid.x = 4;
                grid.y = 5;
                break;
            case 6:
            case 7:
                grid.x = 5;
                grid.y = 5;
                break;
            case 8:
            case 9:
                grid.x = 6;
                grid.y = 5;
                break;
            case 10:
                grid.x = 6;
                grid.y = 6;
                break;
            case 11:
            case 12:
                grid.x = 6;
                grid.y = 7;
                break;
            case 13:
            case 14:
                grid.x = 7;
                grid.y = 7;
                break;
        }
        return grid;
    }

    public Sprite[] GetSprites(int level)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Levels/" + level);
        return sprites;
    }

    public Texture2D GetLevelTexture(int level)
    {
        Texture2D texture = Resources.Load<Texture2D>("Levels/" + level);
        return texture;
    }

}
