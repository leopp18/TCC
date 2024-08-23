using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class EntregadorEntrega
{
    public int Id { get; set; }

    public int FkEntregador { get; set; }

    public int FkEntrega { get; set; }

    public virtual Entrega FkEntregaNavigation { get; set; } = null!;

    public virtual Entregadores FkEntregadorNavigation { get; set; } = null!;
}
