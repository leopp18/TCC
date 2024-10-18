using System;
using System.Collections.Generic;

namespace API_TCC.Models;

public partial class Pagamento
{
    public int Id { get; set; }

    public decimal? Adicional { get; set; }

    public decimal? Desconto { get; set; }

    public decimal? Adiantamento { get; set; }

    public bool? Pago { get; set; }

    public bool? NotaFiscal { get; set; }

    public int FkCidade { get; set; }

    public int FkEntregador { get; set; }

    public DateOnly Periodo { get; set; }

    public virtual Cidade? FkCidadeNavigation { get; set; } = null!;

    public virtual Entregadores? FkEntregadorNavigation { get; set; } = null!;

    public virtual ICollection<PagamentoEntrega> PagamentoEntregas { get; set; } = new List<PagamentoEntrega>();
}
