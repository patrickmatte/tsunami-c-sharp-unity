using System.Collections.Generic;

public delegate void EventHandler (Event evt);

internal class EventDispatcherListener {

	public EventHandler action;
	public string type;

	public EventDispatcherListener (string type, EventHandler action)
	{
		this.type = type;
		this.action = action;
	}

}

public class EventDispatcher {

	private List<EventDispatcherListener> listeners;

	public EventDispatcher() {
		this.listeners = new List<EventDispatcherListener> ();
	}

	public void AddEventListener(string type, EventHandler action) {
		listeners.Add(new EventDispatcherListener(type, action));
	}

	public void RemoveEventListener(string type, EventHandler action) {
		List<EventDispatcherListener> newListeners = new List<EventDispatcherListener>();
		foreach (EventDispatcherListener listener in listeners) {
			if (listener.type == type && listener.action == action) {
			} else {
				newListeners.Add(listener);
			}
		}

		listeners = newListeners;
	}

	public void DispatchEvent(Event evt) {
		evt.target = this;
		evt.currentTarget = this;

		List<EventDispatcherListener> listenersSlice = listeners.GetRange(0, listeners.Count);
		foreach (EventDispatcherListener listener in listenersSlice) {
			if (listener.type == evt.type) {
				evt.eventHandler = listener.action;
				listener.action(evt);
			}
		}
	}

}
