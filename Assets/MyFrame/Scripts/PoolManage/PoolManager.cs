using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonGameObject<PoolManager>
{
    public Dictionary<string, List<GameObject>> gameobjectDic = new Dictionary<string, List<GameObject>>();

    public GameObject GetElement(string name, string path)
    {

        List<GameObject> lstObjs;

        if (gameobjectDic.TryGetValue(name, out lstObjs))
        {
            if (lstObjs.Count < 1)
            {
                GameObject obj = ResourcesManage.Instance.Load<GameObject>(path);
                lstObjs.Add(obj);
            }
            else
            {
                Debug.Log("数量大于零");
            }
        }
        else
        {
            GameObject obj = ResourcesManage.Instance.Load<GameObject>(path);
            lstObjs = new List<GameObject> { obj };
            gameobjectDic.Add(name, lstObjs);
        }

        GameObject returnObj = lstObjs[0].gameObject;
        returnObj.SetActive(true);
        lstObjs.Remove(returnObj);
        return returnObj;
    }

    public void AddElement(string name, GameObject element)
    {
        List<GameObject> lstObjs;
        if (gameobjectDic.TryGetValue(name, out lstObjs) == false)
        {
            lstObjs = new List<GameObject>();
            gameobjectDic.Add(name, lstObjs);
        }

        lstObjs.Add(element);

        element.SetActive(false);
    }
}
