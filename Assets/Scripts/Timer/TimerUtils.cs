using System;
using UnityEngine;
public class TimerUtils {

	/// <summary>
	/// 返回 00:00:00 格式时间
	/// </summary>
	public static string toTimeString(float time_seconds) {
		int h = (int)(time_seconds) / 3600 ;
		int m = (int)(time_seconds) % 3600 / 60;
		int s = (int)(time_seconds) % 3600 % 60;
		return string.Format("{0:D2}:{1:D2}:{2:D2}",h, m, s);
	}
/// <summary>
/// 返回天数
/// </summary>
/// <param name="time_seconds"></param>
/// <returns></returns>
	public static bool ToDayNum(long time_seconds)
	{		
		int d = (int)((time_seconds) / 3600000 / 24);
		return d > 30;
	}
	
	/// <summary>
	/// 返回 00:00 格式时间
	/// </summary>
	public static string ToTimeStringMS(float time_seconds) {
		int m = (int)(time_seconds) / 60;
		int s = (int)(time_seconds) % 60;
		return string.Format("{0:D2}:{1:D2}", m, s);
	}

	//返回秒 时间差 00 格式时间
	public static string timeDifference(long endTime) {
		return string.Format("{0:D2}", timeDifferenceInt(endTime));
	}

	public static int timeDifferenceInt(long endTime)
	{
		long diff = endTime - TimerManager.serviceTime;
		return (int)(diff / 1000);

	}

	/// <summary>
	/// 获取当前时间13位时间戳
	/// </summary>
	/// <returns></returns>
	public static long GetTimeStamp() {
		long time = (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime()).TotalMilliseconds;
		return time;
	}

	
	/// <summary>
	/// 通过输入的时间格式:2018-7-19 12:00:00 转为13位时间搓
	/// </summary>
	/// <returns></returns>
	/// 
	public static long GetNowTimeStamp(string time)
	{
		System.DateTime Newtime = Convert.ToDateTime(time);
		System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
		long t = (Newtime.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位 
		return t;
	}


	public static string GetTimeStamp(System.DateTime time)
	{
		long ts = ConvertDateTimeToInt(time);
		return ts.ToString();
	}
	// <param name="time">时间</param> 10位时间搓
	public static long ConvertDateTimeToInt(System.DateTime time)
	{
		TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
		return Convert.ToInt64(ts.TotalSeconds);
	}
	public static long GetLongTime(System.DateTime time1, System.DateTime time2 )
	{
		TimeSpan ts = time1 - time2;
		return Convert.ToInt64(ts.TotalSeconds);
	}
}
