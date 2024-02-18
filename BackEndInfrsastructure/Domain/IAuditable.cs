using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndInfrsastructure.Domain
{
    public interface IAuditable<PrimaryKey> : IModel<PrimaryKey>
       where PrimaryKey : struct
    {
        string CreatedBy { get; set; }
        string LastModifiedBy { get; set; }
        DateTime CreationTime { get; set; }
        DateTime LastModificationTime { get; set; }
    }
}
