using Microsoft.EntityFrameworkCore;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Engine;

namespace TaskProcessor.Infrastructure.Persistence
{
	public class LocalDBContexts : DbContext
	{
		public DbSet<TodoItem> Todos { get; set; }
		public DbSet<TodoList> TodoLists { get; set; }

		public LocalDBContexts(DbContextOptions<LocalDBContexts> options)
			: base(options)
		{
		}

		override protected void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}

	public class ProcessorDBContexts : DbContext
	{
		public DbSet<TaskMessage> Messages { get; set; }

		public ProcessorDBContexts(DbContextOptions<ProcessorDBContexts> options)
			: base(options)
		{
		}

		override protected void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<TaskMessage>().HasKey(p => p.Id);
			modelBuilder.Owned<StepTask>();
		}
	}
}
