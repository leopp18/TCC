using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class Entregadores
{
    public int Id { get; set; }

    public string? Nome { get; set; }

    public string? Sobrenome { get; set; }

    public string? Pix { get; set; }

    public bool? Situacao { get; set; }

    public virtual ICollection<EntregadorEntrega> EntregadorEntregas { get; set; } = new List<EntregadorEntrega>();

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
