using DAO.BBL;
using Microsoft.Bot.Connector;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JuditeBot.Bot
{
    public static class BotConversation
    {
        public static Activity BotaoInicial(Activity activity)
        {
            Activity replyToConversation = activity.CreateReply("Olá " + activity.From.Name + " Deseja pedir uma pizza");
            replyToConversation.Recipient = activity.From;
            replyToConversation.Type = "message";
            replyToConversation.Attachments = new List<Attachment>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = activity.From.Id + "simPizza",
                Type = "postBack",
                Title = "Sim"
            };

            CardAction plButton2 = new CardAction()
            {
                Value = activity.From.Id + "naoPizza",
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

        public static Activity BotaoCardapio(Activity activity)
        {

            List<Product> produtos = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == 1).SingleOrDefault();
                produtos = pizzaria.menus.Where(pi => pi.productType.ToString().ToUpper() == "PIZZA").ToList<Product>();
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
    }
}