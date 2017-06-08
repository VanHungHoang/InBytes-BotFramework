using FootballData;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InBytesBot
{
    [LuisModel("99c441f5-249c-4043-9688-74a096a8d586", "3c89388c6cb74dffada2fa81523e5d5c")]
    [Serializable]
    public class SimpleLUISDiaglog : LuisDialog<object>
    {
        
        public static Championships champs = new Championships();
        [LuisIntent("TeamCount")]
        public async Task GetTeamCount(IDialogContext context, LuisResult result)
        {
            //Championships champs = new Championships();
            await context.PostAsync($"There are {champs.GetTeamCount()} teams.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            //Championships champs = new Championships();
            await context.PostAsync("I have no idea what you are talking about.");
            context.Wait(MessageReceived);
        }
        [LuisIntent("TopTeam")]
        public async Task BestTeam(IDialogContext context, LuisResult result)
        {
            //Championships champs = new Championships();
            await context.PostAsync($"{champs.GetHighestRatedTeam()} is the best in the championships.");
            context.Wait(MessageReceived);
        }
        [LuisIntent("BottomTeam")]
        public async Task BottomTeam(IDialogContext context, LuisResult result)
        {
            //Championships champs = new Championships();
            await context.PostAsync($"{champs.GetLowestRatedTeam()} is the worst team in the championships.");
            context.Wait(MessageReceived);
        }
        [LuisIntent("RemoveGoodTeam")]
        public async Task RemoveGoodTeam(IDialogContext context, LuisResult result)
        {
            //Championships champs = new Championships();
            List<string> goodTeams = champs.GetTopThreeTeams();
            PromptOptions<string> options = new PromptOptions<string>("Select which of the top three teams to remove.",
                "Sorry please try again", "I give up on you", goodTeams, 2);
            PromptDialog.Choice<string>(context, RemoveGoodTeamAsync, options);
            //await context.PostAsync($"{champs.GetLowestRatedTeam()} is the worst team in the championships.");
            //context.Wait(MessageReceived);
        }

        private async Task RemoveGoodTeamAsync(IDialogContext context, IAwaitable<string> result)
        {
            //Championships champs = new Championships();
            string team = await result;
            if (champs.DoesTeamExist(team))
            {
                champs.RemoveTeam(team);
                await context.PostAsync($"{team} has been removed.");
            }
            else
            {
                await context.PostAsync($"The team {team} was not found");
            }
            context.Wait(MessageReceived);
        }

        string teamName = "";
        [LuisIntent("RemoveTeam")]
        public async Task RemoveTeam(IDialogContext context, LuisResult result)
        {
            
            //string teamName = "";
            EntityRecommendation rec;
            if (result.TryFindEntity("TeamName", out rec))
            {
                //Championships champs = new Championships();
                teamName = rec.Entity;
                if (champs.DoesTeamExist(teamName))
                {
                    //champs.RemoveTeam(teamName);
                    //await context.PostAsync($"We have removed {teamName}");
                    //context.Wait(MessageReceived);
                    PromptDialog.Confirm(context, RemoveTeamAsync, $"Are you sure want to delete {teamName}?");
                }
                else
                {
                    await context.PostAsync($"The team {teamName} was not found.");
                    context.Wait(MessageReceived);
                }
            }
            else
            {
                await context.PostAsync("Sorry no team name was found");
                context.Wait(MessageReceived);
            }
        }
        private async Task RemoveTeamAsync(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                //Championships champs = new Championships();
                champs.RemoveTeam(teamName);
                await context.PostAsync($"{teamName} has been romoved");
            }
            else
            {
                await context.PostAsync($"OK, we wont remove them.");
            }
            context.Wait(MessageReceived);
        }
    }
}