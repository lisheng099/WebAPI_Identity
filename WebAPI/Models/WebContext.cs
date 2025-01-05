using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class WebContext : IdentityDbContext<ApplicationUser>
{

    /*
    Add-Migration <MigrationName> -Context <ContextName>  //建立遷移 <MigrationName>  每次都需不同
    Update-Database -Context WebContext //更新資料庫
    */

    public WebContext(DbContextOptions<WebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsFiles> NewsFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.NewsId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.EndDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.InsertDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.StartDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.UpdateDateTime).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.Enable).HasDefaultValue(true);

            entity.HasOne(d => d.InsertEmployee)
                    .WithMany(p => p.NewsInsertEmployees)
                    .HasForeignKey(d => d.InsertEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.UpdateEmployee)
                .WithMany(p => p.NewsUpdateEmployees)
                .HasForeignKey(d => d.UpdateEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<NewsFiles>(entity =>
        {
            entity.Property(e => e.NewsFilesId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Extension).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.HasOne(d => d.News)
                    .WithMany(p => p.NewsFiles)
                    .HasForeignKey(d => d.NewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
