using System;
using System.Collections.Generic;

public class RouterTransition {

	public List<IBranch> branches = new List<IBranch>();
	public List<RouterTask> tasks = new List<RouterTask>();
	public Router router;
	public string name;

	public RouterTransition(Router router, string name) {
		this.router = router;
		this.name = name;
	}

	public Promise start() {
		Promise promise = Promise.Resolve();
		if (branches.Count > 0) {
			promise = new Promise((resolve, reject) =>
			{
				RouterTask nextTask = null;
				for (int i = tasks.Count - 1; i > -1; i--)
				{
					RouterTask task = tasks[i];
					task.router = router;
					task.branches = new List<IBranch>();
					foreach (IBranch branch in branches)
					{
						task.branches.Add(branch);
					}
					if (nextTask != null)
					{
						task.onComplete = nextTask.start;
					}
					else
					{
						task.onComplete = resolve;
					}
					nextTask = task;
				}
				RouterTask firstTask = tasks[0];
				firstTask.start();
			});
		}
		return promise;
	}

}
