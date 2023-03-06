using System.Collections.Generic;
using Biblioteca.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;




namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

         public IActionResult Login()
        {
            return View();
        }

        

        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario u)
        {                       
            UsuarioService service = new UsuarioService();
            int novoId = service.Cadastrar(u);
            if(novoId !=0)
            {

                ViewData["Mensagem"] = "Cadastro realizado com sucesso";

            }
            else
            {
                ViewData["Mensagem"] = "Falha no cadastro";
            
            }
            
            return View();

        }

        public IActionResult Lista(string q, string ordem)
        {
            UsuarioService service = new UsuarioService();
            if(q == null)
                q = string.Empty;
          
            if(ordem == null)
                ordem = "t";

            return View(service.Listar(q, ordem));

        }

        
        
        public IActionResult Atualiza(int id)
        {
            UsuarioService service = new UsuarioService();
            Usuario u = service.DetalhesUsuarios(id);

            return View("Editar", u);
        }



        public IActionResult deleta(int id)
        {
            UsuarioService service = new UsuarioService();
            Usuario u = service.DetalhesUsuarios(id);

            return View(u);
        }

  [HttpPost]
        public IActionResult Deleta(int id, string decisao)
        {
            if(decisao == "s")
            {
                UsuarioService service = new UsuarioService();
                service.Deletar(id);
            }

            return RedirectToAction("Lista");
        }
        public IActionResult Index(int p = 1)
        {
          int quantidadePorPagina = 2;
            
            UsuarioService service = new UsuarioService();
            ICollection<Usuario> lista = service.GetUsuario(p, quantidadePorPagina);
            
            int quantidadeRegistros = service.Contarusuarios();
            ViewData["Paginas"] = (int)Math.Ceiling((double)quantidadeRegistros / quantidadePorPagina);
            
            return View("Lista", lista);
        }

       

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            UsuarioService usuarioService = new UsuarioService();
            Usuario usuarioSessao = usuarioService.ValidarLogin(usuario);

            if(usuario != null) 
            {
                ViewBag.Mensagem="Você está logado!";
                HttpContext.Session.SetInt32("IdUsuario", usuarioSessao.Id);
                HttpContext.Session.SetString("NomeUsuario", usuarioSessao.Nome);

                return Redirect("Cadastro");
            } else {
                ViewBag.Mensagem = "Falha no login!";
                return View();
            }
        }
  

        


    }
}


        