using InventoryControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryControl.Data.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("suppliers");

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
        
        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(255)")
            .IsRequired();
        
        builder.Property(c => c.PhoneNumber)
            .HasColumnName("phone_number")
            .HasColumnType("varchar(50)")
            .IsRequired();
    }
}