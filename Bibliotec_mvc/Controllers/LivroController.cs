using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bibliotec.Contexts;
using Bibliotec.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;

namespace Bibliotec_mvc.Controllers
{
    [Route("[controller]")]
    public class LivroController : Controller
    {
        private readonly ILogger<LivroController> _logger;

        public LivroController(ILogger<LivroController> logger)
        {
            _logger = logger;
        }

        Context context = new Context();

        public IActionResult Index()
        {
            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;

            //Criar uma lista de livros 
            List<Livro> listaLivros = context.Livro.ToList();

            //Verificar se o livro tem reserva ou nao 
            var livrosReservados = context.LivroReserva.ToDictionary(livro => livro.LivroID, livror => livror.DtReserva);

            ViewBag.Livros = listaLivros;
            ViewBag.LivrosComReserva = livrosReservados;

            return View();
        }
        [Route("Cadastro")]
        //metodo que retorna a tela de cadastro;

        public IActionResult Cadastro()
        {
            ViewBag.Admin = HttpContext.Session.GetString("Admin")!;





            ViewBag.Categorias = context.Categoria.ToList();
            //retorna a view de cadastro:
            return View();
        }

        //metodo para cadastrar um livro:
        [Route("Cadastrar")]

        public IActionResult Cadastrar(IFormCollection form)
        {

            Livro novoLivro = new Livro();

            // oque meu usuario escrever no formulario sera atribuidoao novoLivro 

            novoLivro.Nome = form["Nome"].ToString();
            novoLivro.Descricao = form["Descricao"].ToString();
            novoLivro.Editora = form["Editora"].ToString();
            novoLivro.Escritor = form["Escritor"].ToString();
            novoLivro.Idioma = form["Idioma"].ToString();

            if (form.Files.Count > 0)
            {
                var arquivo = form.Files[0];


                var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwrot/imagens/livros");

                if (Directory.Exists(pasta))
                {
                    //Criar a pasta: 
                    Directory.CreateDirectory(pasta);
                }
                //
                //
                var caminho = Path.Combine(pasta, arquivo.FileName);

                using (var stream = new FileStream(caminho, FileMode.Create))
                {
                    //
                    arquivo.CopyTo(stream);
                }

                novoLivro.Imagem = arquivo.FileName;
            }
            else
            {
                novoLivro.Imagem = "padrao.png";
            }
        
        //img 
        context.Livro.Add(novoLivro);
                context.SaveChanges(); 


                List<LivroCategoria> ListalivroCategorias = new List<LivroCategoria>();

        string[] categoriasSelecionadas = form["Categoria"].ToString().Split(",");

                foreach(string categoria in categoriasSelecionadas){  
                    LivroCategoria livrocategoria = new LivroCategoria();
        livrocategoria.CategoriaID = int.Parse(categoria);
        livrocategoria.LivroID = novoLivro.LivroID;    

                    ListalivroCategorias.Add(livrocategoria); 
               }

    //peguei a colecaonmda ListaLivroCategorias e coloquei na tabela LivroCategoria
    context.LivroCategoria.AddRange(ListalivroCategorias); 

                context.SaveChanges();

                return LocalRedirect("/Livro/Cadastro");


            }
    }
}