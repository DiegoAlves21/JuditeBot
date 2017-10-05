using DAO.BBL;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Model;
using Model.Procucts;
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
        public virtual async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                // Transition into the CarInsuranceDialog
                await Conversation.SendAsync(activity, () => new OrderDialog());
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
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
    }
}