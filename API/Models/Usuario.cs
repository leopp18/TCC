using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Senha { get; set; }

    public int? Permissao { get; set; }
}
