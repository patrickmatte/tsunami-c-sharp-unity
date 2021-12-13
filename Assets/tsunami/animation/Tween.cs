using System;
using System.Collections.Generic;

public class Tween : EventDispatcher, ITween
{

	public List<TweenProp> Properties;
	public Action<Event> UpdateHandler;
	public Action<Event> CompleteHandler;
	public bool ForceUpdate = false;

	protected Type type;
	protected float _startTime;
	protected float _duration;
	protected float _time;
	protected float _tweenTime;
	protected Int64 previousTime;

	public static string CHANGE = "change";
	public static string UPDATE = "update";
	public static string COMPLETE = "complete";
	public static Clock CLOCK;

	public Tween(float startTime, float duration, List<TweenProp> properties, Action<Event> updateHandler = null, Action<Event> completeHandler = null)
	{
		_startTime = startTime;
		_duration = duration;
		Properties = properties;
		UpdateHandler = updateHandler;
		CompleteHandler = completeHandler;
		_tweenTime = float.NaN;
		_time = float.NaN;
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

		set
		{
			_duration = UnityUtils.RoundDecimalToPlace(value, 3);
			DispatchEvent(new Event(Tween.CHANGE));
		}
	}

	public Promise Start(float time = 0, Action<Event> updateHandler = null)
	{
		Stop();
		if (updateHandler != null)
		{
			this.UpdateHandler = updateHandler;
		}
		Promise promise = new Promise((Action<object> resolve, Action<object> reject) => {
			EventHandler completeCallback = (Event evt) => {
				RemoveEventListener(Tween.COMPLETE, evt.eventHandler);
				resolve(this);
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
                foreach (TweenProp prop in Properties)
                {
                    prop.update(tweenTime / Duration);
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

}
