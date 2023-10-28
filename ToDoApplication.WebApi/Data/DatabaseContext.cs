using Microsoft.EntityFrameworkCore;
using ToDoApplication.WebApi.Models;

namespace ToDoApplication.WebApi.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {}

    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ToDoList> ToDoLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship between users and to do lists
        modelBuilder.Entity<User>()
            .HasMany(u => u.ToDoLists)
            .WithOne(tdl => tdl.User)
            .HasForeignKey(tdl => tdl.UserId);

        // Configure the relationship between to do lists and to do items
        modelBuilder.Entity<ToDoList>()
            .HasMany(tdl => tdl.ToDos)
            .WithOne(td => td.ToDoList)
            .HasForeignKey(td => td.ToDoListId);
    }
}
