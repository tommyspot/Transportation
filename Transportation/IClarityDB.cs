using System;
using System.Data.Entity;

namespace Transportation
{
    public interface IClarityDB : IDisposable
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Employee> Employees { get; set; }
		IDbSet<Truck> Trucks { get; set; }
		IDbSet<Customer> Customers { get; set; }
		IDbSet<Wagon> Wagons { get; set; }
        IDbSet<WagonSettlement> WagonSettlements { get; set; }
		IDbSet<Payment> Payments { get; set; }

        IDbSet<Product> Products { get; set; }
        IDbSet<ProductInput> ProductInputs { get; set; }
        IDbSet<Inventory> Inventories { get; set; }
        IDbSet<Order> Orders { get; set; }
        IDbSet<OrderDetail> OrderDetails { get; set; }
        IDbSet<InputOrder> InputOrders { get; set; }
        Database Database { get; }

        void AddEntity<T>(T entity) where T : class;
        int SaveChanges();
    }
}
