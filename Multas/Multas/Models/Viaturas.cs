using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Viaturas {

        public Viaturas() { //Construtor
            ListaDeMultas = new HashSet<Multas>(); //Vai carregar à viatura o possível nº de multas da viatura. C# again.
        }

        [Key]
        public int ID { get; set; } //primary key

        //Dados de uma matrícula
        public string Matricula { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Cor { get; set; }

        //Dados do dono de uma Viatura
        public string NomeDono { get; set; }

        public string MoradaDono { get; set; }

        public string CodPostalDono { get; set; }

        //Referência às multas q um condutor 'recebe' numa viatura
        public virtual ICollection <Multas > ListaDeMultas { get; set; }



    }
}