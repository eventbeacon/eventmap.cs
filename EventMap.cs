using System;
using System.Collections;
using System.Collections.Generic;

public class GenericEventMap<T>
{
	public delegate void Event(params T[] data);

	protected Dictionary<String, ArrayList> beforeEvents;
	protected Dictionary<String, ArrayList> events;
	protected Dictionary<String, ArrayList> afterEvents;

	public GenericEventMap ()
	{
		beforeEvents = new Dictionary<String, ArrayList> ();
		events = new Dictionary<String, ArrayList>();
		afterEvents = new Dictionary<String, ArrayList> ();
	}
	
	private void AddEvent(Dictionary<String, ArrayList> map, String name, Event ev)
	{
		ArrayList value;

		if (!map.TryGetValue(name, out value)) {
			map.Add (name, new ArrayList());
		}

		map [name].Add (ev);
	}

	public void AddListener (String name, Event ev)
	{
		AddEvent (events, name, ev);
	}

	public void RemoveListener (String name)
	{
		ArrayList value;

		if (beforeEvents.TryGetValue(name, out value)) {
			events.Remove (name);
		}

		if (events.TryGetValue(name, out value)) {
			events.Remove (name);
		}

		if (afterEvents.TryGetValue(name, out value)) {
			events.Remove (name);
		}
	}

	public void On(String name, Event ev)
	{
		AddListener (name, ev);
	}

	public void Off(String name)
	{
		RemoveListener (name);
	}

	public void Before(String name, Event ev)
	{
		AddEvent (beforeEvents, name, ev);
	}

	public void After(String name, Event ev)
	{
		AddEvent (afterEvents, name, ev);
	}

	public void Trigger(String name, params T[] data)
	{
		ArrayList value;

		if (beforeEvents.TryGetValue(name, out value)) {
			foreach (Event ev in value) {
				ev(data);
			}
		}

		if (events.TryGetValue(name, out value)) {
			foreach (Event ev in value) {
				ev(data);
			}
		}

		if (afterEvents.TryGetValue(name, out value)) {
			foreach (Event ev in value) {
				ev(data);
			}
		}
	}
}

public class EventMap: GenericEventMap<object> {

}

