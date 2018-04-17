using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Key] //É uma anotação q vai influenciar o comportamento deste atributo
        //se n disser + nada, isto vai ser a chave primária de agente. Define a chave primária
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required(ErrorMessage ="O {0} é de preenchimento obrigatório")] //Estou a dizer q o atributo Nome é de preenchimento obrigatório. O {0} faz referência ao nome do atributo, que, neste caso, é "Nome".
        [RegularExpression("[A-ZÂ][a-záéíóúãõàèìòùâêîôûäëïöüç]+(( | de | da | dos | d' |-)[A-ZÂ][a-záéíóúãõàèìòùâêîôûäëïöüç]+){1,4}", ErrorMessage ="O nome apenas aceita letras. Cada começa começa com maiúscula e o restante minúsculas.")]
        [StringLength(40)]
        public string Nome { get; set; } //nome do agente

        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        //[RegularExpression("[A-ZÂ]*[a-záéíóúãõàèìòùâêîôûäëïöüç -]*", ErrorMessage = "")]
        public string Esquadra { get; set; }

        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        public string Fotografia { get; set; }

        //Referência às multas q um agente 'emite'
        public virtual ICollection<Multas> ListaDeMultas { get; set; }

    }
}