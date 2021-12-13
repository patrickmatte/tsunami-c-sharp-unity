public class Event {

	public string type;
	public EventDispatcher target;
	public EventDispatcher currentTarget;
	public object value;
	public EventHandler eventHandler;

	public Event (string type, object value = null)
	{
		this.type = type;
		this.value = value;
	}

}
