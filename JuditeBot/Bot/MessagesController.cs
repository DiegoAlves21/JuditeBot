﻿using DAO.BBL;
using Microsoft.Bot.Connector;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JuditeBot.Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        const string id = "8655481";


        /// <summary>
        /// POST: JuditeBot/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                //var latitude = activity.Entities[0].Properties.First.First.First.Next.Last;

                if (activity.Entities != null)
                {
                    string mensagemRetorno = "";

                    var latitude = activity.Entities[0].Properties.First.First.First.Next.Last;
                    var longitude = activity.Entities[0].Properties.First.First.First.Next.Next.First;

                    var endereco = latitude + "-" + longitude;

                    ArmazenaPedidoTemporario(Int64.Parse(activity.From.Id), 0, activity.From.Name, "endereco", ref mensagemRetorno, endereco);

                    if (mensagemRetorno != "")
                    {
                        var reply = await connector.Conversations.SendToConversationAsync(activity.CreateReply("Desculpe, não foi possível efetuar o seu pedido"));
                    }
                    else
                    {
                        var reply = await connector.Conversations.SendToConversationAsync(activity.CreateReply("Muito bom! Seu pedido foi efetuado. Fique atento porque podemos entrar em contato com você se necessário. Logo mais você receberá o seu pedido =D"));
                    }
                }
                else if (activity.Text.ToString().ToUpper() == "8655481SIMPIZZA")
                {
                    //Activity replyToConversation = activity.CreateReply("Ótimo, vamos te enviar nosso Cardápio!");
                    var reply = await connector.Conversations.SendToConversationAsync(BotaoCardapio(activity));
                }
                else if (activity.Text.ToString().ToUpper() == "8655481NAOPIZZA")
                {
                    Activity replyToConversation = activity.CreateReply("Tudo bem, quem sabe na próxima!");
                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
                }
                else if (activity.Text.ToString().ToUpper().StartsWith("8655481CARDAPIO"))
                {
                    string mensagemRetorno = "";
                    ArmazenaPedidoTemporario(Int64.Parse(activity.From.Id), Int32.Parse(activity.Text.ToString().Substring("8655481CARDAPIO".Length)), activity.From.Name, "cardapio", ref mensagemRetorno);

                    if (mensagemRetorno != "")
                    {
                        var reply = await connector.Conversations.SendToConversationAsync(activity.CreateReply("Desculpe, não foi possível efetuar o seu pedido"));
                    }
                    else
                    {
                        var reply = await connector.Conversations.SendToConversationAsync(BotaoMetodoPagamento(activity));
                    }
                    //Activity replyToConversation = activity.CreateReply("Excelente escolha!/n" + "Agora nos informe o seu endereço contendo: Cidade, bairro, rua, casa e CEP");

                }
                else if (activity.Text.ToString().ToUpper().StartsWith("8655481MEIOPAGAMENTO"))
                {
                    string mensagemRetorno = "";
                    ArmazenaPedidoTemporario(Int64.Parse(activity.From.Id), Int32.Parse(activity.Text.ToString().Substring("8655481MEIOPAGAMENTO".Length)), activity.From.Name, "meioPagamento", ref mensagemRetorno);

                    if (mensagemRetorno != "")
                    {
                        var reply = await connector.Conversations.SendToConversationAsync(activity.CreateReply("Desculpe, não foi possível efetuar o seu pedido"));
                    }
                    else
                    {
                        var reply = await connector.Conversations.SendToConversationAsync(activity.CreateReply("Favor envie sua localização pelo messenger"));
                    }
                }
                //else if (activity.Text.ToString().Length == 8)
                //{

                //}
                else
                {
                    var reply = await connector.Conversations.SendToConversationAsync(BotaoInicial(activity));
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }

        private Activity BotaoInicial(Activity activity)
        {
            Activity replyToConversation = activity.CreateReply("Olá " + activity.From.Name);
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = "8655481simPizza",
                Type = "postBack",
                Title = "Sim"
            };

            CardAction plButton2 = new CardAction()
            {
                Value = "8655481naoPizza",
                Type = "postBack",
                Title = "Não"
            };
            cardButtons.Add(plButton);
            cardButtons.Add(plButton2);

            HeroCard plCard = new HeroCard()
            {
                Title = "Fast Pizza",
                Subtitle = "Aceita uma Pizza?",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            return replyToConversation;
        }

        private Activity BotaoCardapio(Activity activity)//Falta Implementar
        {

            List<Product> produtos = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.name.ToUpper() == "FAST PIZZA").SingleOrDefault(); ;
                produtos = pizzaria.menus.ToList<Product>();
            }

            //produtos.Add(new Produto() { nome = "Mussarela", valor = 20.00, Id = 1 });
            //produtos.Add(new Produto() { nome = "Calabreza", valor = 18.00, Id = 2 });
            //produtos.Add(new Produto() { nome = "4 queijos", valor = 25.00, Id = 3 });

            Activity replyToConversation = activity.CreateReply();
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.pizzariafornella.com.br/wp-content/uploads/2015/07/CARDAPIO.png"));

            foreach (var produto in produtos)
            {
                CardAction plButton = new CardAction()
                {
                    Value = /*id + "cardapio" + produto.Id.ToString()*/ "123",
                    Type = "postBack",
                    Title = produto.name
                };

                cardButtons.Add(plButton);
            }

            HeroCard plCard = new HeroCard()
            {
                Title = "Cardápio",
                Subtitle = "Escolha a sua pizza",
                Buttons = cardButtons,
                Images = cardImages
            };

            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            return replyToConversation;
        }

        private Activity BotaoMetodoPagamento(Activity activity)//Falta Implementar
        {

            List<PaymentMethod> meiosPagamentos = new List<PaymentMethod>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.name.ToUpper() == "FAST PIZZA").SingleOrDefault(); ;
                meiosPagamentos = pizzaria.paymentMethods.ToList<PaymentMethod>();
            }
            //meiosPagamentos.Add(new MeioPagamento() { meioPagamento = "Debito", Id = 1 });
            //meiosPagamentos.Add(new MeioPagamento() { meioPagamento = "Crédito", Id = 2 });
            //meiosPagamentos.Add(new MeioPagamento() { meioPagamento = "Dinheiro", Id = 3 });

            Activity replyToConversation = activity.CreateReply();
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            List<CardAction> cardButtons = new List<CardAction>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.makemoneyinlife.com/wp-content/uploads/2015/12/Make-Money-On-Credit-Cards.jpg"));

            foreach (var meioPagamento in meiosPagamentos)
            {
                CardAction plButton = new CardAction()
                {
                    Value = id + "meioPagamento" + meioPagamento.Id.ToString(),
                    Type = "postBack",
                    Title = meioPagamento.description
                };

                cardButtons.Add(plButton);
            }

            HeroCard plCard = new HeroCard()
            {
                Title = "Meio de Pagamento",
                Subtitle = "Escolha seu método de pagamento",
                Buttons = cardButtons,
                Images = cardImages
            };

            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);

            return replyToConversation;
        }

        private void ArmazenaPedidoTemporario(long idUsuario, int idDado, string nomeUsuario, string tipoDado, ref string mensagem, string endereco = null)
        {
            PedidoTemporario pedidoTemporario = new PedidoTemporario();

            try
            {
                if (tipoDado == "cardapio")
                {

                    pedidoTemporario.idUsuarioMessenger = idUsuario;
                    pedidoTemporario.idProduto = idDado;
                    pedidoTemporario.nomeUsuarioMessenger = nomeUsuario;

                    using (var repositorio = new PedidoTemporarioRepositorio())
                    {
                        repositorio.Adicionar(pedidoTemporario);
                        repositorio.SalvarTodos();
                    }
                }
                else if (tipoDado == "meioPagamento")
                {

                    using (var repositorio = new PedidoTemporarioRepositorio())
                    {
                        var pedido = repositorio.Get(p => p.idUsuarioMessenger == idUsuario).ToList<PedidoTemporario>().SingleOrDefault();
                        pedido.idMeioPagamento = idDado;
                        repositorio.Atualizar(pedido);
                        repositorio.SalvarTodos();
                    }
                }
                else if (tipoDado == "endereco")
                {
                    using (var repositorio = new PedidoTemporarioRepositorio())
                    {
                        var pedido = repositorio.Get(p => p.idUsuarioMessenger == idUsuario).ToList<PedidoTemporario>().SingleOrDefault();
                        pedido.endereco = endereco;
                        repositorio.Atualizar(pedido);
                        repositorio.SalvarTodos();

                        pedidoTemporario = new PedidoTemporario();
                        pedidoTemporario = repositorio.Get(p => p.idUsuarioMessenger == idUsuario).ToArray<PedidoTemporario>().SingleOrDefault();

                    }

                    //using (var repositorio = new PedidoRepositorio())
                    //{
                    //    var pedido = new Order();
                    //    pedido.meioPagamentoId = pedidoTemporario.idMeioPagamento;
                    //    pedido.produtos = new List<Product>();
                    //    pedido.produtos.Add(new Produto { Id = pedidoTemporario.idProduto });
                    //    pedido.status = new OrderStatus() { description = "Pedido Efetuado" };
                    //    pedido.Address = pedidoTemporario.endereco;
                    //    pedido.clientName = pedidoTemporario.nomeUsuarioMessenger;

                    //    repositorio.Adicionar(pedido);
                    //}
                }

                mensagem = "";
            }
            catch (Exception e)
            {
                mensagem = e.Message;
            }



        }

    }
}