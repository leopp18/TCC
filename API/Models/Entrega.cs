using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class Entrega
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public decimal? Valor { get; set; }

    public virtual ICollection<EntregadorEntrega> EntregadorEntregas { get; set; } = new List<EntregadorEntrega>();

    public virtual ICollection<PagamentoEntrega> PagamentoEntregas { get; set; } = new List<PagamentoEntrega>();
}
