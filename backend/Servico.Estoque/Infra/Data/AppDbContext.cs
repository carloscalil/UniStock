using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Servico.Estoque.Domain.Entities;

namespace Servico.Estoque.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(eb =>
            {
                eb.HasKey(p => p.Codigo);

                eb.Property(p => p.Codigo)
                .ValueGeneratedOnAdd();
            });
        }
        
    }
}