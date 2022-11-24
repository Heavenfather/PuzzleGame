using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityTimer;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    Transform parent;
    [SerializeField]
    RawImage originalImg;
    [SerializeField]
    GridLayoutGroup layout;
    [SerializeField]
    Cell cell;
    [SerializeField]
    Text curLevelText;
    [SerializeField]
    GameObject resultPanel;
    [SerializeField]
    Button btnNext;
    [SerializeField]
    Button btnReturn;
    [SerializeField]
    Button btnReturn2;
    [SerializeField]
    Button btnReplay;
    [SerializeField]
    Text resultText;
    [SerializeField]
    Text timeText;

    private Dictionary<int, Cell> _allCellsDic = new Dictionary<int, Cell>();
    private List<int> _selectCellIds = new List<int>();
    private ChallengeMode _curMode;
    private Timer _timer;
    private int _totalTime;

    void Awake()
    {
        _curMode = LevelMgr.GetInstance().CurChallengeMode;

        btnNext.onClick.AddListener(OnNextLevelClick);
        btnReturn.onClick.AddListener(OnReturnClick);
        btnReturn2.onClick.AddListener(OnReturnClick);
        btnReplay.onClick.AddListener(OnReplayClick);
    }

    void OnTimerUpdate()
    {
        if (_curMode == ChallengeMode.Time)
        {
            _totalTime++;
            timeText.text = GetTimeModeStr();
        }
        else
        {
            _totalTime--;
            if (_totalTime <= 0)
            {
                //失败
                Timer.Pause(_timer);
                resultPanel.SetActive(true);
                btnNext.gameObject.SetActive(false);
                btnReplay.gameObject.SetActive(true);
                resultText.text = "失败";
            }
            else
            {
                timeText.text = GetChallengeModeStr();
            }
        }

    }

    void Start()
    {
        Clear();
        int level = CacheMgr.GetInstance().CurLevel;
        curLevelText.text = "当前关卡:" + (level + 1);
        if (_curMode == ChallengeMode.Time)
        {
            _totalTime = 0;
        }
        else
        {
            _totalTime = LevelMgr.GetInstance().GetLimitTime(level);
        }

        if (_timer == null)
        {
            _timer = Timer.Register(1.0f, OnTimerUpdate, null, true, false, this);
        }
        else
        {
            Timer.Resume(_timer);
        }

        Sprite[] sprites = LevelMgr.GetInstance().GetSprites(level);
        System.Random rd = new System.Random();
        int randomIndex = 0;
        Sprite temp;
        for (int i = 0; i < sprites.Length; i++)
        {
            randomIndex = rd.Next(0, sprites.Length - 1);
            if (randomIndex != i)
            {
                temp = sprites[i];
                sprites[i] = sprites[randomIndex];
                sprites[randomIndex] = temp;
            }
        }

        Texture2D texture = LevelMgr.GetInstance().GetTexture(level);
        originalImg.texture = texture;
        Vector2 grid = LevelMgr.GetInstance().GetGridCount(level);
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = (int)grid.y;
        int index = 0;
        for (int i = 0; i < grid.x; i++)
        {
            for (int j = 0; j < grid.y; j++)
            {
                GameObject go = Instantiate(cell.gameObject, parent);
                go.SetActive(true);
                Cell cellComp = go.GetComponent<Cell>();
                CellData data = new CellData();
                data.sprite = sprites[index];
                data.SetPos(level, index);
                data.id = index;
                cellComp.SetCell(data, this);
                _allCellsDic.Add(index, cellComp);
                index++;
            }
        }

    }

    public void OnCellClick(Cell cell)
    {
        if (_selectCellIds.Count < 2)
        {
            if (_selectCellIds.Contains(cell.GetData().id))
                _selectCellIds.Remove(cell.GetData().id);
            else
                _selectCellIds.Add(cell.GetData().id);
        }
        foreach (var item in _allCellsDic)
        {
            item.Value.SetSelect(_selectCellIds.Contains(item.Key));
        }
        if (_selectCellIds.Count == 2)
        {
            ReplaceSelect();
            _selectCellIds.Clear();
        }

        if (IsWin())
        {
            Timer.Pause(_timer);
            resultPanel.SetActive(true);
            resultText.text = "恭喜通关!!!";
            btnReplay.gameObject.SetActive(false);
            btnNext.gameObject.SetActive(true);

            //save to local;
            int saveTime = _totalTime;
            if (_curMode == ChallengeMode.Challenge)
                saveTime = LevelMgr.GetInstance().GetLimitTime(CacheMgr.GetInstance().CurLevel) - _totalTime;
            CacheMgr.GetInstance().Wirte(CacheMgr.GetInstance().CurLevel, true, _totalTime);
        }
    }

    private void ReplaceSelect()
    {
        int id1 = _selectCellIds[0];
        Cell select1Cell = _allCellsDic[id1];
        int id2 = _selectCellIds[1];
        Cell select2Cell = _allCellsDic[id2];

        Sprite temp = select1Cell.GetSprite();
        select1Cell.SetSprite(select2Cell.GetSprite());
        select2Cell.SetSprite(temp);

        _allCellsDic[id1].SetSelect(false);
        _allCellsDic[id2].SetSelect(false);
    }

    private bool IsWin()
    {
        int correctCount = 0;
        foreach (var item in _allCellsDic)
        {
            if (item.Value.IsCorrect())
            {
                correctCount++;
            }
        }

        return correctCount >= _allCellsDic.Count;
    }

    private void Clear()
    {
        resultPanel.SetActive(false);
        _selectCellIds.Clear();
        foreach (var item in _allCellsDic)
        {
            DestroyImmediate(item.Value.gameObject);
        }
        _allCellsDic.Clear();
    }

    private string GetTimeModeStr()
    {
        (int hour, int min, int second) = ToHMS();
        if (hour > 0)
        {
            return string.Format("已用时:{0}时{1}分{2}秒", hour, min, second);
        }
        if (hour <= 0 && min > 0)
        {
            return string.Format("已用时:{0}分{1}秒", min, second);
        }
        return string.Format("已用时:{0}秒", second);
    }

    private string GetChallengeModeStr()
    {
        (int hour, int min, int second) = ToHMS();
        if (min > 0)
            return string.Format("剩余:{0}分{1}秒", min, second);
        if (min <= 0)
            return string.Format("剩余:{0}秒", second);
        return "";
    }

    private (int, int, int) ToHMS()
    {
        int hour = Mathf.FloorToInt(_totalTime / (60 * 60));
        int minutes = Mathf.FloorToInt(_totalTime / 60);
        int second = _totalTime % 60;
        return (hour, minutes, second);
    }

    private void OnNextLevelClick()
    {
        CacheMgr.GetInstance().CurLevel++;
        Start();
    }

    private void OnReturnClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void OnReplayClick()
    {
        Start();
    }

}
