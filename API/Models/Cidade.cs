using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class Cidade
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public int FkEstado { get; set; }
    public bool? Situacao { get; set; }

    public virtual Estado? FkEstadoNavigation { get; set; }

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
