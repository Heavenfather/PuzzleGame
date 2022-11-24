using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellData
{
    public int id;
    public Sprite sprite;
    //存的是正确的坐标
    public string pos;

    public void SetPos(int level, int index)
    {
        this.pos = level + "_" + index;
    }
}
