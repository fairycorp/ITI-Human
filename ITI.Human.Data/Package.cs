﻿using CK.Setup;
using CK.SqlServer.Setup;

namespace CKS.Data
{
    [SqlPackage(
    ResourcePath = "Res",
    Schema = "CK",
    Database = typeof(SqlDefaultDatabase),
    ResourceType = typeof(Package)),
    Versions("1.0.0")]
    public abstract class Package : SqlPackage
    {
        void StobjConstruct(
            )
        {

        }
    }
}
