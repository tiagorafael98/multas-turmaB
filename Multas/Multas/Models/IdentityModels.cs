using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Multas.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multas_tB.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

        /// <summary>
        /// Claase para efetuar a geração de utilizadores
        /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    /// <summary>
    /// Classe responsável pela criação da base de dados
    ///   - da autenticação
    ///   - do 'negócio' (BD q está associada ao sistema propriamente dito)
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MultasDBConnectionString", throwIfV1Schema: true)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //Aqui, vamos colocar as intruções relativas às tabelas do 'negócio'

        //Descrever os nomes das tabelas na Base de Dados
        public virtual DbSet<Multas.Models.Multas> Multas { get; set; } //Tabela multas
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