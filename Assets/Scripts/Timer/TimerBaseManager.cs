using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void Handler();
public delegate void Handler<T1>(T1 param1);
public delegate void Handler<T1, T2>(T1 param1, T2 param2);
public delegate void Handler<T1, T2, T3>(T1 param1, T2 param2, T3 param3);

public delegate bool LoopHandler();
public delegate bool LoopHandler<T1>(T1 param1);
public delegate bool LoopHandler<T1, T2>(T1 param1, T2 param2);
public delegate bool LoopHandler<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
public interface IAnimatable {
	void AdvanceTime();
}

/**时钟管理器[同一函数多次计时，默认会被后者覆盖,delay小于1会立即执行]*/
public abstract class TimerBaseManager<T> where T : new(){

	private static T _instance;
	public static T instance {
		get {
			if (_instance == null) {
				_instance = new T();
			}
			return _instance;
		}
	}

	private List<TimerHandler> _pool = new List<TimerHandler>();
	/** 用数组保证按放入顺序执行*/
	public List<TimerHandler> _handlers = new List<TimerHandler>();
	private int _currFrame = 0;
	private uint _index = 0;

	public void AdvanceTime() {
		_currFrame++;
		for (int i = 0; i < _handlers.Count; i++) {
			TimerHandler handler = _handlers[i];
			long t = handler.userFrame ? _currFrame : currentTime;
			if (t >= handler.exeTime) {
				Delegate method = handler.method;
				object[] args = handler.args;
				if (handler.repeat) {
					while (t >= handler.exeTime) {
						handler.exeTime += handler.delay;
						method.DynamicInvoke(args);
					}
				} else {
					RemoveHandler(handler.method);
					method.DynamicInvoke(args);
				}
			}
		}
	}

	private object create(bool useFrame, bool repeat, bool cover, int delay, Delegate method, params object[] args) {
		if (method == null) {
			return null;
		}

		//如果执行时间小于1，直接执行
		if (delay < 1) {
			method.DynamicInvoke(args);
			return -1;
		}
		TimerHandler handler;
		TimerHandler coverHandler = _handlers.Find(han => han.method == method);
		if (cover && coverHandler != null) {
			handler = coverHandler;
			_handlers.Remove(coverHandler);
		} else {
			if (_pool.Count > 0) {
				handler = _pool[_pool.Count - 1];
				_pool.Remove(handler);
			} else {
				handler = new TimerHandler();
			}
		}

		handler.userFrame = useFrame;
		handler.repeat = repeat;
		handler.delay = delay;
		handler.method = method;
		handler.args = args;
		handler.exeTime = delay + (useFrame ? _currFrame : currentTime);
		_handlers.Add(handler);
		return method;
	}

	/// /// <summary>
	/// 定时执行一次(基于毫秒)
	/// </summary>
	/// <param name="delay">延迟时间(单位毫秒)</param>
	/// <param name="method">结束时的回调方法</param>
	/// <param name="cover">当method相同时是否覆盖</param>
	/// <param name="args">回调参数</param>
	public void DoOnce(int delay, Handler method, bool cover = false, params object[] args)
	{
		create(false, false, cover, delay, method, args);
	}
	public void DoOnce<T1>(int delay, Handler<T1> method, bool cover = false, params object[] args) {
		create(false, false, cover, delay, method, args);
	}
	public void DoOnce<T1, T2>(int delay, Handler<T1, T2> method, bool cover = false, params object[] args) {
		create(false, false, cover, delay, method, args);
	}
	public void DoOnce<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, bool cover = false, params object[] args) {
		create(false, false, cover, delay, method, args);
	}

	/// /// <summary>
	/// 定时重复执行(基于毫秒)
	/// </summary>
	/// <param name="delay">延迟时间(单位毫秒)</param>
	/// <param name="method">结束时的回调方法</param>
	/// <param name="cover">当method相同时是否覆盖</param>
	/// <param name="args">回调参数</param>
	public void DoLoop(int delay, Handler method, bool cover = false, params object[] args) {
		create(false, true, cover, delay, method, args);
	}
	public void DoLoop<T1>(int delay, Handler<T1> method, bool cover = false, params object[] args) {
		create(false, true, cover, delay, method, args);
	}
	public void DoLoop<T1, T2>(int delay, Handler<T1, T2> method, bool cover = false, params object[] args) {
		create(false, true, cover, delay, method, args);
	}
	public void DoLoop<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, bool cover = false, params object[] args) {
		create(false, true, cover, delay, method, args);
	}

	/// <summary>
	/// 定时执行一次(基于帧率)
	/// </summary>
	/// <param name="delay">延迟时间(单位为帧)</param>
	/// <param name="method">结束时的回调方法</param>
	/// <param name="cover">当method相同时是否覆盖</param>
	/// <param name="args">回调参数</param>
	public void DoFrameOnce(int delay, Handler method, bool cover = false, params object[] args) {
		create(true, false, cover, delay, method, args);
	}
	public void DoFrameOnce<T1>(int delay, Handler<T1> method, bool cover = false, params object[] args) {
		create(true, false, cover, delay, method, args);
	}
	public void DoFrameOnce<T1, T2>(int delay, Handler<T1, T2> method, bool cover = false, params object[] args) {
		create(true, false, cover, delay, method, args);
	}
	public void DoFrameOnce<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, bool cover = false, params object[] args) {
		create(true, false, cover, delay, method, args);
	}

	/// <summary>
	/// 定时重复执行(基于帧率)
	/// </summary>
	/// <param name="delay">延迟时间(单位为帧)</param>
	/// <param name="method">结束时的回调方法</param>
	/// <param name="cover">当method相同时是否覆盖</param>
	/// <param name="args">回调参数</param>
	public void DoFrameLoop(int delay, Handler method, bool cover = false, params object[] args) {
		create(true, true, cover, delay, method, args);
	}
	public void DoFrameLoop<T1>(int delay, Handler<T1> method, bool cover = false, params object[] args) {
		create(true, true, cover, delay, method, args);
	}
	public void DoFrameLoop<T1, T2>(int delay, Handler<T1, T2> method, bool cover = false, params object[] args) {
		create(true, true, cover, delay, method, args);
	}
	public void DoFrameLoop<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, bool cover = false, params object[] args) {
		create(true, true, cover, delay, method, args);
	}

	/// <summary>
	/// 清理定时器
	/// </summary>
	/// <param name="method">method为回调函数本身</param>
	public void RemoveHandler(Handler method) {
		RemoveHandler(( Delegate )method);
	}
	public void RemoveHandler<T1>(Handler<T1> method) {
		RemoveHandler(( Delegate )method);
	}
	public void RemoveHandler<T1, T2>(Handler<T1, T2> method) {
		RemoveHandler(( Delegate )method);
	}
	public void RemoveHandler<T1, T2, T3>(Handler<T1, T2, T3> method) {
		RemoveHandler(( Delegate )method);
	}

	private void RemoveHandler(Delegate method) {
		List<TimerHandler> handler = _handlers.FindAll(t => t.method == method);
		if (handler.Count > 0) {
			handler.ForEach(a => {
				_handlers.Remove(a);
				a.clear();
				if(Engine.Instance != null) {
					Engine.Instance.StartCoroutine(AddPool(a));
				} else {
					_pool.Add(a);
				}
			});
		}
	}

	public IEnumerator AddPool(TimerHandler handler) {
		yield return new WaitForSeconds(1);
		_pool.Add(handler);
	}

	/// <summary>
	/// 清理所有定时器
	/// </summary>
	public void RemoveAllHandler() {
		while (_handlers.Count > 0) {
			RemoveHandler(_handlers[0].method);
		}
	}

	/// <summary>
	/// 游戏自启动运行时间(真实时间，不受加速限制)，毫秒
	/// </summary>
	public virtual long currentTime {
		get { return ( long )(Time.unscaledTime * 1000); }
	}

	/**定时处理器*/

	public class TimerHandler {
		/**执行间隔*/
		public int delay;
		/**是否重复执行*/
		public bool repeat;
		/**是否用帧率*/
		public bool userFrame;

		/**执行时间*/
		public long exeTime;

		/**处理方法*/
		public Delegate method;

		/**参数*/
		public object[] args;

		/**清理*/

		public void clear() {
			method = null;
			args = null;
		}
	}
}
