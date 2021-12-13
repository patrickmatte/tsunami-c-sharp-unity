using System;

public class Clock:EventDispatcher {

	public static string TICK = "tick";

	protected Int64 _time;

	public Clock () {
		_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
	}

	public Int64 time {
		get {
			return _time;
		}
	}

	public void tick() {
		_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
		this.DispatchEvent(new Event(Clock.TICK, time));
	}

}
