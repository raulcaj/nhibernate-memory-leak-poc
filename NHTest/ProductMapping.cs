using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHTest
{
    public class ProductMapping : ClassMap<Product>
    {
        public ProductMapping()
        {
            Table("product");
            Id(a => a.Id).GeneratedBy.Guid();
            Map(a => a.Name);
            Map(a => a.Category);
            Map(a => a.Discontinued);
        }
    }
}
