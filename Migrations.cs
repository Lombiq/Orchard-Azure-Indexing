using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lombiq.Hosting.Azure.Lucene.Models;
using Orchard.Data.Migration;

namespace Lombiq.Hosting.Azure.Lucene
{
    public class Migrations : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable(typeof(AppDataFileRecord).Name,
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Path", column => column.NotNull().Unique().WithLength(1024))
                    .Column<string>("Content", column => column.Unlimited())
                )
            .AlterTable(typeof(AppDataFileRecord).Name,
                table => table
                    .CreateIndex("Path", "Path"));


            return 1;
        }
    }
}