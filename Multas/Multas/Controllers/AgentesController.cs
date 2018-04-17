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

namespace Multas.Controllers
{
    public class AgentesController : Controller {

        //Cria um objeto privado, que representa a base de dados
        private MultasDb db = new MultasDb();

        // GET: Agentes
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
            int idNovoAgente = db.Agentes.Max(a => a.ID) + 1;
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
            // adiciona na estrutura de dados, na memória do servidor, o objeto Agentes
                db.Agentes.Add(agente);
                //faz 'commit' na BD
                db.SaveChanges();
                //Escrever o ficheiro com a fotografia no disco rígido, na pasta
                uploadFotografia.SaveAs(path);
                //redireciona o utilizador para a página de inicio
                return RedirectToAction("Index");
            }

            //Devolve os dados do agente à View
            return View(agente);
        }

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
            //Procurar o agente
            Agentes agentes = db.Agentes.Find(id);
            //Remover da memória
            db.Agentes.Remove(agentes);
            //Commit na BD
            db.SaveChanges();
            return RedirectToAction("Index");
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
