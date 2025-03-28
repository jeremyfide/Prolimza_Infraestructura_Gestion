using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Prolimza.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Prolimza.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Tablas o Entidades
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Canton> Cantones { get; set; }
        public DbSet<Distrito> Distritos { get; set; }
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<MateriaPrima> MateriasPrimas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<MateriaReceta> MateriasReceta { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompraProducto> DetallesCompraProductos { get; set; }
        public DbSet<DetalleCompraMateriaPrima> DetallesCompraMateriaPrimas { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVenta { get; set; }
        public DbSet<EstadoVenta> EstadosVenta { get; set; }
        public DbSet<HistorialEstadoVenta> HistorialesEstadoVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provincia>().HasKey(p => p.IdProvincia);
            modelBuilder.Entity<Canton>().HasKey(c => c.IdCanton);
            modelBuilder.Entity<Distrito>().HasKey(d => d.IdDistrito);
            modelBuilder.Entity<Bodega>().HasKey(b => b.IdBodega);
            modelBuilder.Entity<MateriaPrima>().HasKey(mp => mp.IdMateriaPrima);
            modelBuilder.Entity<Producto>().HasKey(p => p.IdProducto);
            modelBuilder.Entity<Rol>().HasKey(r => r.IdRol);
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Auditoria>().HasKey(a => a.IdAuditoria);
            modelBuilder.Entity<Receta>().HasKey(r => r.IdReceta);
            modelBuilder.Entity<Compra>().HasKey(c => c.IdCompra);
            modelBuilder.Entity<Venta>().HasKey(v => v.IdVenta);
            modelBuilder.Entity<EstadoVenta>().HasKey(ev => ev.IdEstadoVenta);
            modelBuilder.Entity<HistorialEstadoVenta>().HasKey(he => he.IdHistorialEstadoVenta);

            modelBuilder.Entity<MateriaReceta>().HasKey(mr => new { mr.IdReceta, mr.IdMateriaPrima });
            modelBuilder.Entity<DetalleCompraProducto>().HasKey(dcp => new { dcp.IdCompra, dcp.IdProducto });
            modelBuilder.Entity<DetalleCompraMateriaPrima>().HasKey(dcm => new { dcm.IdCompra, dcm.IdMateriaPrima });
            modelBuilder.Entity<DetalleVenta>().HasKey(dv => new { dv.IdVenta, dv.IdProducto });

            modelBuilder.Entity<Canton>()
                .HasOne(c => c.Provincia)
                .WithMany(p => p.Cantones)
                .HasForeignKey(c => c.IdProvincia);

            modelBuilder.Entity<Distrito>()
                .HasOne(d => d.Canton)
                .WithMany(c => c.Distritos)
                .HasForeignKey(d => d.IdCanton);

            modelBuilder.Entity<Bodega>()
                .HasOne(b => b.Distrito)
                .WithMany(d => d.Bodegas)
                .HasForeignKey(b => b.IdDistrito);

            modelBuilder.Entity<MateriaPrima>()
                .HasOne(mp => mp.Bodega)
                .WithMany(b => b.MateriasPrimas)
                .HasForeignKey(mp => mp.IdBodega);

            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Bodega)
                .WithMany(b => b.Productos)
                .HasForeignKey(p => p.IdBodega);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.IdRol);

            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.Producto)
                .WithMany()
                .HasForeignKey(a => a.IdProducto);

            modelBuilder.Entity<Auditoria>()
                .HasOne(a => a.MateriaPrima)
                .WithMany()
                .HasForeignKey(a => a.IdMateriaPrima);
        }
    }
}
