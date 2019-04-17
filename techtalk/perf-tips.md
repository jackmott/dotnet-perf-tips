## Performance Takeaways

### Memory
* Ram is slow
* Move through memory in order to leverage the CPU caches
* Avoid allocations on the heap, less GC pressure, more data locality 

### Iterating
* Contiguos iterating is faster (List, Array, of structs when it makes sense)
	* In hot paths, think carefully about data layout in memory
* Iterating directly over the collection is faster than IEnumerable
	* In hot paths consider if flexibility of IEnumerable is worth the cost
* Linq has significant overhead
	* In hot paths, consider if conciseness and composabiliy of Linq is worth the cost
* Plain for loops are faster than foreach (except arrays)


### List\<T\>
* A great default collection. Array backed, contiguous data, flexible, familiar
* If you know a minimum bound on the size of a List, set that in the constructor:  `new List<T>(500); // at least 500 items`
  * This prevents numerous allocations and copies as the List grows when you call add
  * Good to keep in mind when using Linq, as Linq will not automatically do this for you (except sometimes in .NET core)

### Linq
* When you use Linq, usually best to use it all the way through a workflow
  *  Avoid: `collection.Where(foo).ToList().Select(bar);    // unecessary intermediate allocation and extera iteration`
* But sometimes generating an intermediate collection **is** a good idea
```  
  var expensiveEnumerable = collection.Where(expensiveFunction)
  var result1 = expensiveEnumerable.Reduce(foo);
  var result2 = expensiveEnumerable.Reduce(bar);
  // You just performed the expensive Where clause twice!
  var intermediateCollection = collection.Where(expensiveFunction).ToList();
```
* Think carefully about how your linq will be composed down the line. 
	* Good to comment these considerations:  `// This hits the database, don't enumerate more than once!`
*  Minimize the number of calls:  collection.Where(foo).Where(bar) is slower than collection.Where(foo && bar)
*  When you want to check if a collection is under/equal to a certain size:
	* `collection.Count() < N` //iterates over the entire collection, no matter how small N
    * `collection.Take(N).Count() < N;  collection.Take(N+1).Count() == 1;  //avoids iterating over the entire collection using just Count()`

### Reflection
* Reflection can be surprisingly slow, avoid it in frequently hit endpoints
* Often can be replaced with a switch statement + tests to get the same safety gaurantees

### Measure
* Performance can be uintuitive, for critical code paths, measure any clever ideas you have
   * BenchmarkDotNet is a nice library for measuring time and allocations
* In Visual Studio - Debug -> Performance Profiler to find slow spots in your code


