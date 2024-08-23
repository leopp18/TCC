using System;
using System.Collections.Generic;
using API_TCC.Models;
using Microsoft.EntityFrameworkCore;

namespace API_TCC.Data;

public partial class Contexto : DbContext
{
    public Contexto()
    {
    }

    public Contexto(DbContextOptions<Contexto> options)
        : base(options)
    {
    }

    public virtual DbSet<Cidade> Cidades { get; set; }

    public virtual DbSet<Entrega> Entregas { get; set; }

    public virtual DbSet<EntregadorEntrega> EntregadorEntregas { get; set; }

    public virtual DbSet<Entregadores> Entregadores { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<PagamentoEntrega> PagamentoEntregas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=modolog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cidade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__cidades__3213E83F4E2A82AF");

            entity.ToTable("cidades");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkEstado).HasColumnName("fk_estado");
            entity.Property(e => e.Nome)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("nome");

            entity.HasOne(d => d.FkEstadoNavigation).WithMany(p => p.Cidades)
                .HasForeignKey(d => d.FkEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cidades__fk_esta__286302EC");
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__entregas__3213E83FE6D09B25");

            entity.ToTable("entregas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Valor)
                .HasColumnType("money")
                .HasColumnName("valor");
        });

        modelBuilder.Entity<EntregadorEntrega>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__entregad__3213E83FE5C9A83D");

            entity.ToTable("entregador_entrega");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkEntrega).HasColumnName("fk_entrega");
            entity.Property(e => e.FkEntregador).HasColumnName("fk_entregador");

            entity.HasOne(d => d.FkEntregaNavigation).WithMany(p => p.EntregadorEntregas)
                .HasForeignKey(d => d.FkEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__entregado__fk_en__300424B4");

            entity.HasOne(d => d.FkEntregadorNavigation).WithMany(p => p.EntregadorEntregas)
                .HasForeignKey(d => d.FkEntregador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__entregado__fk_en__2F10007B");
        });

        modelBuilder.Entity<Entregadores>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__entregad__3213E83FB0245E21");

            entity.ToTable("entregadores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Pix)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("pix");
            entity.Property(e => e.Situacao).HasColumnName("situacao");
            entity.Property(e => e.Sobrenome)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("sobrenome");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__estados__3213E83FAE5E77B2");

            entity.ToTable("estados");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Sigla)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("sigla");
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pagament__3213E83F4B2C85A2");

            entity.ToTable("pagamentos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adiantamento)
                .HasColumnType("money")
                .HasColumnName("adiantamento");
            entity.Property(e => e.Adicional)
                .HasColumnType("money")
                .HasColumnName("adicional");
            entity.Property(e => e.Desconto)
                .HasColumnType("money")
                .HasColumnName("desconto");
            entity.Property(e => e.FkCidade).HasColumnName("fk_cidade");
            entity.Property(e => e.FkEntregador).HasColumnName("fk_entregador");
            entity.Property(e => e.NotaFiscal).HasColumnName("notaFiscal");
            entity.Property(e => e.Pago).HasColumnName("pago");
            entity.Property(e => e.Periodo).HasColumnName("periodo");

            entity.HasOne(d => d.FkCidadeNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.FkCidade)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pagamento__fk_ci__32E0915F");

            entity.HasOne(d => d.FkEntregadorNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.FkEntregador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pagamento__fk_en__33D4B598");
        });

        modelBuilder.Entity<PagamentoEntrega>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__pagament__3213E83FA2E41D80");

            entity.ToTable("pagamento_entrega");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkEntrega).HasColumnName("fk_entrega");
            entity.Property(e => e.FkPagamento).HasColumnName("fk_pagamento");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.FkEntregaNavigation).WithMany(p => p.PagamentoEntregas)
                .HasForeignKey(d => d.FkEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pagamento__fk_en__36B12243");

            entity.HasOne(d => d.FkPagamentoNavigation).WithMany(p => p.PagamentoEntregas)
                .HasForeignKey(d => d.FkPagamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pagamento__fk_pa__37A5467C");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__usuarios__3213E83F9CD6B219");

            entity.ToTable("usuarios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Permissao).HasColumnName("permissao");
            entity.Property(e => e.Senha)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("senha");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
