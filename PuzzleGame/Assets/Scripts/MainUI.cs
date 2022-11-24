using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    ScrollRect scroll;
    [SerializeField]
    Transform container;
    [SerializeField]
    PreviewCell cell;
    [SerializeField]
    Button btnTimeMode;
    [SerializeField]
    Button btnChallengeMode;

    private PreviewCell _curSelectedCell;

    void Start()
    {
        btnTimeMode.onClick.AddListener(OnTimeModeClick);
        btnChallengeMode.onClick.AddListener(OnChallengeModeClick);
        InitLevel();
    }

    void InitLevel()
    {
        int count = LevelMgr.GetInstance().GetLevelCount();
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(cell.gameObject,container);
            go.SetActive(true);
            PreviewCell previewCell = go.GetComponent<PreviewCell>();
            previewCell.Refresh(i,this);
        }
    }

    public void OnCellClick(PreviewCell cell)
    {
        if (_curSelectedCell == null)
            _curSelectedCell = cell;
        _curSelectedCell.SetSelected(false);
        _curSelectedCell = cell;
        _curSelectedCell.SetSelected(true);
    }

    private void OnTimeModeClick()
    {
        ToGameScene(ChallengeMode.Time);
    }

    private void OnChallengeModeClick()
    {
        ToGameScene(ChallengeMode.Challenge);
    }

    private void ToGameScene(ChallengeMode mode)
    {
        if (_curSelectedCell != null)
        {
            LevelMgr.GetInstance().CurChallengeMode = mode;
            SceneManager.LoadSceneAsync(2);
        }
    }

}
