using Microsoft.Data.SqlClient;

namespace TransactionsBenchmark;

public interface IUnitOfWork : IDisposable
{
    void Commit();
    void Rollback();
}
public interface IRepository<T>
{
    void Add(T entity);
    void Remove(T entity);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly SqlConnection _connection;
    private SqlTransaction _transaction;
    private bool _disposed;

    public UnitOfWork(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }

    public SqlConnection Connection => _connection;
    public SqlTransaction Transaction => _transaction;

    public void Commit()
    {
        _transaction?.Commit();
        _transaction = _connection.BeginTransaction();
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction = _connection.BeginTransaction();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

public class CustomerRepository(UnitOfWork unitOfWork) : IRepository<Customer>
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;

    public void Add(Customer entity)
    {
        using var command = new SqlCommand("INSERT INTO Customers (Name) VALUES (@Name)", _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.ExecuteNonQuery();
    }

    public void Remove(Customer entity)
    {
        using var command = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", _unitOfWork.Connection, _unitOfWork.Transaction);
        command.Parameters.AddWithValue("@Id", entity.Id);
        command.ExecuteNonQuery();
    }
}

