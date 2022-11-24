using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CacheMgr
{
    private static CacheMgr _instance;
    public static CacheMgr GetInstance()
    {
        if (_instance == null)
            _instance = new CacheMgr();
        return _instance;
    }

    private int _curLevel = 0;
    private Dictionary<int, CacheData> _cacheData = new Dictionary<int, CacheData>();
    private string _saveKey = "PuzzleGameCache";
    private string _currentLevelKey = "PuzzleGameLevelKey";

    public int CurLevel
    {
        get
        {
            _curLevel = PlayerPrefs.GetInt(_currentLevelKey, 0);
            return _curLevel;
        }
        set
        {
            _curLevel = value;
            if (_curLevel >= LevelMgr.GetInstance().GetLevelCount())
                _curLevel = LevelMgr.GetInstance().GetLevelCount() - 1;
            PlayerPrefs.SetInt(_currentLevelKey, _curLevel);
        }
    }

    public void InitCache()
    {
        string value = PlayerPrefs.GetString(_saveKey, "");
        if (!string.IsNullOrEmpty(value))
        {
            //level_isPass_time   isPass:0没通关 1通关
            string[] datas = value.Split(';');
            for (int i = 0; i < datas.Length; i++)
            {
                string data = datas[i];
                string[] cacheStr = data.Split('_');
                CacheData cacheData = new CacheData();
                cacheData.level = int.Parse(cacheStr[0]);
                cacheData.pass = cacheStr[1] == "1";
                cacheData.passTime = int.Parse(cacheStr[2]);
                _cacheData.Add(cacheData.level, cacheData);
            }
        }
    }

    public void Wirte(int level, bool isPass, int time)
    {
        if (_cacheData.ContainsKey(level))
        {
            _cacheData[level].pass = isPass;
            _cacheData[level].passTime = time;
        }
        else
        {
            CacheData data = new CacheData();
            data.level = level;
            data.pass = isPass;
            data.passTime = time;
            _cacheData.Add(level, data);
        }
        SaveToLocal();
    }

    public CacheData GetCache(int level)
    {
        if (_cacheData.ContainsKey(level))
            return _cacheData[level];
        return null;
    }

    public void SaveToLocal()
    {
        List<string> datas = new List<string>();
        foreach (var item in _cacheData)
        {
            string data = string.Format("{0}_{1}_{2}", item.Key, item.Value.pass == true ? 1 : 0, item.Value.passTime);
            datas.Add(data);
        }
        string saveStr = string.Join(";", datas.ToArray());
        PlayerPrefs.SetString(_saveKey, saveStr);
    }

    public void Reset()
    {
        _cacheData.Clear();
        CurLevel = 0;
        PlayerPrefs.SetString(_saveKey,"");
    }

}

public class CacheData
{
    public int level;
    public bool pass;
    public int passTime;

    public string ToShowTime()
    {
        TimeSpan ts = new TimeSpan(0, 0, passTime);
        string result = "";
        if (ts.Hours > 0)
        {
            result = string.Format("{0}时{1}分{2}秒", ts.Hours, ts.Minutes, ts.Seconds.ToString("00"));
        }
        else if (ts.Hours == 0 && ts.Minutes > 0)
        {
            result = string.Format("{0}分{1}秒", ts.Minutes, ts.Seconds.ToString("00"));
        }
        else
        {
            result = string.Format("{0}秒", ts.Seconds.ToString("00"));
        }

        return result;
    }
}
