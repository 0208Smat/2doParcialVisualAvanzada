Imports System
Imports InventarioApiVB.Data
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.EntityFrameworkCore.Infrastructure
Imports Microsoft.EntityFrameworkCore.Metadata
Imports Microsoft.EntityFrameworkCore.Migrations

Namespace Migrations
    <DbContext(GetType(InventarioDbContext))>
    <Migration("20261118000000_InitialCreate")>
    Partial Public Class InitialCreate
        Protected Overrides Sub BuildTargetModel(modelBuilder As ModelBuilder)
            modelBuilder _
                .HasAnnotation("ProductVersion", "8.0.11") _
                .HasAnnotation("Relational:MaxIdentifierLength", 128)

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder)

            modelBuilder.Entity("InventarioApiVB.Models.Categoria",
                Sub(b)
                    b.Property(Of Integer)("Id") _
                        .ValueGeneratedOnAdd() _
                        .HasColumnType("int")

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property(Of Integer)("Id"))

                    b.Property(Of String)("Descripcion") _
                        .HasMaxLength(200) _
                        .HasColumnType("nvarchar(200)")

                    b.Property(Of String)("Nombre") _
                        .IsRequired() _
                        .HasMaxLength(50) _
                        .HasColumnType("nvarchar(50)")

                    b.HasKey("Id")
                    b.HasIndex("Nombre").IsUnique()
                    b.ToTable("Categorias", DirectCast(Nothing, String))
                End Sub)

            modelBuilder.Entity("InventarioApiVB.Models.Producto",
                Sub(b)
                    b.Property(Of Integer)("Id") _
                        .ValueGeneratedOnAdd() _
                        .HasColumnType("int")

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property(Of Integer)("Id"))

                    b.Property(Of Integer)("CategoriaId") _
                        .HasColumnType("int")

                    b.Property(Of String)("Descripcion") _
                        .HasMaxLength(200) _
                        .HasColumnType("nvarchar(200)")

                    b.Property(Of DateTime)("FechaRegistro") _
                        .HasColumnType("datetime2")

                    b.Property(Of String)("Nombre") _
                        .IsRequired() _
                        .HasMaxLength(50) _
                        .HasColumnType("nvarchar(50)")

                    b.Property(Of Decimal)("Precio") _
                        .HasColumnType("decimal(18,2)")

                    b.Property(Of Integer)("Stock") _
                        .HasColumnType("int")

                    b.HasKey("Id")
                    b.HasIndex("CategoriaId")
                    b.HasIndex("Nombre").IsUnique()
                    b.ToTable("Productos", DirectCast(Nothing, String))
                End Sub)

            modelBuilder.Entity("InventarioApiVB.Models.Producto",
                Sub(b)
                    b.HasOne("InventarioApiVB.Models.Categoria", "Categoria") _
                        .WithMany("Productos") _
                        .HasForeignKey("CategoriaId") _
                        .OnDelete(DeleteBehavior.Restrict) _
                        .IsRequired()

                    b.Navigation("Categoria")
                End Sub)

            modelBuilder.Entity("InventarioApiVB.Models.Categoria",
                Sub(b)
                    b.Navigation("Productos")
                End Sub)
        End Sub
    End Class
End Namespace
