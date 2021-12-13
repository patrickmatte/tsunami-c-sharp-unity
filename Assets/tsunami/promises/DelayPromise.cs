using System.Threading.Tasks;

public class DelayPromise:Promise {
	
	private readonly int millisecondsDelay;

	public DelayPromise(int millisecondsDelay) {
		this.millisecondsDelay = millisecondsDelay;
		Start();
	}

	async void Start() {
		await Task.Delay(millisecondsDelay);
		ResolvePromise(millisecondsDelay);
	}

}