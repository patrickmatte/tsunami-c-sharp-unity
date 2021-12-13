using System;

public class TimelineAction : EventDispatcher, ITween
{

	public Action Forward;
	public Action Backward;
	protected float _startTime;
	protected float _time;

	public TimelineAction(float startTime, Action forward, Action backward)
	{
		_startTime = startTime;
		_time = float.NaN;
		Forward = forward;
		Backward = backward;
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
			return StartTime;
		}
	}

	public float Duration
	{
		get
		{
			return 0;
		}
	}

	public float Time
	{
		get
		{
			return _time;
		}

		set
		{
			float previousTime = Time;
			float diff = value - previousTime;
			if (diff > 0)
			{
				if (value >= StartTime && previousTime < StartTime && Forward != null)
				{
					Forward();
				}
			}
			else
			{
				if (value <= StartTime && previousTime > StartTime && Backward != null)
				{
					this.Backward();
				}
			}
			this._time = value;
		}
	}

}