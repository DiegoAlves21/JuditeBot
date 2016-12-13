using DAO.BBL;
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

                var latitude = activity.Entities[0].Properties.First.First.First.Next.Last;

                if (activity.Text.ToString().ToUpper() == "SIMPIZZA") 
                {
                    //Activity replyToConversation = activity.CreateReply("Ótimo, vamos te enviar nosso Cardápio!");
                    var reply = await connector.Conversations.SendToConversationAsync(BotaoCardapio(activity));
                }
                else if(activity.Text.ToString().ToUpper() == "NAOPIZZA")
                {
                    Activity replyToConversation = activity.CreateReply("Tudo bem, quem sabe na próxima!");
                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
                }
                else if (activity.Text.ToString().ToUpper().StartsWith("8655481CARDAPIO"))
                {
                    Activity replyToConversation = activity.CreateReply("Excelente escolha!/n" + "Agora nos informe o seu endereço contendo: Cidade, bairro, rua, casa e CEP");
                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
                }
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
                Value = "simPizza",
                Type = "postBack",
                Title = "Sim"
            };
 
            CardAction plButton2 = new CardAction()
            {
                Value = "naoPizza",
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
 
            List<Produto> produtos = new List<Produto>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.nome.ToUpper() == "FAST PIZZA").SingleOrDefault(); ;
                produtos = pizzaria.cardapio.ToList<Produto>();
            }

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
                    Value = id + "cardapio" + produto.Id.ToString(),
                    Type = "postBack",
                    Title = produto.nome
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
 
    }
}