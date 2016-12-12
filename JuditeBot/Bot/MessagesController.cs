using Microsoft.Bot.Connector;
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
        /// <summary>
        /// POST: JuditeBot/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                 ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                if (activity.Text.ToString().ToUpper() == "SIMPIZZA")
                {
                    Activity replyToConversation = activity.CreateReply("Ótimo, vamos te enviar nosso Cardápio!");
                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
                }
                else if(activity.Text.ToString().ToUpper() == "NAOPIZZA")
                {
                    Activity replyToConversation = activity.CreateReply("Tudo bem, quem sabe na próxima!");
                    var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
                }
                else
                {
                    var reply = await connector.Conversations.SendToConversationAsync(BotaoInicial(activity));
                }
                
                //Activity reply = new Activity();

                //reply = activity.CreateReply("Koé " + activity.From.Name + ", veio pedir uma pizza ou só bater papo?");
                //reply.Attachments = new List<Attachment>();

                //var teste = new { text = "Retornando um botão", buttons = new [] { new { type = "imBack", title = "BLT", value = "1" } } };


                //reply.Attachments.Add(new Attachment { Content = teste });
                //reply.AttachmentLayout = "List";
                //reply.Type = "message";

                //await connector.Conversations.ReplyToActivityAsync(reply);
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
            Activity replyToConversation = activity.CreateReply("Olá " + activity.From.Name);
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            List<CardImage> cardImages = new List<CardImage>();
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

    }
}
