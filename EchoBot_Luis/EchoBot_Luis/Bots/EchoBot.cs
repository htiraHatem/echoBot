// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.6.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using System;
using EchoBot_Luis.Serialization;
using EchoBot_Luis.Services;

namespace EchoBot_Luis.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var connector = new ConnectorClient(new Uri(turnContext.Activity.ServiceUrl));

            var resp = await Response((Activity)turnContext.Activity);
            var msg = (turnContext as ITurnContext).Activity.CreateReply(resp, "en");

            await connector.Conversations.ReplyToActivityAsync(msg);
        }


        private static async Task<string> Response(Activity message)
        {
            Activity output = new Activity();
            var response = await Luis.GetResponse(message.Text);
            if (response != null)
            {
                var intent = new Intent();
                var entity = new Serialization.Entity();
                String msg = "Intent : " + System.Environment.NewLine;

                foreach (var item in response.intents)
                {
                    msg +=  item.intent + " , score = " + item.score + System.Environment.NewLine; 
                }

                output = message.CreateReply(msg);
            }
            return output.Text;
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
