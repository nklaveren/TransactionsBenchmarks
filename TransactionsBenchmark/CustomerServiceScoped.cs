using System.Data.Common;
using System.Transactions;
using Microsoft.Data.SqlClient;
using TransactionsBenchmark;

public interface ICustomerService
{
    void AddCustomer(int id, string name);
    void RemoveCustomer(int id);
    void Add100Customers();
}

public class CustomerServiceScoped(string connectionString) : ICustomerService
{
    private const string INSERT = "INSERT INTO Customers (Name) VALUES (@Name)";
    private const string DELETE = "DELETE FROM Customers WHERE Id = @Id";
    private readonly string _connectionString = connectionString;

    public void AddCustomer(int id, string name)
    {
        using var scope = new TransactionScope();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using (var command = new SqlCommand(INSERT, connection))
        {
            command.Parameters.AddWithValue("@Name", name);
            command.ExecuteNonQuery();
        }
        scope.Complete();
    }

    public void RemoveCustomer(int id)
    {
        using var scope = new TransactionScope();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using (var command = new SqlCommand(DELETE, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
        scope.Complete();
    }

    public void Add100Customers()
    {
        using var scope = new TransactionScope();
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        for (int i = 1; i <= 100; i++)
        {
            using var command = new SqlCommand(INSERT, connection);
            command.Parameters.AddWithValue("@Name", $"Customer {i}");
            command.ExecuteNonQuery();
        }
        scope.Complete();
    }
}
