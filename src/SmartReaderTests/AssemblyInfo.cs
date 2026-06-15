using Xunit;

// The performance tests (PerformanceTests) assert on wall-clock time. Under xUnit's default
// parallel execution they compete with the rest of the suite for CPU, which inflates those
// timings and makes the "does not hang" assertions flaky. Run the whole assembly serially so
// the timing-sensitive tests get an uncontended measurement.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
