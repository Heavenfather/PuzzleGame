using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PreviewCell : MonoBehaviour
{
    [SerializeField]
    RawImage img;
    [SerializeField]
    GameObject noPassMask;
    [SerializeField]
    Text passTime;
    [SerializeField]
    Button btn;
    [SerializeField]
    GameObject selected;

    private int _level;
    private MainUI _mainUI;

    void Start()
    {
        btn.onClick.AddListener(OnLevelClick);
    }

    private void OnLevelClick()
    {
        if (!LevelMgr.GetInstance().IsCanChallenge(_level))
            return;
        CacheMgr.GetInstance().CurLevel = _level;
        if (_mainUI != null)
        {
            _mainUI.OnCellClick(this);
        }
    }

    public void SetSelected(bool b)
    {
        selected.SetActive(b);
    }

    public void Refresh(int level, MainUI parent)
    {
        _level = level;
        _mainUI = parent;
        img.texture = LevelMgr.GetInstance().GetTexture(level);
        CacheData data = CacheMgr.GetInstance().GetCache(level);
        noPassMask.SetActive(!LevelMgr.GetInstance().IsCanChallenge(level));
        if (data != null && data.pass)
        {
            passTime.gameObject.SetActive(true);
            passTime.text = "用时:" + data.ToShowTime();
        }
        else
        {
            passTime.gameObject.SetActive(false);
        }
    }

}
