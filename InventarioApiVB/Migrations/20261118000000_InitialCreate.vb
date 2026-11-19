Imports System
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Migrations
    Partial Public Class InitialCreate
        Inherits Migration

        Protected Overrides Sub Up(migrationBuilder As MigrationBuilder)
            migrationBuilder.CreateTable(
                name:="Categorias",
                columns:=Function(table) New With {
                    .Id = table.Column(Of Integer)(type:="int", nullable:=False).Annotation("SqlServer:Identity", "1, 1"),
                    .Nombre = table.Column(Of String)(type:="nvarchar(50)", maxLength:=50, nullable:=False),
                    .Descripcion = table.Column(Of String)(type:="nvarchar(200)", maxLength:=200, nullable:=True)
                },
                constraints:=Sub(table)
                                 table.PrimaryKey("PK_Categorias", Function(x) x.Id)
                             End Sub)

            migrationBuilder.CreateTable(
                name:="Productos",
                columns:=Function(table) New With {
                    .Id = table.Column(Of Integer)(type:="int", nullable:=False).Annotation("SqlServer:Identity", "1, 1"),
                    .Nombre = table.Column(Of String)(type:="nvarchar(50)", maxLength:=50, nullable:=False),
                    .Descripcion = table.Column(Of String)(type:="nvarchar(200)", maxLength:=200, nullable:=True),
                    .Precio = table.Column(Of Decimal)(type:="decimal(18,2)", nullable:=False),
                    .Stock = table.Column(Of Integer)(type:="int", nullable:=False),
                    .FechaRegistro = table.Column(Of DateTime)(type:="datetime2", nullable:=False),
                    .CategoriaId = table.Column(Of Integer)(type:="int", nullable:=False)
                },
                constraints:=Sub(table)
                                 table.PrimaryKey("PK_Productos", Function(x) x.Id)
                                 table.ForeignKey(
                                     name:="FK_Productos_Categorias_CategoriaId",
                                     column:=Function(x) x.CategoriaId,
                                     principalTable:="Categorias",
                                     principalColumn:="Id",
                                     onDelete:=ReferentialAction.Restrict)
                             End Sub)

            migrationBuilder.CreateIndex(
                name:="IX_Categorias_Nombre",
                table:="Categorias",
                column:="Nombre",
                unique:=True)

            migrationBuilder.CreateIndex(
                name:="IX_Productos_CategoriaId",
                table:="Productos",
                column:="CategoriaId")

            migrationBuilder.CreateIndex(
                name:="IX_Productos_Nombre",
                table:="Productos",
                column:="Nombre",
                unique:=True)
        End Sub

        Protected Overrides Sub Down(migrationBuilder As MigrationBuilder)
            migrationBuilder.DropTable(name:="Productos")
            migrationBuilder.DropTable(name:="Categorias")
        End Sub
    End Class
End Namespace
