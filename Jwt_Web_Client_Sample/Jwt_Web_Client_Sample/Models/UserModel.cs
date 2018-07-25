using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt_Web_Client_Sample.Models 
{
    public class UserModel : IModel
    {
        public object this[string name] => "sdf";

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? Dob { get; set; }

        public IAnnotation FindAnnotation(string name)
        {
            return null;
        }

        public IEntityType FindEntityType(string name)
        {
            return null;
        }

        public IEntityType FindEntityType(string name, string definingNavigationName, IEntityType definingEntityType)
        {
            return null;
            //throw new NotImplementedException();
        }

        public IEnumerable<IAnnotation> GetAnnotations()
        {
            return null;
            //throw new NotImplementedException();
        }

        public IEnumerable<IEntityType> GetEntityTypes()
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}
