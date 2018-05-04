using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using NHibernate;
using NHibernate.Util;

namespace NHTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration _cfg = null;
            using (var factory = Fluently.Configure().Database(SQLiteConfiguration.Standard.UsingFile("nhtestdb"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Product>())
           .ExposeConfiguration(cfg => _cfg = cfg)
           .BuildSessionFactory())
            {
                using (var session = factory.OpenSession())
                {
                    CreateWeakReference(session);
                }
            }
            Console.ReadLine();
        }

        private static void CreateWeakReference(ISession session)
        {
            for (int i = 0; i < 43000; ++i)
            {
                var product = session.Query<Product>().Where<Product>(p => p.Category == Convert.ToString(i)).ToList().SingleOrDefault();
                //Console.WriteLine(product.Name);
            }
        }

        private static void InitialDbConfig(Configuration _cfg, NHibernate.ISessionFactory factory)
        {
            using (var session = factory.OpenSession())
            {
                new SchemaExport(_cfg).Execute(true, true, false, session.Connection, null);
                for (int i = 0; i < 100000; ++i)
                {
                    using (var transacion = session.BeginTransaction())
                    {
                        Product p = new Product();
                        p.Name = Guid.NewGuid().ToString();
                        p.Category = Convert.ToString(i);
                        p.Discontinued = false;
                        session.Save(p);
                        transacion.Commit();
                        session.Evict(p);
                    }
                }
            }
        }


    }
}
