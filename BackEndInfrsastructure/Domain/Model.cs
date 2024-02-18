using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndInfrsastructure.Domain
{
    public class Model<PrimaryKey> : IModel<PrimaryKey> where PrimaryKey : struct
    {
        [Column(Order = 0)]
        public virtual PrimaryKey ID { get; set; }
    }
}
