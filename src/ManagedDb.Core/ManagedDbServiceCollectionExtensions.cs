using ManagedDb.Core.Features.GetLatestChanges;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedDb.Core
{
    public static class ManagedDbServiceCollectionExtensions
    {
        public static IServiceCollection AddManagedDb(
            this IServiceCollection services,
            IOptions<ManagedDbOptions>? options = null) 
        {
            if (options != null) 
            {
                services.AddOptions<ManagedDbOptions>()
                    .Configure(option =>
                    {
                        option.GitHubBaseUrl = options.Value.GitHubBaseUrl;
                        option.Project = options.Value.Project;
                        option.Repository = options.Value.Repository;
                        option.Token = options.Value.Token;
                        option.PrId = options.Value.PrId;
                    });
            }

            services.AddScoped<IPullRequestService, GitHubPullRequestService>();

            services.AddHttpClient("github", (serviceProvider, httpClient) =>
            {
                var mdbOptions = serviceProvider.GetRequiredService<IOptions<ManagedDbOptions>>();

                httpClient.BaseAddress = new Uri(mdbOptions.Value.GitHubBaseUrl);

                httpClient.DefaultRequestHeaders.Add(
                    "Accept", 
                    "application/vnd.github.v3+json");

                httpClient.DefaultRequestHeaders.Add(
                    "User-Agent", 
                    "HttpRequestsSample");

                httpClient.DefaultRequestHeaders.Add(
                    "Authorization",
                    $"Bearer {mdbOptions.Value.Token}");
            });

            return services;
        }
    }

    public class ManagedDbOptions
    {
        public const string ConfigKey = "ManagedDb";

        public string GitHubBaseUrl { get; set; }

        public string Project { get; set; }

        public string Repository { get; set; }

        public int PrId { get; set; }

        public string Token { get; set; }

        public string PathToSave { get; set; }

        [Obsolete]
        public string RepoPath { get; set; }
    }
}
