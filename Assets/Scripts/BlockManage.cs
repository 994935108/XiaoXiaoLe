using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlockManage : MonoBehaviour
{
    public static BlockManage Instance;
    public int row;
    public int column;
    internal  MapBlock[,] mapBlocks;
    public ColorBlock[] colorBlocks;
    public GameObject blockBg;

    private ColorBlock downColorBlock;
    private ColorBlock upColorBlock;
    private bool isOver;

    private bool isClearFinish;//是否清理结束
    private int clearCount;//单次清理数量

    internal UnityAction<int> ClearEvent;


    private void Awake()
    {
        Instance = this;
        EventManage.Instance.AddEventListener("OnColorBlockDown", OnColorBlockDown);
        EventManage.Instance.AddEventListener("OnColorBlockEnter", OnColorBlockEnter);
        EventManage.Instance.AddEventListener("OnColorBlockUp", OnColorBlockUp);
        StartGame();
    }


   
    public void StartGame() {
        mapBlocks = new MapBlock[row, column];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                MapBlock mapBlock = new MapBlock();
                mapBlock.localPos = new Vector3(j, i, 0);
                mapBlock.x = i;
                mapBlock.y = j;
                mapBlocks[i, j] = mapBlock;
                GameObject bg = Instantiate(blockBg, transform);
                bg.transform.localPosition = mapBlock.localPos;
            }
        }
        StartCoroutine(FillBlock());
    }

    public void Restart() {
        isOver = false;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (mapBlocks[i,j].colorBlock!=null) {
                    Destroy(mapBlocks[i, j].colorBlock.gameObject);
                    mapBlocks[i, j].RemoveColorBlock();
                }
            }
        }
        StartCoroutine(FillBlock());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Restart();
        }
    }

    public void GameOver() {
        isOver = true;
    }

    private void OnColorBlockDown(object colorBlock) {
        if (isOver) return;
        if (!isClearFinish) return;
        if (this.downColorBlock == null)
        {
            this.downColorBlock = (ColorBlock)colorBlock;
        }
    }
    private void OnColorBlockEnter(object colorBlock)
    {
        if (isOver) return;

        if (!isClearFinish) return;

        if (this.downColorBlock!=null) {
            if (this.upColorBlock==null) {
               
                this.upColorBlock = (ColorBlock)colorBlock;
                Vector3 pos1 = upColorBlock.transform.localPosition;
                Vector3 pos2 = downColorBlock.transform.localPosition;
                downColorBlock.Move(pos1);
                upColorBlock.Move(pos2);
            }
        }
    }

   
    private void OnColorBlockUp(object colorBlock)
    {
        if (isOver) return;

        if (!isClearFinish) return;

        if (upColorBlock != null && downColorBlock != null && upColorBlock.colorType != downColorBlock.colorType)
        {
            Swap(downColorBlock, upColorBlock);
            this.downColorBlock = null;
            this.upColorBlock = null;
        }
       
    }

    private IEnumerator FillBlock()
    {
        while (true) {
            for (int i = 0; i < row-1; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (mapBlocks[i, j].colorBlock == null)
                    {
                        if (i+1==row-1) {
                            if (mapBlocks[i + 1, j].colorBlock == null) {
                                 mapBlocks[i+1, j].AddColorBlock(SpawnBlock());
                            }
                          }
                        if (mapBlocks[i + 1, j].colorBlock != null)
                        {
                            mapBlocks[i, j].AddColorBlock(mapBlocks[i + 1, j].colorBlock);
                            mapBlocks[i + 1, j].RemoveColorBlock();
                        }
                    }
                }
            }
            if (!IsFill())
            {
                for (int j = 0; j < column; j++)
                {
                    if (mapBlocks[row - 1, j].colorBlock == null)
                    {
                        mapBlocks[row - 1, j].AddColorBlock(SpawnBlock());
                    }
                }
            }
            else {
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
       Clear();
    }
    private bool IsFill() {

        for (int j = 0; j < column; j++)
        {
            if (mapBlocks[row - 1, j].colorBlock == null)
            {
                return false;
            }
        }
        return true;

    }
    private ColorBlock SpawnBlock() {
        ColorBlockType colorBlockType = (ColorBlockType)Random.Range(0, colorBlocks.Length);
        return  CreateColorBlock(colorBlockType);
              
    }
    private ColorBlock CreateColorBlock(ColorBlockType colorBlockType)
    {
        for (int i = 0; i < colorBlocks.Length; i++)
        {
            if (colorBlocks[i].colorType == colorBlockType)
            {
                return Instantiate(colorBlocks[i], transform);
            }
        }
        return null;
    }
    private void  Clear() {
        for (int i = 0; i < row-1; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (mapBlocks[i,j].colorBlock!=null) {
                    if (CanRemoved(mapBlocks[i, j].colorBlock)) {
                        List<ColorBlock> tempBlock=new List<ColorBlock>();
                        tempBlock.Clear();
                        FindTheSameBlock(mapBlocks[i, j].colorBlock, in tempBlock);
                        if (tempBlock != null)
                        {
                            isClearFinish = false;
                            clearCount++;
                            StartCoroutine(IeClear(tempBlock));
                            return;
                        }
                    }
                }
            }
        }
        ClearEvent?.Invoke(clearCount);
        clearCount = 0;
        isClearFinish = true;
    }

    private IEnumerator IeClear(List<ColorBlock> tempBlock) {
        for (int k = 0; k < tempBlock.Count; k++)
        {
            if (tempBlock[k]!=null) {
                DestroyImmediate(tempBlock[k].gameObject);
                mapBlocks[tempBlock[k].x, tempBlock[k].y].RemoveColorBlock();
                yield return new WaitForSeconds(0.01f);
            }
           
        }
        yield return StartCoroutine(FillBlock());
        
    }
    private bool CanRemoved(ColorBlock colorBlock) {
        List<ColorBlock> mathingBlock = new List<ColorBlock>();
        mathingBlock.Add(colorBlock);
        int x = colorBlock.x;
        int y = colorBlock.y;
        bool isLeftOrUp = true;
        bool isRightOrDown = true;

        // 横向遍历
        for (int j = 1; j < column; j++)
        {
            int leftIndex = y - j;
            int rightIndex = y + j;
            if (!isRightOrDown && !isLeftOrUp)
            {
                break;

            }
            if (leftIndex >= 0 && isLeftOrUp)
            {
                if (mapBlocks[x, leftIndex].colorBlock.colorType == colorBlock.colorType)
                {
                    mathingBlock.Add(mapBlocks[x, leftIndex].colorBlock);
                }
                else
                {
                    isLeftOrUp = false;
                }
            }
            if (rightIndex < column && isRightOrDown)
            {
                if (mapBlocks[x, rightIndex].colorBlock.colorType == colorBlock.colorType)
                {
                    mathingBlock.Add(mapBlocks[x, rightIndex].colorBlock);
                }
                else
                {
                    isRightOrDown = false;
                }
            }
        }


        if (mathingBlock.Count < 3)
        {
            isLeftOrUp = true;
            isRightOrDown = true;
            mathingBlock.Clear();
            mathingBlock.Add(colorBlock);
            //纵向遍历
            for (int i = 1; i < row; i++)
            {
                int downIndex = x - i;
                int upIndex = x + i;
                if (!isRightOrDown && !isLeftOrUp)
                {
                    break;
                }

                if (downIndex >= 0 && isRightOrDown)
                {
                    if (mapBlocks[downIndex, y].colorBlock.colorType == colorBlock.colorType)
                    {
                        mathingBlock.Add(mapBlocks[downIndex, y].colorBlock);
                    }
                    else
                    {
                        isRightOrDown = false;
                    }
                }
                if (upIndex < row && isLeftOrUp)
                {
                    if (mapBlocks[upIndex, y].colorBlock.colorType == colorBlock.colorType)
                    {
                        mathingBlock.Add(mapBlocks[upIndex, y].colorBlock);
                    }
                    else
                    {
                        isLeftOrUp = false;
                    }
                }
            }
        }

        if (mathingBlock.Count<3) {
            return false;
        }
        return true;
    }


    private void  FindTheSameBlock(ColorBlock colorBlock,in List<ColorBlock> sameBlockList) {
        
        sameBlockList.Add(colorBlock);
        int  i = colorBlock.x;
        int j = colorBlock.y;

        if (i-1>=0) {
            if (mapBlocks[i-1,j].colorBlock.colorType== colorBlock.colorType) {
                if (!sameBlockList.Contains(mapBlocks[i - 1, j].colorBlock)) {
                    sameBlockList.Add(mapBlocks[i - 1, j].colorBlock);
                    FindTheSameBlock(mapBlocks[i - 1, j].colorBlock, in sameBlockList);
                }
            }
        }
        if (i + 1 <row)
        {
            if (mapBlocks[i + 1, j].colorBlock.colorType == colorBlock.colorType)
            {
                if (!sameBlockList.Contains(mapBlocks[i + 1, j].colorBlock)) { 
                sameBlockList.Add(mapBlocks[i + 1, j].colorBlock);
                FindTheSameBlock(mapBlocks[i + 1, j].colorBlock, in sameBlockList);
                }
            }
        }

        if (j + 1 < column)
        {
            if (mapBlocks[i, j+1].colorBlock.colorType == colorBlock.colorType)
            {
                if (!sameBlockList.Contains(mapBlocks[i, j + 1].colorBlock))
                {
                    sameBlockList.Add(mapBlocks[i, j + 1].colorBlock);
                    FindTheSameBlock(mapBlocks[i, j + 1].colorBlock, in sameBlockList);
                }

            }
        }

        if (j - 1 >=0)
        {
            if (mapBlocks[i, j - 1].colorBlock.colorType == colorBlock.colorType)
            {
                if (!sameBlockList.Contains(mapBlocks[i, j - 1].colorBlock))
                {
                    sameBlockList.Add(mapBlocks[i, j - 1].colorBlock);
                    FindTheSameBlock(mapBlocks[i, j - 1].colorBlock, in sameBlockList);
                }
            }
        }
       
    }

    private void Swap(ColorBlock downColorBlock,ColorBlock upColorBlock) {
        int i = Mathf.Abs(Mathf.Abs(downColorBlock.x) - Mathf.Abs(upColorBlock.x));
        int j = Mathf.Abs(Mathf.Abs(downColorBlock.y) - Mathf.Abs(upColorBlock.y));
        if ((i == 0 && j == 1)||(j == 0 && i == 1))
        {
            int x = upColorBlock.x;
            int y = upColorBlock.y;
            mapBlocks[downColorBlock.x, downColorBlock.y].AddColorBlock(upColorBlock);
            mapBlocks[x, y].AddColorBlock(downColorBlock);

            if (CanRemoved(upColorBlock) || CanRemoved(downColorBlock))
            {
               Clear();
            }
            else {
                 x = upColorBlock.x;
                 y = upColorBlock.y;
                mapBlocks[downColorBlock.x, downColorBlock.y].AddColorBlockAnim(upColorBlock);
                mapBlocks[x, y].AddColorBlock(downColorBlock);
            }
        }
    }
}
