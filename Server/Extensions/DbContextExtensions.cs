using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LabApp.Server.Extensions
{
    public static class DbContextExtensions
    {
        public static string GetDefaultValueForDateTime(DatabaseFacade database)
        {
            if (database.IsSqlite()) return "(datetime('now'))";
            if (database.IsSqlServer()) return "(getdate())";
            if (database.IsNpgsql()) return "timezone('utc', now())";
            
            throw new ArgumentOutOfRangeException();
        }
    }
}