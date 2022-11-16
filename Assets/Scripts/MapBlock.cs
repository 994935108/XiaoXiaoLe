using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock
{
    public int x,y;
    public Vector3 localPos;
    public ColorBlock colorBlock;
    /// <summary>
    /// 填充色块
    /// </summary>
    /// <param name="colorBlock"></param>

    public void AddColorBlock(ColorBlock colorBlock) {
        this.colorBlock = colorBlock;
        this.colorBlock.transform.localPosition = localPos;
        this.colorBlock.x = x;
        this.colorBlock.y = y;
    }

    public void AddColorBlockAnim(ColorBlock colorBlock)
    {
        this.colorBlock = colorBlock;
        this.colorBlock.Move(localPos);
        this.colorBlock.x = x;
        this.colorBlock.y = y;
    }
    /// <summary>
    /// 移除色块
    /// </summary>
    /// <param name="colorBlock"></param>

    public void RemoveColorBlock()
    {
        this.colorBlock = null;
    }

}
