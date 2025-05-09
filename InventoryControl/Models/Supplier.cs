﻿namespace InventoryControl.Models;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public List<Product> Products { get; set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; set; }
}