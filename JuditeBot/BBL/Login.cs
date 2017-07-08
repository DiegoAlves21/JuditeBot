using DAO.BBL;
using DAO.MapperManual;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.BBL
{
    public class Login
    {
        public dynamic Valida(string username, string password)
        {

            Users usuario;
            try
            {
                using (var repositorio = new UsuarioRepositorio())
                {
                    if (username != null && password != null)
                    {
                        usuario = repositorio.Get(u => (u.password.ToLower() == password.ToLower() && u.username.ToLower() == username.ToLower())).ToList().SingleOrDefault();
                    }
                    else
                    {
                        return new { isValid = false, mensagem = "Usuário ou Senha incorretos", pizzariaId = "" };
                    }
                }
                using (var repositorio = new PizzariaRepositorio())
                {
                    if (usuario != null)
                    {
                        var p = repositorio.Get(pi => pi.users.Where(u => u.Id == usuario.Id).SingleOrDefault().Id == usuario.Id).ToList().SingleOrDefault();
                        return new { isValid = true, mensagem = "Operação realizada com sucesso", pizzariaId = p.PizzariaId.ToString() };
                    }
                    else
                    {
                        return new { isValid = false, mensagem = "Erro ao buscar o usuário vinculado a pizzaria", pizzariaId = "" };
                    }
                }
            }
            catch (Exception e)
            {
                return new { isValid = false, mensagem = e.Message, pizzariaId = "" };
            }

        }
    }
}