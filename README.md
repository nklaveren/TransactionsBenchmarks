<!-- create a default readme to describe this project... where i'm create a benchmark to compare the implementation, and i can see small diferences between unit of work implementation vs using TransactionScope class... -->

<!-- title: -->
# Benchmarking Unit of Work vs TransactionScope

<!-- TOC -->
- [Benchmarking Unit of Work vs TransactionScope](#benchmarking)



``` ini

BenchmarkDotNet=v0.13.1, OS=ubuntu 22.04
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 10 physical cores
.NET SDK=8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT



|                              Method |      Mean |     Error |    StdDev |    Median |
|------------------------------------ |----------:|----------:|----------:|----------:|
|       Add100CustomersWithUnitOfWork | 50.117 ms | 0.5944 ms | 0.5269 ms | 50.171 ms |
| Add100CustomersWithTransactionScope | 49.545 ms | 0.6248 ms | 0.5845 ms | 49.424 ms |
|           AddCustomerWithUnitOfWork |  3.177 ms | 0.0553 ms | 0.0490 ms |  3.166 ms |
|     AddCustomerWithTransactionScope |  2.293 ms | 0.0456 ms | 0.0488 ms |  2.276 ms |
|        RemoveCustomerWithUnitOfWork |  2.197 ms | 0.0324 ms | 0.0303 ms |  2.195 ms |
|  RemoveCustomerWithTransactionScope |  1.454 ms | 0.0334 ms | 0.0946 ms |  1.424 ms |


```