using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHelper
{
    /// <summary>
    /// 获取当前得时间戳
    /// </summary>
    /// <returns></returns>
    public static string GetSystemCurrentTimeStamp()
    {
        return ConvertDateTimeTotTmeStamp(DateTime.Now).ToString();
    }
    /// <summary>
    /// 将时间转换成可视的时间格式
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string DataTimeToString(DateTime dateTime)
    {
        return string.Format("{3:D4}/{4:D2}/{5:D2}      " + "{0:D2}:{1:D2}:{2:D2} ", dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Year, dateTime.Month, dateTime.Day);
    }
    /// <summary>
    /// 将时间格式转换成时间戳
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long ConvertDateTimeTotTmeStamp(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (time.Ticks - startTime.Ticks) / 10000;  //除10000调整为13位   
        return t;
    }
    /// <summary>    
    /// 时间戳转为C#格式时间    
    /// </summary>    
    /// <param name=”timeStamp”></param>    
    /// <returns></returns>    
    public static DateTime ConvertStringToDateTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    /// <summary>
    /// 转化为00:00:00 格式
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string SecondConvertToHMS(int  seconds) {
        TimeSpan t = new TimeSpan(0, 0, Convert.ToInt32(seconds));
         return $"{t.Hours:00}:{t.Minutes:00}:{t.Seconds: 00}";
    }

    /// <summary>
    /// 转化为00-00-00 格式
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string SecondConvertToHMS1(int seconds)
    {
        TimeSpan t = new TimeSpan(0, 0, Convert.ToInt32(seconds));
        return $"{t.Hours:00}-{t.Minutes:00}-{t.Seconds:00}";
    }
}
