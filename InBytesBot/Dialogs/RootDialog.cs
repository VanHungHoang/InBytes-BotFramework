using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using FootballData;

namespace InBytesBot.Dialog
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            //// calculate something for us to return
            //int length = (activity.Text ?? string.Empty).Length;

            //// return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            //CodeNew
            Championships champs = new Championships();
            if (activity.Text.Contains("how many teams"))
            {
                await context.PostAsync($"There are {champs.GetTeamCount()} teams.");
            }
            else if (activity.Text.StartsWith("who") || activity.Text.StartsWith("which team"))
            {
                if (activity.Text.Contains("best") || activity.Text.Contains("hightest") || activity.Text.Contains("greatest"))
                {
                    await context.PostAsync($"The hightest ranked team is {champs.GetHighestRatedTeam()}");
                }
                else if (activity.Text.Contains("worst") || activity.Text.Contains("lowest"))
                {
                    await context.PostAsync($"The lowest ranked team is {champs.GetLowestRatedTeam()}");
                }
            }
            else
            {
                await context.PostAsync($"Sorry but my response are limited. Please ask the right questions");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}