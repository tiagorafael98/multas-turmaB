using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class MultasDb : DbContext { //Classe especial q representa uma base de dados. C# AGAIN

        public MultasDb() : base("name=MultasDBConnectionString") { }

        //Descrever os nomes das tabelas na Base de Dados
        public virtual DbSet<Multas> Multas { get; set; } //Multas em azul é o nome da claase. Multas em branco é o atributo. Tabela multas.
        public virtual DbSet<Condutores> Condutores { get; set; } //Tabela Concudores
        public virtual DbSet<Agentes> Agentes { get; set; } //Tabela Agentes
        public virtual DbSet<Viaturas> Viaturas { get; set; } //Tabela Viaturas


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }


    }
}