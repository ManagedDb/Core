using ManagedDb.Core.Features.DataProxyCreators;
using ManagedDb.Core.Features.GenerateDbs;
using ManagedDb.Core.Features.GetLatestChanges;
using ManagedDb.Core.Features.PullRequests;
using ManagedDb.Core.Features.SchemaConvertors;
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
            // register latest changes services
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

            services.AddKeyedScoped<IPullRequestService, LocalPullRequestService>("local");
            services.AddKeyedScoped<IPullRequestService, GitHubPullRequestService>("github");

            services.AddHttpClient("github", (serviceProvider, httpClient) =>
            {
                var mdbOptions = serviceProvider.GetRequiredService<IOptions<ManagedDbOptions>>();

                httpClient.BaseAddress = new Uri(mdbOptions.Value.GitHubBaseUrl ?? string.Empty);

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

            // register schema
            services.AddScoped<SchemaProvider>();
            services.AddScoped<SchemaConvertor>();

            // register db generator
            services.AddScoped<DbGenerator>();

            return services;
        }
    }

    public class ManagedDbOptions
    {
        /// <summary>
        /// Key in appsettings.json file.
        /// </summary>
        public const string ConfigKey = "ManagedDb";

        /// <summary>
        /// Url for github.
        /// </summary>
        public string? GitHubBaseUrl { get; set; }

        /// <summary>
        /// Github project name.
        /// </summary>
        public string? Project { get; set; }

        /// <summary>
        /// Github repo name.
        /// </summary>
        public string? Repository { get; set; }

        /// <summary>
        /// PR id. The changes will be recieved by this id.
        /// </summary>
        public int PrId { get; set; }

        /// <summary>
        /// GitHub token.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Place to save changes.json file.
        /// </summary>
        public string? PathToSave { get; set; }

        /// <summary>
        /// Current repo path.
        /// </summary>
        public string? RepoPath { get; set; }

        /// <summary>
        /// Place for data folder to get entities and data.
        /// </summary>
        public string? DataFolderPath { get; set; }

        /// <summary>
        /// Path to save generated db.
        /// </summary>
        public string? DbPath { get; set; }
    }
}
