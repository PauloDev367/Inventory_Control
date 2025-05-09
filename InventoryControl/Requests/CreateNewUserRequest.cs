﻿using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class CreateNewUserRequest
{
    [Required]
    public string Nome { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(50)]
    public string Senha { get; set; }
    [Compare(nameof(Senha))]
    public string SenhaConfirmacao { get; set; }
}
