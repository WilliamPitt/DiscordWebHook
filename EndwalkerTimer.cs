using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Discord.Net.WebSockets;
using Discord;
using Discord.Webhook;
using System.Collections.Generic;

namespace Learning
{
    public static class EndwalkerTimer
    {
        public static ulong webhookId;
        public static string webhookToken;

        [FunctionName("EndwalkerTimer")]
        public static Task Run([TimerTrigger("0 30 17 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            DateTime releaseDate = new DateTime(2021, 12, 3, 12, 0, 0);

            var timeTillRelease = releaseDate.Subtract(DateTime.Now);
            log.LogDebug($"Calculated time till endwalker as {timeTillRelease}");

            using (var client = new DiscordWebhookClient(webhookId, webhookToken))
            {
                // Webhooks are able to send multiple embeds per message
                // As such, your embeds must be passed as a collection.
                try
                {
                    log.LogDebug("About to created embeded message");
                    var embeded = new EmbedBuilder{
                        ImageUrl = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/d97a55fd-6470-4e60-bd9b-7b28d10610c7/dd6vkcj-ce9ff16c-73b3-4a7c-90c8-6801216fb02b.png/v1/fill/w_1024,h_759,strp/ffxiv__tataru_by_winterleigh_dd6vkcj-fullview.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7ImhlaWdodCI6Ijw9NzU5IiwicGF0aCI6IlwvZlwvZDk3YTU1ZmQtNjQ3MC00ZTYwLWJkOWItN2IyOGQxMDYxMGM3XC9kZDZ2a2NqLWNlOWZmMTZjLTczYjMtNGE3Yy05MGM4LTY4MDEyMTZmYjAyYi5wbmciLCJ3aWR0aCI6Ijw9MTAyNCJ9XV0sImF1ZCI6WyJ1cm46c2VydmljZTppbWFnZS5vcGVyYXRpb25zIl19.vT5QwfzMpsvWeX6oekdPJ75i8u8n2e4VvtBi3Ltlf3w",
                        Title = "Suni says",
                        Description = $"There are {timeTillRelease.Days} Days till endwalker releases"                    
                    };
                    
                    log.LogDebug("About to send message to Discord API");
                    client.SendMessageAsync(embeds: new []{ embeded.Build()});
                }
                catch (Exception e)
                {
                    log.LogError(e, "Failed to call the webhook");
                }
            }

            return Task.CompletedTask;
        }
    }
}
