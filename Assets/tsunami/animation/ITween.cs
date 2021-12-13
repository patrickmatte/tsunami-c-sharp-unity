using System;

public interface ITween
{

	float StartTime
	{
		get;
	}

	float Duration
	{
		get;
	}

	float EndTime
	{
		get;
	}

	float Time
	{
		get;
		set;
	}

	public void AddEventListener(string type, EventHandler action);

	public void RemoveEventListener(string type, EventHandler action);

}
