using System;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace LabApp.Shared.EventConsistency.Stores.EF
{
    public class CreateTablesStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return async x =>
            {
                await x.ApplicationServices.GetRequiredService<IEventConsistencyMigrator>().MigrateAsync();
                next(x);
            };
        }
    }
}