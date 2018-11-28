using CK.DB.Actor;
using CK.Setup;
using CK.SqlServer.Setup;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Human.Data
{
    [SqlTable("tUserBalance", Package = typeof(Package))]
    [Versions("1.0.0")]
    public abstract class UserBalanceTable : SqlTable
    {
        void StObjConstruct(UserTable uTable)
        {

        }
    }
}
