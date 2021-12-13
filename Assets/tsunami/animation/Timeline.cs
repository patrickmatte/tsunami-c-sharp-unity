using System.Collections.Generic;
using System;

public class Timeline : EventDispatcher, ITween {

	public Action<Event> UpdateHandler;
	public Action<Event> CompleteHandler;
	public bool ForceUpdate = false;
	public bool ResetTweensOnScrub;
	public bool SetTimeOnAddTween;

	protected List<ITween> Tweens = new List<ITween> ();
	protected List<ITween> TweensByStartTime;
	protected List<ITween> TweensByEndTime;
	protected float _startTime;
	protected float _duration;
	protected float _time;
	protected float _tweenTime;
	protected Int64 previousTime;

	public Timeline(List<ITween> tweens = null, float startTime = 0, Action<Event> updateHandler = null, Action<Event> completeHandler = null)
    {
		if(tweens != null)
        {
			Tweens = tweens;
		}
		_startTime = startTime;
		_duration = 0;
		UpdateHandler = updateHandler;
		CompleteHandler = completeHandler;
		_tweenTime = float.NaN;
		_time = float.NaN;
		ForceUpdate = false;
		ResetTweensOnScrub = true;
		SetTimeOnAddTween = true;
		TweensByStartTime = new List<ITween>();
		TweensByEndTime = new List<ITween>();
		foreach (ITween tween in Tweens)
		{
			Add(tween);
		}
	}

	public float StartTime
    {
		get
		{
			return _startTime;
		}

		set
		{
			_startTime = value;
			DispatchEvent(new Event(Tween.CHANGE));
		}
	}

	public float EndTime
    {
		get
		{
			return StartTime + Duration;
		}
	}

	public float Duration
    {
		get
		{
			return _duration;
		}
	}


	public Promise Start(float time = 0, Action<Event> updateHandler = null)
	{
		Stop();
		if (updateHandler != null)
		{
			this.UpdateHandler = updateHandler;
		}
		Promise promise = new Promise ((Action<object> resolve, Action<object> reject) => {
			EventHandler completeCallback = (Event evt) => {
				RemoveEventListener (Tween.COMPLETE, evt.eventHandler);
				resolve (this);
			};
			AddEventListener(Tween.COMPLETE, completeCallback);
		});
		_tweenTime = float.NaN;
		Time = time;
		previousTime = Tween.CLOCK.time;
		Tween.CLOCK.AddEventListener(Clock.TICK, TickHandler);
		return promise;
	}

	public void TickHandler(Event e)
	{
		Int64 currentTime = (Int64)e.value;
		Time += (currentTime - previousTime) / 1000f;
		previousTime = currentTime;
	}

	public void Pause()
	{
		Tween.CLOCK.RemoveEventListener(Clock.TICK, TickHandler);
	}

	public void Resume()
	{
		this.previousTime = Tween.CLOCK.time;
		Tween.CLOCK.AddEventListener(Clock.TICK, TickHandler);
	}


	public void Stop()
	{
		Tween.CLOCK.RemoveEventListener(Clock.TICK, TickHandler);
	}

	public float Time
    {
		get
		{
			return _time;
		}

		set
		{
			_time = value;
			float tweenTime = value - StartTime;
			tweenTime = Math.Max(tweenTime, 0);
			tweenTime = Math.Min(tweenTime, Duration);
			if (tweenTime != _tweenTime || ForceUpdate)
			{
				_tweenTime = tweenTime;
				foreach (ITween tween in TweensByStartTime)
				{
					if (tweenTime < tween.StartTime && ResetTweensOnScrub)
					{
						tween.Time = tweenTime;
					}
				}
				foreach (ITween tween in TweensByEndTime)
				{
					if (tweenTime > tween.EndTime && ResetTweensOnScrub)
					{
						tween.Time = tweenTime;
					}
				}

				foreach (ITween tween in Tweens)
				{
					float startTime = tween.StartTime;
					float endTime = tween.EndTime;
					if (tweenTime >= startTime && tweenTime <= endTime)
					{
						tween.Time = tweenTime;
					}
				}

				Event updateEvent = new Event(Tween.UPDATE);
				if (UpdateHandler != null)
				{
					UpdateHandler(updateEvent);
				}
				DispatchEvent(updateEvent);
			}
			if (tweenTime >= Duration)
			{
				Event completeEvent = new Event(Tween.COMPLETE);
				if (CompleteHandler != null)
				{
					CompleteHandler(completeEvent);
				}
				Stop();
				DispatchEvent(completeEvent);
			}
		}
    }

	public float TimeFraction
    {
		set
		{
			Time = value * Duration;
		}

		get
		{
			return Time / Duration;
		}
    }

	public void Add(ITween tween)
	{
		tween.AddEventListener(Tween.CHANGE, TweenChangeHandler);
		Tweens.Add(tween);
		float startTime = tween.StartTime;
		float endTime = tween.StartTime + tween.Duration;
		if (Time >= tween.StartTime && Time <= tween.EndTime && SetTimeOnAddTween)
		{
			tween.Time = Time;
		}
		RecalculateDuration();
	}

	public void Remove(ITween tween)
	{
		List<ITween> array = new List<ITween>();
		foreach (ITween oldTween in Tweens)
		{
			if (oldTween == tween)
			{
				tween.RemoveEventListener(Tween.CHANGE, TweenChangeHandler);
			}
			else
			{
				array.Add(oldTween);
			}
		}
		Tweens = array;
		RecalculateDuration();
	}

	public void TweenChangeHandler(Event e)
	{
		RecalculateDuration();
	}

	public int SortTweensByStartTime(ITween x, ITween y)
	{
		return ((x.StartTime - y.StartTime) > 0) ? 0 : 1;
	}

	public int SortTweensByEndTime(ITween x, ITween y)
	{
		return ((y.StartTime - x.StartTime) < 0) ? 0 : 1;
	}

	protected void RecalculateDuration()
	{
		float duration = 0;
		foreach (ITween tween in Tweens)
		{
			float tweenDuration = tween.StartTime + tween.Duration;
			duration = Math.Max(duration, tweenDuration);
		}
		_duration = duration;

		TweensByStartTime = new List<ITween>();
		TweensByEndTime = new List<ITween>();
		foreach (ITween tween in Tweens)
		{
			TweensByStartTime.Add(tween);
			TweensByEndTime.Add(tween);
		}
		TweensByStartTime.Sort(SortTweensByStartTime);
		TweensByEndTime.Sort(SortTweensByEndTime);
	}

}

