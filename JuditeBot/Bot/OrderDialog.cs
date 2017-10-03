using DAO.BBL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Model;
using Model.Procucts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JuditeBot.Bot
{
    [Serializable]
    public class OrderDialog : IDialog<object>
    {
        protected int idPizzaria { get; set; }
        protected string aceitaPizza { get; set; }
        protected string maisSabores { get; set; }
        protected string bebida { get; set; }
        protected int sabor { get; set; }
        protected string pagamento { get; set; }
        protected string endereco { get; set; }
        protected double frete { get; set; }
        protected double totalPizza { get; set; }
        protected double totalBebida { get; set; }
        protected List<string> tamanhos { get; set; }
        protected Dictionary<int, string> sabores { get; set; }
        //protected Dictionary<int, string> saboresTamanho { get; set; }
        protected Dictionary<int, string> bebidas { get; set; }
        protected Dictionary<int, string> saboresSelecionados { get; set; }
        protected List<PizzasSelecionadas> pizzasSelecionadas { get; set; }
        protected List<BebidasSelecionadas> bebidasSelecionadas { get; set; }
        protected double totalPedido { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedStartConversation);
        }

        public async Task MessageReceivedStartConversation(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            idPizzaria = int.Parse(ConfigurationManager.AppSettings[context.Activity.Recipient.Id.ToString()]);// Recebe cada id da pagina do facebook e vincula com o id da pizza no banco
            await context.PostAsync("Olá " + context.Activity.From.Name);
            this.pizzasSelecionadas = new List<PizzasSelecionadas>();
            this.bebidasSelecionadas = new List<BebidasSelecionadas>();
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Aceita uma Pizza?"));
            await context.PostAsync(message);
            context.Wait(MessageReceivedAceitaPizza);
        }

        public async Task MessageReceivedAceitaPizza(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {

            if (this.sabores == null)
            {
                this.aceitaPizza = (await argument).Text;

                if (this.aceitaPizza.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA")
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
                this.sabores = null;
                message.Attachments.Add(BotaoTamanhosPizza());
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
            
        }

        public async Task MessageReceivedPizzaSelecionada(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {   
            this.aceitaPizza = (await argument).Text;
            if (this.aceitaPizza.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA")
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
                await context.PostAsync("Valor do seu pedido é: R$" + CalculaValorPedido() + "!");
                await context.PostAsync("Selecione o método de pagamento: ");
                var message = context.MakeMessage();
                message.Attachments = new List<Attachment>();
                message.Attachments.Add(BotaoMetodoPagamento());
                await context.PostAsync(message);
                context.Wait(MessageReceivedMetodoPag);
            }

        }

        public async Task MessageReceivedBebidaSelecionada(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.bebida = (await argument).Text;
            await context.PostAsync("Bebida Selecionada. Escolha o seu tamanho: ");
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(BotaoTamanhosBebida());
            await context.PostAsync(message);
            context.Wait(MessageReceivedSelecionarTamBebida);

        }

        public async Task MessageReceivedSelecionarTamBebida(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var tamanho = (await argument).Text;
            this.bebidasSelecionadas.Add(new BebidasSelecionadas() { key = this.bebidas.Where(a => a.Key == int.Parse(bebida)).Select(aa => aa.Key).SingleOrDefault(), value = this.bebidas.Where(a => a.Key == int.Parse(bebida)).Select(aa => aa.Value).SingleOrDefault(), tamanho = tamanho });
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

        public async Task MessageReceivedMetodoPag(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.pagamento = (await argument).Text;
            await context.PostAsync("Escreva o seu endereço para a entrega ");
            context.Wait(MessageReceivedEndereco);

        }

        public async Task MessageReceivedEndereco(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            this.endereco = (await argument).Text;
            var message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(GetReceiptCard(context.Activity.From.Name));
            await context.PostAsync(message);

            message = context.MakeMessage();
            message.Attachments = new List<Attachment>();
            message.Attachments.Add(BotaoPergunta(context.Activity.From.Id, "Deseja confirmar o pedido? ", "N"));
            await context.PostAsync(message);
            context.Wait(MessageReceivedConfOrder);
        }

        public async Task MessageReceivedConfOrder(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var confirmacao = (await argument).Text;
            if(confirmacao.Replace(context.Activity.From.Id, "").ToUpper() == "SIMPIZZA")
            {
                SaveInformations(context.Activity.From.Name.ToString(), context.Activity.From.Id.ToString(), context.Activity);
                await context.PostAsync("Muito bom! Seu pedido foi efetuado. Fique atento porque podemos entrar em contato com você se necessário. Logo mais você receberá o seu pedido!");
                context.Done<object>(new object());
            }
            else
            {
                await context.PostAsync("Tudo bem " + context.Activity.From.Name + ", agradecemos o seu contato! Quem sabe em uma próxima =)");
                context.Done<object>(new object());
            }
            

        }

        private Attachment BotaoPergunta(string UserId, string subtitle, string showImg = "S")
        {

            List<CardImage> cardImages = new List<CardImage>();
            if(showImg == "S")
            {
                cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
                cardImages.Add(new CardImage(url: "http://www.nutrivifalcao.com.br/wp-content/uploads/2016/11/pizza-site-or.jpg"));
            }
            
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

            if(showImg == "S")
            {
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
            else
            {
                HeroCard plCard = new HeroCard()
                {
                    Title = "Fast Pizza",
                    Subtitle = subtitle,
                    Buttons = cardButtons
                };
                Attachment plAttachment = plCard.ToAttachment();

                return plAttachment;
            }
            
        }

        private Attachment BotaoCardapio()//Falta Implementar
        {

            List<Product> produtos = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == idPizzaria).SingleOrDefault();
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

        private Attachment BotaoCardapioBebida()
        {

            List<Product> produtos = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == idPizzaria).SingleOrDefault();
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

        private Attachment BotaoTamanhosPizza()
        {
            List<CardAction> cardButtons = new List<CardAction>();
            List<Product> produtos = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == idPizzaria).SingleOrDefault();
                produtos = pizzaria.menus.Where(p => p.productType.ToString().ToUpper() == "PIZZA").ToList<Product>();

                foreach (var productInstance in produtos[0].productInstance.Take(3))
                {
                    CardAction plButton = new CardAction()
                    {
                        Value = productInstance.productSize.name,
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

        private Attachment BotaoTamanhosBebida()
        {
            List<CardAction> cardButtons = new List<CardAction>();
            List<Product> produtos = new List<Product>();

            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == idPizzaria).SingleOrDefault();
                produtos = pizzaria.menus.Where(p => p.productType.ToString().ToUpper() == "BEVERAGE").ToList<Product>();

                foreach (var productInstance in produtos[0].productInstance.Take(3))
                {
                    CardAction plButton = new CardAction()
                    {
                        Value = productInstance.productSize.name,
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

        private Attachment BotaoMetodoPagamento()
        {
            Pizzaria pizzaria = new Pizzaria();
            List<CardAction> cardButtons = new List<CardAction>();

            using (var repositorio = new PizzariaRepositorio())
            {
                pizzaria = repositorio.Get(p => p.PizzariaId == idPizzaria).SingleOrDefault();
                foreach (var metodoPagamento in pizzaria.paymentMethods)
                {
                    CardAction plButton = new CardAction()
                    {
                        Value = metodoPagamento.Id.ToString(),
                        Type = "postBack",
                        Title = ChangePaymentName(metodoPagamento.paymentMethod.ToString())
                    };

                    cardButtons.Add(plButton);
                }
            }

            var plCard = new ThumbnailCard()
            {
                Title = "Pagamento",
                Subtitle = "Escolha o método",
                Buttons = cardButtons
            };

            Attachment plAttachment = plCard.ToAttachment();

            return plAttachment;
        }

        private Attachment GetReceiptCard(string userName)
        {
            try
            {
                var receiptCard = new ReceiptCard
                {
                    Title = userName,
                    Facts = new List<Fact> { new Fact("Order Number", "Aguardando Confirmação"), new Fact("Payment Method", ChangePaymentName(GetPaymentMethod(int.Parse(pagamento)))) },
                    Items = new List<ReceiptItem>
            {
                new ReceiptItem("Pizza", price: totalPizza.ToString(), quantity: pizzasSelecionadas.Count.ToString(), image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")),
                new ReceiptItem("Bebida", price: totalBebida.ToString(), quantity: bebidasSelecionadas.Count.ToString(), image: new CardImage(url: "https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")),
            },
                    Tax = this.frete.ToString(),
                    Total = (this.totalPedido + this.frete).ToString() /*,
                    Buttons = new List<CardAction>
            {
                new CardAction(
                    ActionTypes.OpenUrl,
                    "More information",
                    "https://account.windowsazure.com/content/6.10.1.38-.8225.160809-1618/aux-pre/images/offer-icon-freetrial.png",
                    "https://azure.microsoft.com/en-us/pricing/")
            }*/
                };
                return receiptCard.ToAttachment();
            }
            catch(Exception e)
            {
                return null;
            }
            

            
        }

        private string CalculaValorPedido()
        {
            totalPedido = 0;
            totalPizza = 0;
            totalBebida = 0;
            double maiorSabor = 0;
            List<Product> pizzas = new List<Product>();
            List<Product> bebidas = new List<Product>();
            using (var repositorio = new PizzariaRepositorio())
            {
                var pizzaria = repositorio.Get(p => p.PizzariaId == idPizzaria).SingleOrDefault();
                pizzas = pizzaria.menus.Where(p => p.productType.ToString().ToUpper() == "PIZZA").ToList<Product>();
                this.frete = pizzaria.deliveryTax;
                foreach (PizzasSelecionadas p in pizzasSelecionadas)
                {
                    foreach(var sabor in p.sabores)
                    {
                        var produto = (Product)pizzas.Where(pr => pr.Id == sabor.Key).SingleOrDefault();
                        var nomeProduto = "";

                        foreach(var instance in produto.productInstance.Take(3))
                        {
                            nomeProduto = instance.productSize.name;

                            if(nomeProduto.ToUpper() == p.tamanho.ToUpper())
                            {
                                if (instance.cost > maiorSabor)
                                {
                                    maiorSabor = instance.cost;
                                }
                            }
                            
                        }
                    }
                    totalPedido += maiorSabor;
                    maiorSabor = 0;
                }

                totalPizza = totalPedido;

                bebidas = pizzaria.menus.Where(p => p.productType.ToString().ToUpper() == "BEVERAGE").ToList<Product>();

                foreach (BebidasSelecionadas b in bebidasSelecionadas)
                {
                    var bebida = bebidas.Where(be => be.Id == b.key).SingleOrDefault();
                    var nomeProduto = "";

                    foreach (var instance in bebida.productInstance.Take(3))
                        {
                            nomeProduto = instance.productSize.name;

                            if(nomeProduto.ToUpper() == b.tamanho.ToUpper())
                            {
                                maiorSabor = instance.cost;
                            }
                            
                        }
                    totalPedido += maiorSabor;
                    totalBebida += maiorSabor;
                }

            }
            return totalPedido.ToString();
        }

        private string ChangePaymentName(string name)
        {
            if (name.ToUpper() == "CREDIT")
            {
                return "CREDITO";
            }
            else if (name.ToUpper() == "DEBIT")
            {
                return "DEBITO";
            }
            else
            {
                return name;
            }
        }

        private string GetPaymentMethod(int id)
        {
            Pizzaria pizzaria = new Pizzaria();

            using (var repositorio = new PizzariaRepositorio())
            {
                pizzaria = repositorio.Get(p => p.PizzariaId == idPizzaria).SingleOrDefault();
                return pizzaria.paymentMethods.Where(p => p.Id == id).SingleOrDefault().paymentMethod.ToString();
            }
            
        }

        private void SaveInformations(string clientName, string clientId, IActivity activity)
        {
            try
            {
                //Armazenando o cliente da mensagem
                //ClientRepositorio clientRepositorio = new ClientRepositorio();

                Client verificaClient = new Client();

                Client client = new Client();
                client.toId = activity.From.Id;
                client.toName = activity.From.Name;
                client.fromId = activity.Recipient.Id;
                client.fromName = activity.Recipient.Name;
                client.serviceUrl = activity.ServiceUrl;
                client.channelId = activity.ChannelId;
                client.conversationId = activity.Conversation.Id;

                //clientRepositorio.AdicionarBBL(client);

                //Armazenando o pedido e atrelando o cliente da mensagem
                PedidoRepositorio pedidoRepositorio = new PedidoRepositorio();
                
                List<ProductInstance> productInstances = new List<ProductInstance>();
                List<MixedPizza> mixedPizzas = new List<MixedPizza>();
                Order order = new Order();

                order.Address = this.endereco;
                order.clientName = clientName;
                order.created = DateTime.Now;

                order.paymentMethodId = int.Parse(this.pagamento);

                foreach (var pSelec in this.pizzasSelecionadas)
                {
                    if (pSelec.sabores.Count() > 1)
                    {
                        MixedPizza m = new MixedPizza();
                        m.productInstances = new List<ProductInstance>();

                        foreach (var sabor in pSelec.sabores)
                        {
                            using (var repositorio = new ProdutoRepositorio())
                            {
                                var produto = repositorio.Get(p => p.Id == sabor.Key).SingleOrDefault();
                                var pInstance = produto.productInstance.Take(3).Where(p => p.productSize.name.ToUpper() == pSelec.tamanho.ToUpper()).SingleOrDefault();
                                m.productInstances.Add(pInstance);
                                repositorio.Dispose();
                            }
                        }
                        mixedPizzas.Add(m);
                    }
                    else
                    {
                        foreach (var sabor in pSelec.sabores)
                        {
                            using (var repositorio = new ProdutoRepositorio())
                            {
                                var produto = repositorio.Get(p => p.Id == sabor.Key).SingleOrDefault();
                                var pInstance = produto.productInstance.Take(3).Where(p => p.productSize.name.ToUpper() == pSelec.tamanho.ToUpper()).SingleOrDefault();
                                productInstances.Add(pInstance);
                                repositorio.Dispose();
                            }
                        }
                    }
                }

                foreach (var b in bebidasSelecionadas)
                {
                    using (var repositorio = new ProdutoRepositorio())
                    {
                        var produto = repositorio.Get(p => p.Id == b.key).SingleOrDefault();
                        var pInstance = produto.productInstance.Take(3).Where(p => p.productSize.name.ToUpper() == b.tamanho.ToUpper()).SingleOrDefault();
                        productInstances.Add(pInstance);
                        repositorio.Dispose();
                    }
                }

                if(productInstances.Count > 0)
                {
                    order.productInstances = productInstances;
                }

                if(mixedPizzas.Count > 0)
                {
                    order.mixedPizzas = mixedPizzas;
                }

                order.ordersStatus = OrderStatus.WAITING;
                order.pizzariaId = idPizzaria;
                order.clientInformations = clientId;

                using (var repositorio = new ClientRepositorio())
                {
                    verificaClient = repositorio.Get(c => c.conversationId == activity.Conversation.Id && c.fromId == activity.Recipient.Id && c.toId == activity.From.Id).SingleOrDefault();
                }

                if(verificaClient != null)
                {
                    order.clientId = verificaClient.Id;
                }
                else
                {
                    order.client = client;
                }

                pedidoRepositorio.AdicionarBBL(order);
            }
            catch(Exception e)
            {
                throw new NotImplementedException();
            }
            
        }
    }
}