using LabApp.Shared.Data.EF.EventOutbox;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Data.EventOutbox
{
    public interface IContextWithEventOutbox
    {
        DbSet<EventMessage> EventOutbox { get; set; } 
    }
}