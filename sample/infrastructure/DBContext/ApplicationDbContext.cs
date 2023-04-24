using Microsoft.EntityFrameworkCore;
using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Milestone>? Milestones { get; set; }
    public DbSet<Goal>? Goals { get; set; }
    //public DbSe <TaskBase> { get; set; }
}