using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class Estado
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Sigla { get; set; }

    public virtual ICollection<Cidade> Cidades { get; set; } = new List<Cidade>();
}
