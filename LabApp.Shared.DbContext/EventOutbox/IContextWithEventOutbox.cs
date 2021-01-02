using LabApp.Shared.DbContext.Models;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.DbContext.EventOutbox
{
    public interface IContextWithEventOutbox
    {
        DbSet<EventMessage> EventOutbox { get; set; } 
    }
}