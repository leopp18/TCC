using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class PagamentoEntrega
{
    public int Id { get; set; }

    public int? Quantidade { get; set; }

    public int FkEntrega { get; set; }

    public int FkPagamento { get; set; }

    public virtual Entrega? FkEntregaNavigation { get; set; }

    public virtual Pagamento? FkPagamentoNavigation { get; set; }
}
