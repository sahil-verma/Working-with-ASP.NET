namespace FirstApplication.Migrations.DataContext
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AddDefaultValues : DbMigration
    {
        public override void Up()
        {
            List<string> tables = new List<string>();
            tables.Add("Rating");
            tables.Add("Movie");
            tables.Add("MovieActor");
            tables.Add("Actor");

            foreach (var table in tables)
            {
                string tablename = String.Format("dbo.{0}s", table);
                string primarykey = String.Format("{0}Id", table);

                AlterColumn(tablename, primarykey, x => x.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"));
                AlterColumn(tablename, "CreateDate", x => x.DateTime(nullable: false, defaultValueSql: "getutcdate()"));
                AlterColumn(tablename, "EditDate", x => x.DateTime(nullable: false, defaultValueSql: "getutcdate()"));
            }
        }   

        public override void Down()
        {
        }
    }
}
