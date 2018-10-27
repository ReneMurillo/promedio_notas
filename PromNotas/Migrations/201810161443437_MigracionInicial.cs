namespace PromNotas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigracionInicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notas",
                c => new
                    {
                        NotaID = c.Int(nullable: false, identity: true),
                        Nota1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Nota2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Nota3 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Promedio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Estado = c.String(),
                    })
                .PrimaryKey(t => t.NotaID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notas");
        }
    }
}
