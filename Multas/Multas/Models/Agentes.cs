using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Agentes {
        public Agentes()
        { //Construtor
            ListaDeMultas = new HashSet<Multas>();
        }

        //Descrição dos atributos (ID, Nome, esquadra, fotografia)

        [Key]
        //se n disser + nada, isto vai ser a chave primária de agente
        public int ID { get; set; }

        public string Nome { get; set; } //nome do agente

        public string Esquadra { get; set; }

        public string Fotografia { get; set; }

        //Referência às multas q um agente 'emite'
        public virtual ICollection<Multas> ListaDeMultas { get; set; }

    }
}