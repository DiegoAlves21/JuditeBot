using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JuditeBot.Bot
{
    [Serializable]
    public class OrderDialog : IDialog<object>
    {
        protected string aceitaPizza { get; set; }
        protected string startDate { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedStartConversation); // State transition: wait for user to start conversation
        }

        public async Task MessageReceivedStartConversation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync("Olá " + context.Activity.From.Name);

            var message = context.MakeMessage();

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = context.Activity.From.Id + "simPizza",
                Type = "postBack",
                Title = "Sim"
            };

            CardAction plButton2 = new CardAction()
            {
                Value = context.Activity.From.Id + "naoPizza",
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

            message.Attachments = new List<Attachment>();
            message.Attachments.Add(plAttachment);

            await context.PostAsync(message);
            context.Wait(MessageReceivedAceitaPizza); // State transition: wait for user to provide registration number
        }

        public async Task MessageReceivedAceitaPizza(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.aceitaPizza = (await argument).Text;
            await context.PostAsync("When do you want cover to start?");
            context.Wait(MessageReceivedCoverStart); // State transition: wait for user to provide cover start date
        }

        public async Task MessageReceivedCoverStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.startDate = (await argument).Text;
            // do your search/aggregation here
            await context.PostAsync($"OK, I found these deals for {aceitaPizza} starting {startDate}...");
            context.Done<object>(new object()); // Signal completion
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
    }
}