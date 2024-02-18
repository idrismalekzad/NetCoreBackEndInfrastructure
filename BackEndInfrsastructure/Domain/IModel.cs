using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndInfrsastructure.Domain
{
    public interface IModel<PrimaryKey> where PrimaryKey : struct
    {
        public PrimaryKey ID { get; set; }
    }
}
