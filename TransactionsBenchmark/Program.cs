using BenchmarkDotNet.Running;
using TransactionsBenchmark;

TransactionBenchmarks.Setup();

var summary = BenchmarkRunner.Run<TransactionBenchmarks>();
