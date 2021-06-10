using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQToolkit.Data;

namespace IQToolkit.Extensions
{
    public interface ICustomDbOwner
    {

        DbEntityProvider DbOwner
        {
            get;
        }
        void Attach(DbEntityProvider dbprovider);
        void Detach();
    }
}
