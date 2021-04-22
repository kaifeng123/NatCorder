using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Engine : MonoBehaviour {
	public static Engine Instance;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		//在后台运行
		Application.runInBackground = true;
		//屏幕常亮
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        //此对象在场景销毁的时候不删除
        DontDestroyOnLoad(gameObject);

		
	}
	void Update()
	{
		TimerManager.timerList.ForEach(advance => {
#if !UNITY_EDITOR
			try {
#endif
			advance.AdvanceTime();
#if !UNITY_EDITOR
			} catch (Exception err) {
				Debug.LogError(err.Message);
			}
#endif
		});
	}
	

	void OnDestroy()
	{	
	}
}
