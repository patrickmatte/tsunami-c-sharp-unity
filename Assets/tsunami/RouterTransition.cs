using System;
using System.Collections.Generic;

public class RouterTransition {

	public List<Branch> branches = new List<Branch>();
	public List<RouterTask> tasks = new List<RouterTask>();
	public Router router;
	public string name;
	public Action onComplete;

	public RouterTransition(Router router, string name, Action onComplete) {
		this.router = router;
		this.name = name;
		this.onComplete = onComplete;
	}

	public void start() {
		if (branches.Count > 0) {
			RouterTask nextTask = null;
			for (int i = tasks.Count - 1; i > -1; i--) {
				RouterTask task = tasks[i];
				task.router = router;
				task.branches = new List<Branch> ();
				foreach (Branch branch in branches) {
					task.branches.Add(branch);
				}
				if (nextTask != null) {
					task.onComplete = nextTask.start;
				} else {
					task.onComplete = tasksComplete;
				}
				nextTask = task;
			}
			RouterTask firstTask = tasks[0];
			firstTask.start();
		} else {
			tasksComplete();
		}
	}

	public void tasksComplete() {
		onComplete();
	}

}
