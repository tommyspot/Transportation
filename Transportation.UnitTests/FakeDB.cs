using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Transportation.UnitTests
{
    public class FakeDB : DbContext, IClarityDB
    {
        public bool SaveChangesCalled;

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<Employee> Employees { get; set; }

        public Database Database
        {
            get { return null; }
        }

        public FakeDB()
        {
            Users = new FakeDbSet<User>();
        }

        public int SaveChanges()
        {
            SaveChangesCalled = true;
            return 0;
        }

        public void Dispose()
        {
        }

        public void AddEntity<T>(T entity) where T : class
        {
            switch (typeof(T).Name)
            {
                case "User": Users.Add(entity as User); break;
                default: throw new NotImplementedException();
            }
        }

        public class FakeDbSet<T> : IDbSet<T> where T : class
        {
            private Func<IEnumerable<T>, object[], T> findFunction;

            private readonly HashSet<T> nonStaticData;


            private HashSet<T> Data
            {
                get
                {
                    if (nonStaticData == null)
                    {
                        return staticData;
                    }
                    else
                    {
                        return nonStaticData;
                    }
                }
            }

            private static readonly HashSet<T> staticData = new HashSet<T>();

            public Func<IEnumerable<T>, object[], T> FindFunction
            {
                get { return findFunction; }
                set { findFunction = value; }
            }

            /// <summary>
            /// Creates an instance of the InMemoryDbSet using the default static backing store.This means
            /// that data persists between test runs, like it would do with a database unless you
            /// cleared it down.
            /// </summary>
            public FakeDbSet()
                : this(true)
            {
            }

            /// <summary>
            /// This constructor allows you to pass in your own data store, instead of using
            /// the static backing store.
            /// </summary>
            /// <param name="data">A place to store data.</param>
            public FakeDbSet(HashSet<T> data)
            {
                nonStaticData = data;
            }

            /// <summary>
            /// Creates an instance of the InMemoryDbSet using the default static backing store.This means
            /// that data persists between test runs, like it would do with a database unless you
            /// cleared it down.
            /// </summary>
            /// <param name="clearDownExistingData"></param>
            public FakeDbSet(bool clearDownExistingData)
            {
                if (clearDownExistingData)
                {
                    Clear();
                }
            }


            public void Clear()
            {
                Data.Clear();
            }

            public T Add(T entity)
            {
                Data.Add(entity);

                return entity;
            }

            public T Attach(T entity)
            {
                Data.Add(entity);
                return entity;
            }

            public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
            {
                return Activator.CreateInstance<TDerivedEntity>();
            }

            public T Create()
            {
                return Activator.CreateInstance<T>();
            }

            public virtual T Find(params object[] keyValues)
            {
                if (FindFunction == null)
                {
                    return FindFunction(Data, keyValues);
                }
                else
                {
                    throw new NotImplementedException("Derive from InMemoryDbSet and override Find, or provide a FindFunction.");
                }
            }

            public ObservableCollection<T> Local
            {
                get { return new ObservableCollection<T>(Data); }
            }

            public T Remove(T entity)
            {
                Data.Remove(entity);

                return entity;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return Data.GetEnumerator();
            }

            private IEnumerator GetEnumerator1()
            {
                return Data.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator1();
            }

            public Type ElementType
            {
                get { return Data.AsQueryable().ElementType; }
            }

            public Expression Expression
            {
                get { return Data.AsQueryable().Expression; }
            }

            public IQueryProvider Provider
            {
                get { return Data.AsQueryable().Provider; }
            }
        }
    }
}