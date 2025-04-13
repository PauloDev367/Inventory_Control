﻿using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Requests;

public class UserLoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Senha { get; set; }

}