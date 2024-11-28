using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bibliotec.Contexts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bibliotec.Models;

namespace Bibliotec_mvc.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        //Criando obj da classe Context:
        Context context = new Context(); 

        //O metodo esta retornando a view ususario/Index.cshtml 

        public IActionResult Index()
        {
            //pegar as informacoes da session que sao nevessarias para que aparece os detalhes do meu usuario
           int id =int.Parse(HttpContext.Session.GetString("UsuarioID"));
           ViewBag.Admin =  HttpContext.Session.GetString("Admin");

           //busquei o usuario que esta logado (beatriz)
           Usuario usuarioEncontrado = context.Usuario.FirstOrDefault(usuario => usuario.
           UsuarioID ==id)!; 

           //se nao for encontrado ninguem 

           if(usuarioEncontrado == null){
            return NotFound();
           }

           //Procurar o curso que meu usuario esta cadastrado 
           Curso cursoEncontrado = context.Curso.FirstOrDefault(curso => curso.CursoID ==
           usuarioEncontrado.CursoID)!; 

        //Tabela Usuario -> FK CursoID
        //Tabela curso -> PK CursoID
        //Dev integral -> CursoID = 6 
        //Hiorhanna -> CursoID = 6  

        //Verificar se o usuario possui ou nao o curso
        if(cursoEncontrado == null){
            // o usuario nao possui curso cadastrado 
            //Preciso que vc mmande essa mensagem para a View: 
            ViewBag.Curso = "O usuario nao possui curso cadastrado";
        }else{
            //o usuario possui o curso XXX
            ViewBag.Curso = cursoEncontrado.Nome; 
        }


    ViewBag.Nome = usuarioEncontrado.Nome;
    ViewBag.Email = usuarioEncontrado.Email; 
    ViewBag.Telefone = usuarioEncontrado.Contato; 
    ViewBag.DtNasc = usuarioEncontrado.DtNascimento.ToString("dd/MM/yyy"); 




        return View(); 


        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     return View("Error!");
        // }
    }
}