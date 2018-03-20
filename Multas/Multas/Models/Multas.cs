using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Multas.Models
{
    public class Multas
    {
        [Key]
        public int ID { get; set; } //Primary Key

        public string Infracao { get; set; }

        public string LocalDaMulta { get; set; }

        public decimal ValorMulta { get; set; }

        public DateTime DataDaMulta { get; set; }

        //************************************************************
        //Representar as chaves forasteiras que relacionam esta classe
        // com as outras classes
        //************************************************************

        //FK para a tabela dos condutores. AGORA É LINGUAGEM C#

        //... CONSTRAINT NomeDaTermo
        // FOREIGN KEY NomeDoAtributo REFERENCES NomeDaTabela(NomeDoAtributoPK)

        [ForeignKey("Condutor")]
        public int CondutorFK { get; set; }
        public virtual Condutores Condutor { get; set; } //isto (ll.33-35) vai fazer o mm q tá nas linhas 30 e 31

        //FK para as viaturas
        [ForeignKey("Viatura")]
        public int ViaturaFK { get; set; }
        public virtual Viaturas Viatura { get; set; }

        //FK para os Agentes
        [ForeignKey("Agente")]
        public int AgenteFK { get; set; }
        public virtual Agentes Agente { get; set; }

        //NOTA: O "virtual" é lazy loading ("carregamento preguiçoso"). Só vai ser carregado na memória da máquina
        // se forem utilizados. Se ver q ñ precisa, ñ carrega. Ent, a maquina faz esse trabalho por nós.

    }
}