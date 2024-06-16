using HataChatSerives.Models;
using Microsoft.EntityFrameworkCore;

namespace HataChatSerives.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Sender)
            .WithMany()
            .HasForeignKey(cm => cm.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Receiver)
            .WithMany()
            .HasForeignKey(cm => cm.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}