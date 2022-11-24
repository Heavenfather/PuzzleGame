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
    [SerializeField]
    GameObject selectGo;

    private CellData data;
    private string curPos;
    private GameUI gameUI;

    void Start()
    {
        btn.onClick.AddListener(OnCellClick);
    }

    private void OnCellClick()
    {
        gameUI.OnCellClick(this);
    }

    public void SetCell(CellData data, GameUI parent)
    {
        this.data = data;
        SetSprite(data.sprite);
        gameUI = parent;
    }

    public void SetSprite(Sprite sp)
    {
        img.sprite = sp;
    }
    
    public Sprite GetSprite()
    {
        return img.sprite;
    }

    public void SetSelect(bool b)
    {
        selectGo.SetActive(b);
    }


    public bool IsCorrect()
    {
        return img.sprite.name == data.pos;
    }

    public CellData GetData()
    {
        return data;
    }
}
