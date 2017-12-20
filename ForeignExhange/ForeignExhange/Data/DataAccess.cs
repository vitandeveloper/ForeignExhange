// esta clase maneja todas las operaciones que se ralizan en la BD

namespace ForeignExhange.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Models;
    using SQLiteNetExtensions.Extensions;
    using Xamarin.Forms;
    using SQLite.Net;

    public class DataAccess : IDisposable
    {
        SQLiteConnection connection;

        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(config.Platform,
                System.IO.Path.Combine(config.DirectoryDB, "ForeignExchange.db3"));
            connection.CreateTable<Rate>();
        }

        public void Insert<T>(T model)
        {
            connection.Insert(model);
        }

        public void Update<T>(T model)
        {
            connection.Update(model);
        }

        public void Delete<T>(T model)
        {
            connection.Delete(model);
        }

        public T First<T>(bool WithChildren) where T : class
        {
            if (WithChildren)
            {
                //return connection.GetAllWithChildren<T>().FirstOrDefault();
                return connection.Table<T>().FirstOrDefault();
            }
            else
            {
                return connection.Table<T>().FirstOrDefault();
            }
        }

        public List<T> GetList<T>(bool WithChildren) where T : class
        {
            if (WithChildren)
            {

                //return connection.GetAllWithChildren<T>().ToList();
                return connection.Table<T>().ToList();
            }
            else
            {
                return connection.Table<T>().ToList();
            }
        }

        public T Find<T>(int pk, bool WithChildren) where T : class
        {
            if (WithChildren)
            {

                //return connection.GetAllWithChildren<T>().FirstOrDefault(m => m.GetHashCode() == pk);
                return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
            }
            else
            {
                return connection.Table<T>().FirstOrDefault(m => m.GetHashCode() == pk);
            }
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
