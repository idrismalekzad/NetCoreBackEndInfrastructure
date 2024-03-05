
using BackEndInfrsastructure.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiExample.DB.Data.Domain;

namespace WebApiExample.Data.Entities
{
    [Table("WEBSERVICEUSER")]
    public class WebServiceUserEntity : WebServiceUser
    {
        public WebServiceUserEntity()
        {
                
        }

    }
}
