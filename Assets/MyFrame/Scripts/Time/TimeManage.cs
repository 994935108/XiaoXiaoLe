using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Timer
{
	private MonoBehaviour mono;
	internal bool isPause;//是否暂停计时
	internal bool isStop;
	private Coroutine coroutine;

	public Timer(MonoBehaviour mono)
	{
		this.mono = mono;

	}
	/// <summary>
	/// 开始倒计时
	/// </summary>
	/// <param name="second"></param>
	/// <param name="onTimer"></param>
	public void Countdown(int second, UnityAction<int, bool> onTimer)
	{
		coroutine=mono.StartCoroutine(StartTiming(second, onTimer));
	}

	/// <summary>
	/// 开始计时
	/// </summary>
	/// <param name="onTimer"></param>

	public void Timing(UnityAction<int> onTimer) {
		coroutine=mono.StartCoroutine(StartTiming(onTimer));
	}
	/// <summary>
	/// 开始倒计时
	/// </summary>
	/// <param name="second"></param>
	/// <param name="onTimer"></param>
	/// <returns></returns>
	private IEnumerator StartTiming(int second, UnityAction<int, bool> onTimer)
	{
		while (second >= 0)
		{
			if (isStop) break;
			if (!isPause) onTimer?.Invoke(second, false);
			yield return new WaitForSeconds(1);
			if (!isPause) second -= 1;
			if (second <= 0) onTimer?.Invoke(second, true);
		}
	}

	/// <summary>
	/// 开始计时
	/// </summary>
	/// <param name="onTimer"></param>
	/// <returns></returns>
	private IEnumerator StartTiming(UnityAction<int> onTimer)
	{
		int second = 0;
		for (; ; )
		{
			if (isStop) break;
			if (!isPause) onTimer?.Invoke(second);
			yield return new WaitForSeconds(1);
			if (!isPause) second += 1;
			
		}
	}

	public  void StopTiming() {
		isStop = true;
	}

	public void Pause() {
		this.isPause = !this.isPause;
	}
}

public class TimeManage : SingletonGameObject<TimeManage>
{
	private Dictionary<string, Timer> timerDic = new Dictionary<string, Timer>();
	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.A))
		//{
		//	CreateTiming("1", (value) => { Debug.Log(TimeHelper.SecondConvertToHMS(value)); });
		//}
		//if (Input.GetKeyDown(KeyCode.S))
		//{
		//	PauseTiming("1");
		//}
		//if (Input.GetKeyDown(KeyCode.D))
		//{
		//	StopTiming("1");
		//}
	}
	/// <summary>
	/// 创建一个定时器
	/// </summary>
	/// <param name="second">  定时时长</param>
	/// <param name="onTimer"></param>
	public void CreateTimer(string timerName, int second, UnityAction<int, bool> onTimer) {
		Timer timer;
		if (!timerDic.TryGetValue(timerName, out timer))
		{
			timer = new Timer(this);
			timerDic.Add(timerName, timer);
			timer.Countdown(second, onTimer);
		}
		else {
			Debug.Log("已经包含该计时器："+ timerName);
		}
	}

	/// <summary>
	/// 停止计时
	/// </summary>
	/// <param name="timerName"></param>

	public void StopTiming(string timerName) {
		Timer timer;
		if (timerDic.TryGetValue(timerName, out timer))
		{
			timer.StopTiming();//停止计时 
			timerDic.Remove(timerName);//移除计时器
		}
		else
		{
			Debug.Log("没有包含要停止的计时器："+ timerName);
		}
	}
	/// <summary>
	/// 暂停计时
	/// </summary>
	/// <param name="timerName"></param>

	public void PauseTiming(string timerName)
	{
		Timer timer;
		if (timerDic.TryGetValue(timerName, out timer))
		{
			timer.Pause();//停止计时 
		}
		else
		{
			Debug.Log("没有包含该定时器");
		}
	}

	/// <summary>
	/// 创建一个计时器
	/// </summary>
	/// <param name="onTimer"></param>
	public void CreateTiming(string timerName,UnityAction<int> onTimer)
	{
		Timer timer;
		if (!timerDic.TryGetValue(timerName, out timer))
		{
			timer = new Timer(this);
			timerDic.Add(timerName, timer);
			timer.Timing(onTimer);
		}
		else
		{
			Debug.Log("已经包含该计时器");
		}
	}
}

