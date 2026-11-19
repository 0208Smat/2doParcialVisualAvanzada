Imports InventarioApiVB.Models
Imports Microsoft.EntityFrameworkCore

Namespace Data
    Public Class InventarioDbContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of InventarioDbContext))
            MyBase.New(options)
        End Sub

        Public Property Categorias As DbSet(Of Categoria)
        Public Property Productos As DbSet(Of Producto)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            MyBase.OnModelCreating(modelBuilder)

            ' Configuración de la entidad Categoria
            modelBuilder.Entity(Of Categoria)(
                Sub(entity)
                    entity.ToTable("Categorias")
                    entity.HasKey(Function(c) c.Id)

                    entity.Property(Function(c) c.Nombre) _
                        .IsRequired() _
                        .HasMaxLength(50)

                    entity.HasIndex(Function(c) c.Nombre) _
                        .IsUnique()

                    entity.Property(Function(c) c.Descripcion) _
                        .HasMaxLength(200)
                End Sub)

            ' Configuración de la entidad Producto y relación 1:N con Categoria
            modelBuilder.Entity(Of Producto)(
                Sub(entity)
                    entity.ToTable("Productos")
                    entity.HasKey(Function(p) p.Id)

                    entity.Property(Function(p) p.Nombre) _
                        .IsRequired() _
                        .HasMaxLength(50)

                    entity.HasIndex(Function(p) p.Nombre) _
                        .IsUnique()

                    entity.Property(Function(p) p.Descripcion) _
                        .HasMaxLength(200)

                    entity.Property(Function(p) p.Precio) _
                        .IsRequired() _
                        .HasColumnType("decimal(18,2)")

                    entity.Property(Function(p) p.Stock) _
                        .IsRequired()

                    entity.Property(Function(p) p.FechaRegistro) _
                        .IsRequired()

                    entity.HasOne(Function(p) p.Categoria) _
                        .WithMany(Function(c) c.Productos) _
                        .HasForeignKey(Function(p) p.CategoriaId) _
                        .OnDelete(DeleteBehavior.Restrict)
                End Sub)
        End Sub
    End Class
End Namespace
