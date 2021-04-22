using System.Collections.Generic;
using UnityEngine;
public class TimerManager : TimerBaseManager<TimerManager>, IAnimatable {

	public TimerManager() {
		TimerManager.timerList.Add(this);
	}

	public static List<IAnimatable> timerList = new List<IAnimatable>();
	public static long serviceTime;

	public override long currentTime {
		get { return ( long )(Time.unscaledTime * 1000); }
	}

	public static void RemoveTimer(IAnimatable timerMgr) {
		timerList.Remove(timerMgr);
	}
}
