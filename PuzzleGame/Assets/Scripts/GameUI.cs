using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Texture2D[] allTexture;

    private List<Cell> allCells = new List<Cell>();

    void Start()
    {
        int level = 2;        
        Sprite[] sprites = LevelMgr.GetInstance().GetSprites(level);
        Dictionary<string, Sprite> dic = new Dictionary<string, Sprite>();
        for (int i = 0; i < sprites.Length; i++)
        {
            dic.Add(sprites[i].name, sprites[i]);
        }
        Texture2D texture = allTexture[level];
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

                data.pos = new Vector2(i, j);
                //cellComp.SetCell(sprites[i],this)
                //index++;
            };
        }
    }

    public void OnCellClick(Cell cell)
    {

    }

}
