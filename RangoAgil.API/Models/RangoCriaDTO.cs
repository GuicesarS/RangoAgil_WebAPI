﻿using System.ComponentModel.DataAnnotations;

namespace RangoAgil.API.Models;

public class RangoCriaDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Nome { get; set; }
}
