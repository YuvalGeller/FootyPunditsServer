using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FootyPunditsBL.Models;
using FootyPundits.DTO;
using System;


namespace FootyPundits.Hubs
{
    public class ChatHub : Hub
    {
        #region Add connection to the db context using dependency injection
        FootyPunditsDBContext context;
        public ChatHub(FootyPunditsDBContext context) : base()
        {
            this.context = context;
        }
        #endregion

        public async Task SendMessageToGroup(AccMessageDTO message, string groupId)
        {
            AccMessage msg = new AccMessage()
            {
                AccountId = message.AccountId,
                ChatGameId = message.GameId,
                Content = message.Content,
                SentDate = DateTime.Now
            };
            AccMessage returnedMsg = context.AddMsg(msg);
            if (returnedMsg != null)
            {
                IClientProxy proxy = Clients.Group(groupId);
                await proxy.SendAsync("ReceiveMessageFromGroup", message.AccountId, message.Content, groupId, returnedMsg.MessageId);
            }
        }

        public async Task OnConnect(string[] gameIds)
        {
            foreach (string gameId in gameIds)
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await base.OnConnectedAsync();
        }
    }
}
