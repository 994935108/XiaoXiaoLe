using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum ColorBlockType { 
    Blue=0,
    Green=1,
    Pink=2,
    Red=3,
    Yellow=4,
    White=5,
    Black=6,
}

public class ColorBlock : MonoBehaviour
{
    internal int x, y;
    internal bool canMove;
    public ColorBlockType colorType;

    public void OnMouseDown()
    {
        EventManage.Instance.OnTriggerEvent("OnColorBlockDown",this);

    }

    public void OnMouseEnter()
    {
        EventManage.Instance.OnTriggerEvent("OnColorBlockEnter", this);

    }
   
    public void OnMouseUp()
    {
        EventManage.Instance.OnTriggerEvent("OnColorBlockUp", this);
    }

    public void Move(Vector3 pos)
    {
        Debug.Log("移动");
        transform.DOLocalMove(pos, 0.2f);
    }
}
