using System.Linq;
using System.Collections.Generic;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using MySql.Data.MySqlClient;

namespace Biblioteca.Models
{
    public class UsuarioService
    {
        public int Cadastrar(Usuario u)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                bc.Usuarios.Add(u);
                bc.SaveChanges();
                return u.Id;
            }
        }

        public void Atualizar(Usuario u)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario usuario = bc.Usuarios.Find(u.Id);
                if(usuario != null){
                usuario.Nome = u.Nome;
                usuario.Login = u.Login;
                usuario.Senha = u.Senha;

                bc.SaveChanges();}
            
            }
        }

        public ICollection<Usuario> Listar(string termo, string ordem)
        
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                IQueryable<Usuario> consulta = 
                bc.Usuarios.Where(p => p.Nome.Contains(termo, System.StringComparison.OrdinalIgnoreCase));
                if(ordem == "t")
                consulta = consulta.OrderBy(p => p.Nome);
                else
                consulta = consulta.OrderBy(p => p.Login);




                return consulta.ToList();

            }
        }

        public Usuario DetalhesUsuarios(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                Usuario registro = bc.Usuarios.Where(p => p.Id == id).SingleOrDefault();
                return registro;
            }
        }

        public void Deletar(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {   
            Usuario registro = bc.Usuarios.Find(id);
            bc.Usuarios.Remove(registro);
            bc.SaveChanges();
            }
        }

        public ICollection<Usuario> GetUsuario(int page, int size)
        {    
            using(BibliotecaContext bc = new BibliotecaContext())
          {

          int pular = (page - 1) * size;

          IQueryable<Usuario> consulta =
              bc.Usuarios.Include(u => u.Nome).OrderByDescending(u => u.Nome);
  
          return consulta.Skip(pular).Take(size).ToList();
          } 
        }

        public int Contarusuarios()
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            return bc.Usuarios.Count();
        }

        



        

        public Usuario ObterPorId(int id)
        {
            using(BibliotecaContext bc = new BibliotecaContext())
            {
                return bc.Usuarios.Find(id);
            }
        }

        private const string DadosConexao = "Database=CRUD; Data Source=localhost; User Id=root; password=Merinho1-;";


        public Usuario ValidarLogin(Usuario usuario)
        {
            MySqlConnection Conexao = new MySqlConnection(DadosConexao);
            Conexao.Open();
            string Query = "SELECT * FROM Usuario WHERE Login=@Login AND Senha=@Senha";
            MySqlCommand Comando = new MySqlCommand(Query, Conexao);
            Comando.Parameters.AddWithValue("@Login", usuario.Login);
            Comando.Parameters.AddWithValue("@Senha", usuario.Senha);
            MySqlDataReader Reader = Comando.ExecuteReader();

            Usuario UsuarioEncontrado = new Usuario();

            if(Reader.Read())
            {
                UsuarioEncontrado.Id = Reader.GetInt32("Id");
                
                if(!Reader.IsDBNull(Reader.GetOrdinal("Nome")))
                    UsuarioEncontrado.Nome = Reader.GetString("Nome");
                if(!Reader.IsDBNull(Reader.GetOrdinal("Login")))                    
                    UsuarioEncontrado.Login = Reader.GetString("Login");
                if(!Reader.IsDBNull(Reader.GetOrdinal("Senha")))                    
                    UsuarioEncontrado.Senha = Reader.GetString("Senha");
            }
            Conexao.Close();
            return UsuarioEncontrado;
        }
    }
}