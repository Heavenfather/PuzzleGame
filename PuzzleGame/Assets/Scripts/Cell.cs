using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField]
    Image img;
    [SerializeField]
    Button btn;

    private CellData data;
    private GameUI gameUI;

    void Start()
    {
        btn.onClick.AddListener(OnCellClick);
    }

    private void OnCellClick()
    {
        gameUI.OnCellClick(this);
    }

    public void SetCell(CellData data,GameUI parent)
    {
        img.sprite = data.sprite;
        gameUI = parent;
    }

    public void SetCellPos(Vector2 pos)
    {
        data.pos = pos;
    }

    public CellData GetData()
    {
        return data;
    }

}
