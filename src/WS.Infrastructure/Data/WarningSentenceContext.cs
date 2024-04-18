using Microsoft.EntityFrameworkCore;
using WS.Core.Entities.WSAggregate;

namespace WS.Infrastructure.Data;

public class WarningSentenceContext : DbContext
{
    //WarningSentence Aggregate
    public DbSet<WarningSentence>? WarningSentences { get; set; }
    public DbSet<WarningCategory>? WarningCategories { get; set; }
    public DbSet<WarningType>? WarningTypes { get; set; }
    public DbSet<WarningSignalWord>? WarningSignalWords { get; set; }
    public DbSet<WarningPictogram>? WarningPictograms { get; set; }

    public WarningSentenceContext(DbContextOptions<WarningSentenceContext> options) : base(options)
    {
    }
    
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Primary keys
        modelBuilder.Entity<WarningSentence>().HasKey(warningSentence => warningSentence.Id);
        modelBuilder.Entity<WarningCategory>().HasKey(warningCategory => warningCategory.Id);
        modelBuilder.Entity<WarningType>().HasKey(warningType => warningType.Id);
        modelBuilder.Entity<WarningSignalWord>().HasKey(warningSignalWord => warningSignalWord.Id);
        modelBuilder.Entity<WarningPictogram>().HasKey(warningPictogram => warningPictogram.Id);
        
        //Relationships
        
        //WarningSentence to WarningCategory (one to many)
        modelBuilder.Entity<WarningSentence>()
            .HasOne(warningSentence => warningSentence.WarningCategory)
            .WithMany(warningCategory => warningCategory.WarningSentences)
            .HasForeignKey(w => w.WarningCategoryId);
        
        //WarningSentence to WarningSignalWord (one to many)
        modelBuilder.Entity<WarningSentence>()
            .HasOne(warningSentence => warningSentence.WarningSignalWord)
            .WithMany(warningSignalWord => warningSignalWord.WarningSentences)
            .HasForeignKey(w => w.WarningSignalWordId);
        
        //WarningSentence to WarningPictogram (one to many)
        modelBuilder.Entity<WarningSentence>()
            .HasOne(warningSentence => warningSentence.WarningPictogram)
            .WithMany(warningPictogram => warningPictogram.WarningSentences)
            .HasForeignKey(w => w.WarningPictogramId);
        
        //WarningCategory to WarningType (one to many)
        modelBuilder.Entity<WarningCategory>()
            .HasOne(warningCategory => warningCategory.WarningType)
            .WithMany(warningType => warningType.WarningCategories)
            .HasForeignKey(w => w.WarningTypeId);
    }
}