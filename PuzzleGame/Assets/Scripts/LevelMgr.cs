using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class LevelMgr
{
    private static LevelMgr _instance;

    public static LevelMgr GetInstance()
    {
        if (_instance == null)
            _instance = new LevelMgr();
        return _instance;
    }

    public ChallengeMode CurChallengeMode;

    private Dictionary<string, LevelData> _levelDatas = new Dictionary<string, LevelData>();

    public void InitConfig()
    {
        string path = "Levels/LevelConfig";
        string levelJson = Resources.Load<TextAsset>(path).text;
        _levelDatas = JsonMapper.ToObject<Dictionary<string, LevelData>>(levelJson);
    }

    public Vector2 GetGridCount(int level)
    {
        Vector2 grid = Vector2.one;
        LevelData data = _levelDatas[level.ToString()];
        grid.x = data.col;
        grid.y = data.row;
        return grid;
    }

    public int GetLevelCount()
    {
        return _levelDatas.Count;
    }

    public int GetLimitTime(int level)
    {
        int time = 0;
        LevelData data = _levelDatas[level.ToString()];
        time = data.limitTime;
        return time;
    }

    public Sprite[] GetSprites(int level)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Levels/" + level);
        return sprites;
    }

    public Texture2D GetTexture(int level)
    {
        Texture2D texture = Resources.Load<Texture2D>("Levels/" + level + "/" + level);
        return texture;
    }

    public List<Texture2D> GetAllLevelTexture()
    {
        List<Texture2D> l = new List<Texture2D>();

        int count = _levelDatas.Count;
        for (int i = 0; i < count; i++)
        {
            l.Add(GetTexture(i));
        }

        return l;
    }

    public bool IsCanChallenge(int level)
    {
        if (level == 0)
            return true;
        CacheData data = CacheMgr.GetInstance().GetCache(level - 1);
        if (data != null && data.pass)
            return true;
        return false; ;
    }

}

public class LevelData
{
    public int col;
    public int row;
    public int limitTime;
}

public enum ChallengeMode
{
    Time,
    Challenge
}
