using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;
using Multas_tB.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller {

        //Cria um objeto privado, que representa a base de dados
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Agentes
        /// <summary>
        /// Lista de todos os agentes
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() {

            //db.Agentes.ToList() --> Em SQL: SELECT * FROM Agentes
            //Enviar para a View uma lista com todos os agentes, da BD

            //obter a lista de todos os agentes
            // em SQL: SELECT * FROM Agentes ORDER BY Nome;
            var listaDeAgentes = db.Agentes.ToList().OrderBy(a=>a.Nome);

            return View(listaDeAgentes);
            //return View();
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// Apresenta os detalhes de um Agente
        /// </summary>
        /// <param name="id">Representa a PK que identifica o Agente </param>
        /// <returns></returns>
        public ActionResult Details(int? id) {
            //se se escrever 'int?' é possivel
            //não fornecer o valor para o ID e não há erro

            //int? - significa que pode haver valores nulos

            //Protege a execução do método contra a Não existência de dados
            if (id == null) {
                //instrução original
                // devolve um erro qnd não há ID
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //HttpStatusCodeResult é uma exceção. Esta linha aparece automatica. p.e. quando fazemos ".../Agentes/Details/" aparece erro 400 pq n especificamos o ID.".../Agentes/Details/1000" error 404 pq n existe nenhum id 10000

                // redirecionar para uma pagina que nós controlamos
                return RedirectToAction("Index");

            }
            // vai procurar o Agente cujo ID foi fornecido
            Agentes agente = db.Agentes.Find(id);

            //Se o agente NÃO for encontrado...
            if (agente == null) {
                //O agente não foi encontrado. Logo, gera-se uma msg de erro. 
                //return HttpNotFound();

                return RedirectToAction("Index");

            }

            //Envia para a View os dados do Agente se encontrado
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] //Protege o metodo contra o ataque de roubo de entidade
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente, HttpPostedFileBase uploadFotografia) {
            //Escrever os dados de um novo Agente na BD

            //Especificar o ID do novo Agente
            //Testar se há registos na tabela dos Agentes
            //if (db.Agentes.Count()!=0) { }

            // ou então, usar a instrução TRY/CATCH
            int idNovoAgente = 0;
            try {
                idNovoAgente = db.Agentes.Max(a => a.ID) + 1;

            }
            catch (Exception) {
                idNovoAgente = 1;
            }
            //Guardar o ID do novo Agente
            agente.ID = idNovoAgente;

            //Guardar o ID do novo agente
            agente.ID = idNovoAgente;
            //Especificar (escolher) o nome do ficheiro
            string nomeImagem = "Agente_"+idNovoAgente +".jpg";

            //Var. auxiliar
            string path = "";
            //Validar se a imagem foi fornecida
            if(uploadFotografia != null) {
                //O ficheiro foi fornecida
                //Validar se o que foi fornecido é uma imagem ---> Fazer em casa.
                //Formatar o tamanho da imagem

                //Criar o caminho completo até ao sítio onde o ficheiro será guardado
                path = Path.Combine(Server.MapPath("~imagens/"), nomeImagem);

                //Guardar o nome do ficheiro na BD
                agente.Fotografia = nomeImagem;
            }
            else {
                //Não foi fornecido qq ficheiro
                //Gerar uma mensagem de erro
                ModelState.AddModelError("", "Não foi fornecida uma imagem.");
                //Devolver o controlo à View
                return View(agente);
            }
            //Escrever o ficheiro com a fotografia no disco rígido, na pasta 'imagens'


            //ModelState.IsValid --> confronta os dados fornecidos com o modelo, se não respeitar as regras
            //do modelo, rejeita os dados
            if (ModelState.IsValid) {
                try {
                    // adiciona na estrutura de dados, na memória do servidor, o objeto Agentes
                    db.Agentes.Add(agente);
                    //faz 'commit' na BD
                    db.SaveChanges();
                    //Escrever o ficheiro com a fotografia no disco rígido, na pasta
                    uploadFotografia.SaveAs(path);
                    //redireciona o utilizador para a página de inicio se correr tudo bem
                    return RedirectToAction("Index");
                }
                catch (Exception ) {
                    ModelState.AddModelError("", "Houve um erro com a criação do Novo Agente...");

                    ///Se existir uma classes chamada 'Erro.cs'
                    ///iremos registar os dados do erro.
                    /// - criar um objeto desta classe
                    /// - atribuir a esse objeto os dados do erro
                    ///   - nome do controller
                    ///   - nome do método
                    ///   - data + hora do erro
                    ///   - mensagem do erro (ex)
                    ///   - dados que se tentavam inserir
                    ///   - outros dados considerados relevante
                    /// - guardar o objeto na BD
                    /// 
                    /// - notificar um gestor do sistema, por e-amil,
                    ///   ou por outro meio, da ocurrência do erro 
                    ///   e dos seus dados
                }
             }

            //Devolve os dados do agente à View
            return View(agente);
        }
        /// <summary>
        /// edição de cada agente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id) {
            //se se escrever 'int?' é possivel
            //não fornecer o valor para o ID e não há erro

            //int? - significa que pode haver valores nulos

            //Protege a execução do método contra a Não existência de dados
            if (id == null)
            {
                //instrução original
                // devolve um erro qnd não há ID
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                // redirecionar para uma pagina que nós controlamos
                return RedirectToAction("Index");

            }
            // vai procurar o Agente cujo ID foi fornecido
            Agentes agente = db.Agentes.Find(id);

            //Se o agente NÃO for encontrado...
            if (agente == null) {
                //O agente não foi encontrado. Logo, gera-se uma msg de erro. 
                //return HttpNotFound();

                return RedirectToAction("Index");
            }
            //Envia para a View os dados do Agente se encontrado
            return View(agente);
        }

        // POST: Agentes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid) {
                // atualiza os dados do Agente, na estrutura de dados em memória
                db.Entry(agentes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        /// <summary>
        /// apresenta na view os dados de um agente,
        /// com vista à sua eventual 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int? id) {

            //Verificar se foi fornecido um ID válido
            if (id == null) {
                return RedirectToAction("Index");
            }

            //Pesquisar pelo Agente cujo ID foi fornecido
            Agentes agente = db.Agentes.Find(id);

            //Verificar se o Agente foi encontrado
            if (agente == null) {
                //O agente não existe
                //Redirecionar para a página atual
                return RedirectToAction("Index");
            }
            
            //Retornar para a view os dados do agente
            return View(agente);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
          Agentes agente = db.Agentes.Find(id);
            try {
                //Procurar o agente
               
                //Remover da memória
                db.Agentes.Remove(agente);
                //Commit na BD
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex) {
               ModelState.AddModelError("", string.Format("Não é possível apagar o Agente nº {0} - {1}, porque há multas associadas a ele...", 
                                            id,agente.Nome)
                   );
            }
            //Se cheguei aqui é pq houve um problema
            //Devolvo os dados do Agente à View
            return View(agente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
