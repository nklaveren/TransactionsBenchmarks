using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;

namespace TransactionsBenchmark;

public class TransactionBenchmarks
{

    private const string connectionString = "Server=localhost,1433;Database=master;User=sa;Password=Password123;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

    public static void Setup()
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var dropTableCommand = new SqlCommand("DROP TABLE IF EXISTS Customers", connection);
        dropTableCommand.ExecuteNonQuery();
        //create table customer, id primary auto increment sql server
        using var createTableCommand = new SqlCommand("CREATE TABLE Customers (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50))", connection);
        createTableCommand.ExecuteNonQuery();
    }

    [Benchmark]
    public void Add100CustomersWithUnitOfWork()
    {
        using var unitOfWork = new UnitOfWork(connectionString);
        var customerRepository = new CustomerRepository(unitOfWork);
        var customerService = new CustomerService(unitOfWork, customerRepository);
        customerService.Add100Customers();
    }

    [Benchmark]
    public void Add100CustomersWithTransactionScope()
    {
        var customerService = new CustomerServiceScoped(connectionString);
        customerService.Add100Customers();
    }


    [Benchmark]
    public void AddCustomerWithUnitOfWork()
    {
        using var unitOfWork = new UnitOfWork(connectionString);
        var customerRepository = new CustomerRepository(unitOfWork);
        var customerService = new CustomerService(unitOfWork, customerRepository);
        customerService.AddCustomer(1, "Customer 1");
    }

    [Benchmark]
    public void AddCustomerWithTransactionScope()
    {
        var customerService = new CustomerServiceScoped(connectionString);
        customerService.AddCustomer(1, "Customer 1");
    }

    [Benchmark]
    public void RemoveCustomerWithUnitOfWork()
    {
        using var unitOfWork = new UnitOfWork(connectionString);
        var customerRepository = new CustomerRepository(unitOfWork);
        var customerService = new CustomerService(unitOfWork, customerRepository);
        customerService.RemoveCustomer(1);
    }

    [Benchmark]
    public void RemoveCustomerWithTransactionScope()
    {
        var customerService = new CustomerServiceScoped(connectionString);
        customerService.RemoveCustomer(1);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = new SqlCommand("TRUNCATE TABLE Customers", connection);
        command.ExecuteNonQuery();
    }

}
