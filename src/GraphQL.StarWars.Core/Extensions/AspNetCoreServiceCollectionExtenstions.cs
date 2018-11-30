using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Http;
using GraphQL.StarWars;
using GraphQL.StarWars.Types;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.StartWars.Standard.Extensions
{
    public static class AspNetCoreServiceCollectionExtenstions
    {
        public static void AddStarWarsTypes(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddSingleton<StarWarsData>();
            services.AddSingleton<StarWarsQuery>();
            services.AddSingleton<StarWarsMutation>();
            services.AddSingleton<HumanType>();
            services.AddSingleton<HumanInputType>();
            services.AddSingleton<DroidType>();
            services.AddSingleton<CharacterInterface>();
            services.AddSingleton<EpisodeEnum>();
            services.AddSingleton<ISchema, StarWarsSchema>();
        }
    }
}
