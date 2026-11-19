Imports ProductosAPI.Models
Imports Microsoft.EntityFrameworkCore

Namespace Data
    Public Class ApplicationDbContext
        Inherits DbContext
        Public Sub New(options As DbContextOptions(Of ApplicationDbContext))
            MyBase.New(options)
        End Sub
        Public Property Productos As DbSet(Of Producto)
        Public Property Categorias As DbSet(Of Categoria)
        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Categoria)(Sub(e)
                e.ToTable("Categorias")
                e.HasKey(Function(c) c.Id)
                e.Property(Function(c) c.Nombre).HasMaxLength(50).IsRequired()
                e.Property(Function(c) c.Descripcion).HasMaxLength(200)
                e.HasIndex(Function(c) c.Nombre).IsUnique()
            End Sub)
            modelBuilder.Entity(Of Producto)(Sub(e)
                e.ToTable("Productos")
                e.HasKey(Function(p) p.Id)
                e.Property(Function(p) p.Nombre).HasMaxLength(50).IsRequired()
                e.Property(Function(p) p.Descripcion).HasMaxLength(200)
                e.Property(Function(p) p.Precio).HasColumnType("decimal(18,2)")
                e.Property(Function(p) p.FechaRegistro).HasDefaultValueSql("SYSUTCDATETIME()")
                e.HasIndex(Function(p) p.Nombre).IsUnique()
                e.HasOne(Function(p) p.Categoria).WithMany(Function(c) c.Productos).HasForeignKey(Function(p) p.CategoriaId).OnDelete(DeleteBehavior.Restrict)
            End Sub)
        End Sub
    End Class
End Namespace
