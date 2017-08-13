using DAO.BBL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using System.Web.Http;
using Microsoft.Owin.Security;
using System.Web;
using System.Net.Http;
using Model.Procucts;
using Model.Enum;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot")]
    public class ProdutoController : ApiController
    {
        // Retorna Nosso Authentication Manager
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }


        [Route("products/{name}/{size}/{type}/{available}/{p}/{per_page}/{s}/{s_dir}")]
        [HttpGet]
        public IHttpActionResult Listar(string name, string size, string type, bool? available, int p, int per_page, string s, string s_dir)
        {
            /*name = null;
            size = null;
            type = null;
            available = null;
            p = 0;
            per_page = 0;
            s = null;
            s_dir = null;*/

            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();
                    var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
                    var pizzaria = pizzariaRepositorio.Get(pi => pi.PizzariaId == pizzariaId).SingleOrDefault();

                    var produtos = FilterDeviceList(pizzaria.menus, name, size, type, available, p, per_page, s, s_dir);

                    return Ok(produtos.ToList());
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("products")]
        [HttpPost]
        public IHttpActionResult Cadastrar([FromBody] Product product)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {

                    Product pizza = new Product { name = "Muzzarela", avaible = true, productType = ProductType.PIZZA };
                    List<ProductInstance> productInstancePizza = new List<ProductInstance>();
                    ProductSize productSizeBig = new ProductSize { name = "Grande" };
                    ProductSize productSizeMedium = new ProductSize { name = "Médio" };
                    productInstancePizza.Add(new ProductInstance { cost = 30.00, productSize = productSizeBig });
                    productInstancePizza.Add(new ProductInstance { cost = 20.00, productSize = productSizeMedium });
                    pizza.productInstance = productInstancePizza;

                    product = pizza;

                    if (true == true)// Aqui entra as validações
                    {
                        PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();
                        var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
                        var pizzaria = pizzariaRepositorio.Get(pi => pi.PizzariaId == pizzariaId).SingleOrDefault();

                        pizzaria.menus.Add(product);
                        pizzariaRepositorio.AtualizarBBL(pizzaria);

                        return Created("Criado", product);
                    }
                    else
                    {
                        return new System.Web.Http.Results.ResponseMessageResult(
                            Request.CreateErrorResponse(
                                (HttpStatusCode)422,
                                new HttpError("Erros de validação ocorreram")
                            )
                        );
                    }
                    
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("products/{productId}")]
        [HttpGet]
        public IHttpActionResult Find(int productId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    ProdutoRepositorio produtoRepositorio = new ProdutoRepositorio();
                    var product = produtoRepositorio.Get(pro => pro.Id == productId);
                    return Ok(product.SingleOrDefault());
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }


        [Route("products/{productId}")]
        [HttpDelete]
        public IHttpActionResult Delete(int productId)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    ProdutoRepositorio produtoRepositorio = new ProdutoRepositorio();
                    var produto = produtoRepositorio.Get(pro => pro.Id == productId).SingleOrDefault();

                    PedidoRepositorio pedidoRepositorio = new PedidoRepositorio();
                    var pedidos = pedidoRepositorio.GetAll().ToList<Order>();
                    var contProdutos = pedidos.SelectMany(pe => pe.productInstances.Where(pro => pro.productId == productId)).Count();
                    
                    if(contProdutos == 0)
                    {
                        PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();
                        var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
                        var pizzaria = pizzariaRepositorio.Get(pi => pi.PizzariaId == pizzariaId).SingleOrDefault();

                        ProductInstanceRepositorio productInstanceRepositorio = new ProductInstanceRepositorio();
                        foreach(ProductInstance productInstance in pizzaria.menus.Where(pro => pro.Id == productId).SingleOrDefault().productInstance)
                        {
                            productInstanceRepositorio.Excluir(prIns => prIns.Id == productInstance.Id);
                            productInstanceRepositorio.SalvarTodos();
                        }

                        produtoRepositorio.Excluir(pro => pro.Id == productId);
                        produtoRepositorio.SalvarTodos();

                        /*pizzaria.menus.RemoveAt(pizzaria.menus.IndexOf(pizzaria.menus.Where(pro => pro.Id == productId).SingleOrDefault()));
                        pizzariaRepositorio.AtualizarBBL(pizzaria);*/
                    }
                    else
                    {
                        return BadRequest();
                    }

                    return new ResponseMessageResult(Request.CreateResponse((HttpStatusCode)204));
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        [Route("products-size")]
        [HttpGet]
        public IHttpActionResult GetProductsSize(string productType)
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    ProdutoRepositorio produtoRepositorio = new ProdutoRepositorio();
                    var product = produtoRepositorio.GetAll();
                    var productsPerType = product.Where(pro => pro.productType.ToString() == productType);
                    var productsSizes = productsPerType.SelectMany(pr => pr.productInstance.Where(p => p.productSize.name != ""));

                    List<object> retorno = new List<object>();

                    foreach (ProductInstance prIns in productsSizes)
                    {
                        var re = (object) new { name = prIns.productSize.name };
                        retorno.Add(re);
                    }

                    return Ok(retorno.Distinct());
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }

            }
            else
            {
                return Unauthorized();
            }

        }

        private static IQueryable<Product> FilterDeviceList(IList<Product> products, string name, string size, string type, bool? available, int p , int per_page, string s, string s_dir)
        {
            var query = products.AsQueryable();
            int cont = 0;

            if (name != null)
            {
                query = query.Where(pro => pro.name == name);
            }
            
            //VERIFICAR E REFAZER PARA FUNCIONAMENTO CORRETO    
            if (size != null)
            {
                query = query.Where(pro => pro.productInstance.Where(pr => pr.productSize.name == size).Count() > 0);
            }
                
            if (type != null)
            {
                query = query.Where(pro => pro.productType.ToString() == type);
            }
            
            if(available != null)
            {
                query = query.Where(pro => pro.avaible == available);
            }

            //Verificar como vai ficar o p -> número da página

            if(per_page != 0)
            {
                query = query.Take(per_page);
            }
            
            if(s != null)
            {
                if(s_dir == "ASC")
                {
                    query = query.OrderBy(pro => pro.name);
                }
                else
                {
                    query = query.OrderByDescending(pro => pro.name);
                }
                
            }


            return query;
        }

    }
}
