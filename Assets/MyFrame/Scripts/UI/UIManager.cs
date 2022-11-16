using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



    public class UIManager : SingletonGameObject<UIManager>
    {
        public Dictionary<string, UIPanelBase> mUIPanelDir = new Dictionary<string, UIPanelBase>();
    
        void Awake()
        {
            Debug.Log("添加面板");
            EventManage.Instance.AddEventListener("AddPanel", AddPanel);
        }
        void OnDisable()
        {
            EventManage.Instance.RemoveEventListener("AddPanel", AddPanel);
        }
        private void AddPanel(object obj)
        {

            if (!mUIPanelDir.ContainsKey((obj as GameObject).name))
            {
                mUIPanelDir.Add((obj as GameObject).name, (obj as GameObject).GetComponent<UIPanelBase>());
            }
        }


	

	/// <summary>
	/// 根据面板名字显示某个类型的面板实例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="panelName"></param>
	/// <returns></returns>
	public T Show<T>() where T : UIPanelBase
	{
		string panelName = typeof(T).Name;
		if (mUIPanelDir.ContainsKey(panelName))
		{
			mUIPanelDir[panelName].Show();
			return mUIPanelDir[panelName] as T;
		}
		else
		{
			Debug.Log("没有相应面板" + panelName);
		}
		return null;
	}

	public void HideAllPanel()
	{
		foreach (var panel in mUIPanelDir.Values)
		{
			panel.Hide();
		}
	}



	public void Hide<T>() where T : UIPanelBase
	{
		string panelName = typeof(T).ToString();

		if (mUIPanelDir.ContainsKey(panelName))
		{
			mUIPanelDir[panelName].Hide();
		}
	}

}
