﻿using AdaptiveCards;
using DAO.BBL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Model;
using Model.Procucts;
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
        protected string maisSabores { get; set; }
        protected int sabor { get; set; }
        protected List<string> tamanhos { get; set; }
        protected Dictionary<int, string> sabores { get; set; }
        //protected Dictionary<int, string> saboresTamanho { get; set; }
        protected Dictionary<int, string> bebidas { get; set; }
        protected Dictionary<int, string> saboresSelecionados { get; set; }
        protected List<PizzasSelecionadas> pizzasSelecionadas { get; set; }
        protected BebidasSelecionadas bebidasSelecionadas { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedStartConversation); // State transition: wait for user to start conversation
        }

        public async Task MessageReceivedStartConversation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            await context.PostAsync("Olá " + context.Activity.From.Name);
            this.pizzasSelecionadas = new List<PizzasSelecionadas>();
            this.bebidasSelecionadas = new BebidasSelecionadas();
            this.bebidasSelecionadas.bebidas = new Dictionary<int, string>();
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Aceita uma Pizza?"));
            await context.PostAsync(message);
            context.Wait(MessageReceivedAceitaPizza); // State transition: wait for user to provide registration number
        }

        public async Task MessageReceivedAceitaPizza(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            if (this.sabores == null)
            {
                var aceitaPizza = (await argument).Text;

                if (aceitaPizza.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA" || this.aceitaPizza.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA")
                {
                    await context.PostAsync("Selecione os sabores: ");
                    var message = context.MakeMessage();
                    message.Attachments = new List<Attachment>();
                    message.Attachments.Add(BotaoCardapio());
                    
                    await context.PostAsync(message);
                    context.Wait(MessageReceivedCoverStart);
                }
                else
                {
                    if(this.pizzasSelecionadas.Count > 0)
                    {
                        var message = context.MakeMessage();
                        message.Attachments = new List<Attachment>();
                        message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja Alguma bebida? "));
                        await context.PostAsync(message);
                        context.Wait(MessageReceivedSelecionandoBebida);
                    }
                    else
                    {
                        await context.PostAsync("Tudo bem " + context.Activity.From.Name + ", agradecemos o seu contato! Quem sabe em uma próxima =)");
                        context.Done<object>(new object());
                    }
                }
            }
            else
            {
                this.maisSabores = (await argument).Text;

                if (this.maisSabores.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA")
                {
                    await context.PostAsync("Selecione os sabores: ");
                    var message = context.MakeMessage();
                    message.Attachments = new List<Attachment>();
                    message.Attachments.Add(BotaoCardapio());

                    await context.PostAsync(message);
                    context.Wait(MessageReceivedCoverStart);
                }
                else
                {
                    /*await context.PostAsync("Pizza Selecionada! Sabor: " + this.saboresSelecionados.First().Value + " !");
                    var message = context.MakeMessage();
                    message.Attachments = new List<Attachment>();
                    pizzasSelecionadas.Add(new PizzasSelecionadas(){ sabores = this.saboresSelecionados });
                    this.saboresSelecionados = null;
                    this.sabores = null;
                    message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja pedir mais pizzas ? "));
                    await context.PostAsync(message);
                    context.Wait(MessageReceivedPizzaSelecionada);*/
                    await context.PostAsync("Pizza Selecionada! Sabor: " + this.saboresSelecionados.First().Value + " !");
                    var message = context.MakeMessage();
                    message.Attachments = new List<Attachment>();
                    this.sabores = null;
                    message.Attachments.Add(BotaoTamanhosPizza());
                    await context.PostAsync(message);
                    context.Wait(MessageReceivedTamanhoPizza);
                }

            }

        }

        public async Task MessageReceivedCoverStart(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var sabor = (await argument).Text;
            this.sabor =  int.Parse(sabor);
            this.saboresSelecionados.Add(this.sabores.Where(a => a.Key == this.sabor).Select(aa => aa.Key).SingleOrDefault(), this.sabores.Where(a => a.Key == this.sabor).Select(aa => aa.Value).SingleOrDefault());
            this.sabores.Remove(this.sabor);
            if(this.saboresSelecionados.Count >= 2)
            {
                await context.PostAsync("Pizza Selecionada! Sabores: " + this.saboresSelecionados.First().Value + " e " + this.saboresSelecionados.Last().Value + " !");
                var message = context.MakeMessage();
                message.Attachments = new List<Attachment>();
                //pizzasSelecionadas.Add(new PizzasSelecionadas() { sabores = this.saboresSelecionados });
                //this.saboresSelecionados = null;
                this.sabores = null;
                message.Attachments.Add(BotaoTamanhosPizza());
                //message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja pedir mais pizzas ? "));
                await context.PostAsync(message);
                context.Wait(MessageReceivedTamanhoPizza);
            }
            else
            {
                var message = context.MakeMessage();
                message.Attachments = new List<Attachment>();
                message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja mais algum sabor ? "));
                await context.PostAsync(message);
                context.Wait(MessageReceivedAceitaPizza);
            }
            
            //context.Done<object>(new object()); // Signal completion
        }

        public async Task MessageReceivedPizzaSelecionada(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {   
            this.aceitaPizza = (await argument).Text;
            //this.saboresSelecionados = null;
            //this.sabores = null;
            if (aceitaPizza.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA" || this.aceitaPizza.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA")
            {
                await context.PostAsync("Selecione os sabores: ");
                var message = context.MakeMessage();
                message.Attachments = new List<Attachment>();
                message.Attachments.Add(BotaoCardapio());

                await context.PostAsync(message);
                context.Wait(MessageReceivedCoverStart);
            }
            else
            {
                var message = context.MakeMessage();
                message.Attachments = new List<Attachment>();
                message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja Alguma bebida? "));
                await context.PostAsync(message);
                context.Wait(MessageReceivedSelecionandoBebida);
                //context.Done<object>(new object()); // Signal completion
            }

        }

        public async Task MessageReceivedSelecionandoBebida(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var aceitaBebida = (await argument).Text;

            if (aceitaBebida.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA")
            {
                await context.PostAsync("Selecione a bebida: ");
                var message = context.MakeMessage();
                message.Attachments = new List<Attachment>();
                message.Attachments.Add(BotaoCardapioBebida());

                await context.PostAsync(message);
                context.Wait(MessageReceivedBebidaSelecionada);
            }
            else
            {
                await context.PostAsync("Valor do pedido e qual a forma de pagamento");
                context.Done<object>(new object()); // Signal completion
            }

        }

        //public async Task MessageReceivedSelecionarTamBebida(IDialogContext context, IAwaitable<IMessageActivity> argument)
        //{
        //    var tamanho = (await argument).Text;

        //    await context.PostAsync("Bebida Selecionada.");
        //    var message = context.MakeMessage();
        //    message.Attachments = new List<Attachment>();
        //    message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja mais Alguma bebida? "));
        //    await context.PostAsync(message);
        //    context.Wait(MessageReceivedSelecionandoBebida);

        //}

        public async Task MessageReceivedBebidaSelecionada(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var bebida = (await argument).Text;
            this.bebidasSelecionadas.bebidas.Add(this.bebidas.Where(a => a.Key == int.Parse(bebida)).Select(aa => aa.Key).SingleOrDefault(), this.bebidas.Where(a => a.Key == int.Parse(bebida)).Select(aa => aa.Value).SingleOrDefault());

            await context.PostAsync("Bebida Selecionada.");
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja mais Alguma bebida? "));
            await context.PostAsync(message);
            context.Wait(MessageReceivedSelecionandoBebida);

        }

        public async Task MessageReceivedTamanhoPizza(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var tamanho = (await argument).Text;
            pizzasSelecionadas.Add(new PizzasSelecionadas() { sabores = this.saboresSelecionados, tamanho = tamanho });
            this.saboresSelecionados = null;
            await context.PostAsync("Tamanho Selecionado.");
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja mais Alguma Pizza? "));
            await context.PostAsync(message);
            context.Wait(MessageReceivedAceitaPizza);

        }

        private Attachment BotaoPergunta(string UserId, string subtitle)
        {

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = UserId + "simPizza",
                Type = "postBack",
                Title = "Sim"
            };

            CardAction plButton2 = new CardAction()
            {
                Value = UserId + "naoPizza",
                Type = "postBack",
                Title = "Não"
            };
            cardButtons.Add(plButton);
            cardButtons.Add(plButton2);

            HeroCard plCard = new HeroCard()
            {
                Title = "Fast Pizza",
                Subtitle = subtitle,
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();

            return plAttachment;
        }

        private Attachment BotaoCardapio()//Falta Implementar
        {

            List<Product> produtos = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == 1).SingleOrDefault();
                produtos = pizzaria.menus.Where(p => p.productType.ToString().ToUpper() == "PIZZA").ToList<Product>();
            }

            List<CardAction> cardButtons = new List<CardAction>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://1.bp.blogspot.com/-6ziUbuUGS1I/VuHuua1N2hI/AAAAAAAAAVI/eU17sMzkpLI-npKJPwypY_dU4gGf7Epaw/s1600/CARDAPIO.png"));

            if(sabores == null)
            {
                this.sabores = new Dictionary<int, string>();
                this.saboresSelecionados = new Dictionary<int, string>();

                foreach (var produto in produtos)
                {
                    this.sabores.Add(produto.Id, produto.name);
                    CardAction plButton = new CardAction()
                    {
                        Value = produto.Id.ToString(),
                        Type = "postBack",
                        Title = produto.name
                    };

                    cardButtons.Add(plButton);
                }
            }
            else
            {
                foreach (var produto in sabores)
                {
                    CardAction plButton = new CardAction()
                    {
                        Value = produto.Key.ToString(),
                        Type = "postBack",
                        Title = produto.Value
                    };

                    cardButtons.Add(plButton);
                }
            }
            

            var plCard = new ThumbnailCard()
            {
                Title = "Cardápio",
                Subtitle = "Escolha o sabor",
                Buttons = cardButtons,
                Images = cardImages
            };

            Attachment plAttachment = plCard.ToAttachment();

            //AdaptiveCard card = new AdaptiveCard();

            //card.Body.Add(new TextBlock() { Text = "Escolha a sua pizza" });
            //card.BackgroundImage = "https://1.bp.blogspot.com/-6ziUbuUGS1I/VuHuua1N2hI/AAAAAAAAAVI/eU17sMzkpLI-npKJPwypY_dU4gGf7Epaw/s1600/CARDAPIO.png";

            //var choices = new List<Choice>();

            //foreach (var produto in produtos)
            //{
            //    choices.Add(new Choice() { Title = produto.name , Value = produto.name });
            //}

            //card.Body.Add(new ChoiceSet() {
            //    Id = "snooze",
            //    Style = ChoiceInputStyle.Compact,
            //    Choices = choices
            //});

            //Attachment plAttachment = new Attachment()
            //{
            //    ContentType = AdaptiveCard.ContentType,
            //    Content = card
            //};

            return plAttachment;
        }

        private Attachment BotaoCardapioBebida()//Falta Implementar
        {

            List<Product> produtos = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == 1).SingleOrDefault();
                produtos = pizzaria.menus.Where(p => p.productType.ToString().ToUpper() == "BEVERAGE").ToList<Product>();
            }

            List<CardAction> cardButtons = new List<CardAction>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: "https://www.shipit.com/wp-content/uploads/2015/10/Beer-Wine-Spirits-and-Other-Beverages.jpg"));

            if (this.bebidas == null)
            {
                this.bebidas = new Dictionary<int, string>();
                foreach (var produto in produtos)
                {
                    this.bebidas.Add(produto.Id, produto.name);
                    CardAction plButton = new CardAction()
                    {
                        Value = produto.Id.ToString(),
                        Type = "postBack",
                        Title = produto.name
                    };

                    cardButtons.Add(plButton);
                }
            }
            else
            {
                foreach (var produto in bebidas)
                {
                    CardAction plButton = new CardAction()
                    {
                        Value = produto.Key.ToString(),
                        Type = "postBack",
                        Title = produto.Value
                    };

                    cardButtons.Add(plButton);
                }
            }


            var plCard = new ThumbnailCard()
            {
                Title = "Cardápio",
                Subtitle = "Escolha a bebida",
                Buttons = cardButtons,
                Images = cardImages
            };

            Attachment plAttachment = plCard.ToAttachment();

            return plAttachment;
        }

        private Attachment BotaoTamanhosPizza()//Falta Implementar
        {
            List<CardAction> cardButtons = new List<CardAction>();
            List<Product> produtos = new List<Product>();
            //ProductInstance productInstance = new ProductInstance();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == 1).SingleOrDefault();
                produtos = pizzaria.menus.Where(p => p.productType.ToString().ToUpper() == "PIZZA").ToList<Product>();

                
                //List<CardImage> cardImages = new List<CardImage>();
                //cardImages.Add(new CardImage(url: "https://www.shipit.com/wp-content/uploads/2015/10/Beer-Wine-Spirits-and-Other-Beverages.jpg"));

                foreach (var productInstance in produtos[0].productInstance.Take(3))
                {
                    CardAction plButton = new CardAction()
                    {
                        Value = productInstance.Id.ToString(),
                        Type = "postBack",
                        Title = productInstance.productSize.name
                    };

                    cardButtons.Add(plButton);
                }
    
            }

            var plCard = new ThumbnailCard()
            {
                Title = "Opções",
                Subtitle = "Escolha o tamanho",
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();
            

            return plAttachment;
        }
    }
}