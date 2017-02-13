using System;
using System.Data.Entity;

namespace Transportation
{
    public interface IClarityDB : IDisposable
    {
        IDbSet<User> Users { get; set; }
        IDbSet<Employee> Employees { get; set; }
		IDbSet<Truck> Trucks { get; set; }
		Database Database { get; }

        void AddEntity<T>(T entity) where T : class;
        int SaveChanges();
    }
}
