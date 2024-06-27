namespace TransactionsBenchmark;

public class CustomerService(IUnitOfWork unitOfWork, IRepository<Customer> customerRepository) : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRepository<Customer> _customerRepository = customerRepository;

    public void RemoveCustomer(int id)
    {
        var customer = new Customer(id, default);
        _customerRepository.Remove(customer);
        _unitOfWork.Commit();
    }

    public void Add100Customers()
    {
        for (int i = 1; i <= 100; i++)
        {
            var customer = new Customer(i, $"Customer {i}");
            _customerRepository.Add(customer);
        }
        _unitOfWork.Commit();
    }

    public void AddCustomer(int id, string name)
    {
        var customer = new Customer(id, name);
        _customerRepository.Add(customer);
        _unitOfWork.Commit();
    }

}
