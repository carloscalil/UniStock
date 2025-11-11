using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servico.Faturamento.Domain.Entities;

namespace Servico.Faturamento.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<ItemNotaFiscal> ItensNotaFiscal { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotaFiscal>(eb =>
            {
                eb.HasKey(n => n.Numero); 
                
                eb.Property(n => n.Numero).ValueGeneratedOnAdd(); 
                
                eb.Property(n => n.Status).HasConversion<int>(); 

                eb.HasMany(n => n.Itens)
                  .WithOne(i => i.NotaFiscal)
                  .HasForeignKey(i => i.NotaFiscalNumero);
            });

            modelBuilder.Entity<ItemNotaFiscal>(eb =>
            {
                eb.HasKey(i => i.Id);

                eb.Property(i => i.Id).ValueGeneratedOnAdd();
            });
        }
    }
}