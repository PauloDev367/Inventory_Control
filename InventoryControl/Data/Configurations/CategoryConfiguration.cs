using InventoryControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryControl.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.HasIndex(c => c.Name).IsUnique();

        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.DeletedAt)
            .HasColumnName("deleted_at")
            .HasDefaultValue(null);
        
        builder.HasMany(c => c.Products)
            .WithMany(p=>p.Categories)
            .UsingEntity(j=>j.ToTable("CategoriesProducts"));
    }
}