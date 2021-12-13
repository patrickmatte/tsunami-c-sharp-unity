using System;
using System.Collections.Generic;

public interface IPromise<PromisedT> {

	void then (Action<PromisedT> action);
	void thenAll(Action<List<PromisedT>> action);

	PromisedT result {
		get;
	}

	List<PromisedT> resultAll {
		get;
	}

	void all (List<IPromise<PromisedT>> list);

	IPromise<PromisedT> resolveAll (List<PromisedT> value);

	IPromise<PromisedT> resolve (PromisedT value);

}