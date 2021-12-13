using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void DefinePromise ();

public class Promise {

	//public delegate void Handler (object value);

	protected bool isResolved;
	protected bool isRejected;
	private object _result;
	private List<IPromiseCallback> resolveListeners = new List<IPromiseCallback> ();
	private List<IPromiseCallback> rejectListeners = new List<IPromiseCallback> ();

	public Promise ()
	{
	}

	public Promise (Action<Action<object>, Action<object>> initializer)
	{
		if (initializer != null) {
			initializer (ResolvePromise, RejectPromise);
		}
	}

	public object result {
		get {
			return _result;
		}
	}

	public void ResolvePromise (object value = null)
	{
		_result = value;
		isResolved = true;
		foreach (IPromiseCallback action in resolveListeners) {
			action.execute (_result);
		}
	}

	public void RejectPromise (object value = null)
	{
		_result = value;
		isRejected = true;
		foreach (IPromiseCallback action in rejectListeners) {
			action.execute (_result);
		}
	}

	public Promise Then (Action<object> resolveCallback, Action<object> rejectCallback = null)
	{
		Promise promise = new Promise ();
		if (isResolved) {
			resolveCallback (_result);
			promise.ResolvePromise();
		}
		else {
			resolveListeners.Add (new PromiseCallback (resolveCallback, promise));
		}
		if (rejectCallback != null) {
			if (isRejected) {
				rejectCallback (_result);
				promise.ResolvePromise();
			}
			else {
				rejectListeners.Add (new PromiseCallback (rejectCallback, promise));
			}
		}
		return promise;
	}

	public Promise Then (Func<object, object> resolveCallback, Func<object, object> rejectCallback = null)
	{
		Promise promise = new Promise ();
		if (isResolved) {
			object obj = resolveCallback (_result);
			promise.ResolvePromise (obj);
		} else {
			resolveListeners.Add (new PromiseCallbackFunc(resolveCallback, promise));
		}
		if (rejectCallback != null) {
			if (isRejected) {
				object obj = rejectCallback (_result);
				promise.ResolvePromise (obj);
			} else {
				rejectListeners.Add (new PromiseCallbackFunc (rejectCallback, promise));
			}
		}
		return promise;
	}

	public Promise Then (Func<object, Promise> resolveCallback, Func<object, Promise> rejectCallback = null)
	{
		Promise promise = new Promise();
		if (isResolved) {
			promise = resolveCallback (_result);
		} else {
			resolveListeners.Add (new PromiseCallbackFuncPromise(resolveCallback, promise));
		}
		if (rejectCallback != null) {
			if (isRejected) {
				promise = rejectCallback (_result);
			} else {
				rejectListeners.Add (new PromiseCallbackFuncPromise (resolveCallback, promise));
			}
		}
		return promise;
	}



	public static PromiseAll All (List<Promise> promises)
	{
		PromiseAll thePromise = new PromiseAll ((Action<List<object>> resolve, Action<List<object>> reject) => {

			int promiseCompletedTotal = 0;

			Action<object> promiseValue = (value => {
				promiseCompletedTotal++;
				if (promiseCompletedTotal == promises.Count) {
					List<object> resultList = new List<object> ();
					foreach (Promise promise in promises) {
						resultList.Add (promise.result);
					}
					resolve (resultList);
				}
			});

			foreach (Promise promise in promises) {
				promise.Then (promiseValue);
			}

		});
		return thePromise;
	}

	public static Promise Resolve (object value = null)
	{
		Promise promise = new Promise ((Action<object> resolve, Action<object> reject) => {
			resolve (value);
		});
		return promise;
	}

	public static Promise Reject (object value = null)
	{
		Promise promise = new Promise ((Action<object> resolve, Action<object> reject) => {
			reject (value);
		});
		return promise;
	}

}

public class PromiseAll:Promise {

	public delegate void Handler (List<object> value);

	private List<Action<List<object>>> resolveListenersAll = new List<Action<List<object>>> ();
	private List<Action<List<object>>> rejectListenersAll = new List<Action<List<object>>> ();
	private List<object> _result;

	public PromiseAll ()
	{

	}

	public PromiseAll (Action<Action<List<object>>, Action<List<object>>> initializer) {
		initializer (ResolvePromise, RejectPromise);
	}

	public new List<object> result {
		get {
			return _result;
		}
	}

	protected void ResolvePromise (List<object> value)
	{
		_result = value;
		isResolved = true;
		foreach (Action<List<object>> action in resolveListenersAll) {
			action (_result);
		}
	}

	protected void RejectPromise (List<object> value)
	{
		_result = value;
		isRejected = true;
		foreach (Action<List<object>> action in rejectListenersAll) {
			action (_result);
		}
	}

	public Promise Then(Action<List<object>> resolveCallback, Action<List<object>> rejectCallback = null)
	{
		Promise promise = new Promise();
		if (isResolved)
		{
			resolveCallback(_result);
			promise.ResolvePromise();
		}
		else
		{
			resolveListenersAll.Add(resolveCallback);
		}
		if (rejectCallback != null)
		{
			if (isRejected)
			{
				rejectCallback(_result);
				promise.ResolvePromise();
			}
			else
			{
				rejectListenersAll.Add(rejectCallback);
			}
		}
		return promise;
	}

}

internal interface IPromiseCallback
{
	void execute(object value);
}

internal interface IPromiseAllCallback
{
	void execute(List<object> value);
}

internal class PromiseCallback : IPromiseCallback
{

	public Action<object> callback;
	public Promise promise;

	public PromiseCallback(Action<object> callback, Promise promise)
	{
		this.callback = callback;
		this.promise = promise;
	}

	public void execute(object value)
	{
		callback(value);
		promise.ResolvePromise();
	}

}

internal class PromiseAllCallback : IPromiseAllCallback
{

	public Action<List<object>> callback;
	public Promise promise;

	public PromiseAllCallback(Action<List<object>> callback, Promise promise)
	{
		this.callback = callback;
		this.promise = promise;
	}

	public void execute(List<object> value)
	{
		callback(value);
		promise.ResolvePromise();
	}

}

internal class PromiseCallbackFunc : IPromiseCallback {

	public Func<object, object> callback;
	public Promise promise;

	public PromiseCallbackFunc (Func<object, object> callback, Promise promise)
	{
		this.callback = callback;
		this.promise = promise;
	}

	public void execute (object value)
	{
		object returnValue = callback (value);
		promise.ResolvePromise (returnValue);
	}

}

internal class PromiseCallbackFuncPromise : IPromiseCallback {

	public Func<object, Promise> callback;
	public Promise promise;

	public PromiseCallbackFuncPromise (Func<object, Promise> callback, Promise promise)
	{
		this.callback = callback;
		this.promise = promise;
	}

	public void execute (object value)
	{
		Promise returnPromise = callback (value);
		returnPromise.Then (returnPromiseResult);
	}

	protected void returnPromiseResult(object value) {
		promise.ResolvePromise (value);
	}

}