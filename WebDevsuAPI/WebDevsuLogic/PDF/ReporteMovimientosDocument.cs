using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WebDevsuDatabase.Models.Consulta;

public class ReporteMovimientosDocument : IDocument
{
    public List<MovimientosReporte> Movimientos { get; }
    public string NombreCliente { get; }
    public DateTime FechaDesde { get; }
    public DateTime FechaHasta { get; }

    public ReporteMovimientosDocument(
        List<MovimientosReporte> movimientos,
        DateTime fechaDesde,
        DateTime fechaHasta)
    {
        Movimientos = movimientos ?? new List<MovimientosReporte>();
        NombreCliente = movimientos?.FirstOrDefault()?.Cliente ?? "N/A";
        FechaDesde = fechaDesde;
        FechaHasta = fechaHasta;
    }

    public DocumentMetadata GetMetadata() => new DocumentMetadata
    {
        Title = "Reporte de Movimientos",
        Author = "Sistema Bancario",
        Subject = $"Movimientos de {NombreCliente}",
        Creator = "QuestPDF",
        CreationDate = DateTime.Now
    };

    public DocumentSettings GetSettings() => new DocumentSettings
    {
        PdfA = false
    };

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape()); // Horizontal para más espacio
            page.Margin(30);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(9));

            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    void ComposeHeader(IContainer container)
    {
        container.Column(col =>
        {
            // Título principal
            col.Item().AlignCenter().PaddingBottom(10).Column(titleCol =>
            {
                titleCol.Item().Text("REPORTE DE MOVIMIENTOS BANCARIOS")
                    .FontSize(18)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);

                titleCol.Item().PaddingTop(3).LineHorizontal(2)
                    .LineColor(Colors.Blue.Darken2);
            });

            // Información del cliente y fechas
            col.Item().PaddingTop(10).PaddingBottom(10).Row(row =>
            {
                row.RelativeItem().Column(leftCol =>
                {
                    leftCol.Item().Text(text =>
                    {
                        text.Span("Cliente: ").Bold();
                        text.Span(NombreCliente);
                    });
                });

                row.RelativeItem().Column(rightCol =>
                {
                    rightCol.Item().AlignRight().Text(text =>
                    {
                        text.Span("Período: ").Bold();
                        text.Span($"{FechaDesde:dd/MM/yyyy} - {FechaHasta:dd/MM/yyyy}");
                    });
                });
            });

            // Línea separadora
            col.Item().PaddingBottom(10).LineHorizontal(1)
                .LineColor(Colors.Grey.Medium);
        });
    }

    void ComposeContent(IContainer container)
    {
        container.Column(col =>
        {
            // Verificar si hay datos
            if (!Movimientos.Any())
            {
                col.Item().PaddingVertical(50).AlignCenter().Text("No se encontraron movimientos para el período seleccionado")
                    .FontSize(12)
                    .Italic()
                    .FontColor(Colors.Grey.Darken1);
                return;
            }

            // Tabla de movimientos
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(75);   // Fecha
                    columns.ConstantColumn(110);  // Cuenta
                    columns.ConstantColumn(90);   // Tipo Cuenta
                    columns.ConstantColumn(90);   // Saldo Inicial
                    columns.ConstantColumn(50);   // Estado
                    columns.ConstantColumn(90);   // Movimiento
                    columns.ConstantColumn(100);  // Saldo Disponible
                });

                // ENCABEZADOS
                table.Header(header =>
                {
                    header.Cell().Element(HeaderStyle).Text("Fecha");
                    header.Cell().Element(HeaderStyle).Text("N° Cuenta");
                    header.Cell().Element(HeaderStyle).Text("Tipo");
                    header.Cell().Element(HeaderStyle).Text("Saldo Inicial");
                    header.Cell().Element(HeaderStyle).Text("Estado");
                    header.Cell().Element(HeaderStyle).Text("Movimiento");
                    header.Cell().Element(HeaderStyle).Text("Saldo Final");
                });

                // FILAS DE DATOS
                foreach (var m in Movimientos)
                {
                    table.Cell().Element(CellStyle).Text(m.Fecha?.ToString("dd/MM/yyyy") ?? "-");

                    table.Cell().Element(CellStyle).Text(m.NumeroCuenta ?? "-");

                    table.Cell().Element(CellStyle).Text(m.TipoCuenta == "A" ? "Ahorros": "Corriente" ?? "-");

                    table.Cell().Element(CellStyle).AlignRight()
                        .Text($"${m.SaldoInicial?.ToString("N2") ?? "0.00"}");

                    table.Cell().Element(CellStyle).AlignCenter().Text(text =>
                    {
                        var estado = m.Estado == true ? "Activo" : "Inactivo";
                        var color = m.Estado == true ? Colors.Green.Darken2 : Colors.Red.Darken2;
                        text.Span(estado).FontColor(color).FontSize(8).Bold();
                    });

                    table.Cell().Element(CellStyle).AlignRight().Text(text =>
                    {
                        var valor = m.ValorMovimiento ?? 0;
                        var color = valor >= 0 ? Colors.Green.Darken2 : Colors.Red.Darken2;
                        var signo = valor >= 0 ? "+" : "";
                        text.Span($"{signo}${valor:N2}").FontColor(color).Bold();
                    });

                    table.Cell().Element(CellStyle).AlignRight()
                        .Text($"${m.SaldoDisponible?.ToString("N2") ?? "0.00"}");
                }
            });

            // Sección de resumen
            col.Item().PaddingTop(20).Element(ComposeResumen);
        });
    }

    void ComposeResumen(IContainer container)
    {
        var totalCreditos = Movimientos.Where(m => m.ValorMovimiento > 0).Sum(m => m.ValorMovimiento ?? 0);
        var totalDebitos = Movimientos.Where(m => m.ValorMovimiento < 0).Sum(m => m.ValorMovimiento ?? 0);
        var totalMovimientos = Movimientos.Count;

        container.Column(col =>
        {
            col.Item().BorderTop(2).BorderColor(Colors.Grey.Darken1);

            col.Item().PaddingTop(15).Row(row =>
            {
                // Columna izquierda - Estadísticas
                row.RelativeItem().Column(statsCol =>
                {
                    statsCol.Item().Text("RESUMEN DEL PERÍODO")
                        .FontSize(11)
                        .Bold()
                        .FontColor(Colors.Blue.Darken2);

                    statsCol.Item().PaddingTop(8).Text(text =>
                    {
                        text.Span("Total de movimientos: ").FontSize(9);
                        text.Span(totalMovimientos.ToString()).Bold().FontSize(9);
                    });
                });

                // Columna derecha - Totales
                row.ConstantItem(300).Background(Colors.Grey.Lighten3)
                    .Padding(10).Column(totalsCol =>
                    {
                        totalsCol.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Total Créditos:").FontSize(9).Bold();
                            r.RelativeItem().AlignRight().Text($"+${totalCreditos:N2}")
                                .FontColor(Colors.Green.Darken2)
                                .FontSize(9)
                                .Bold();
                        });

                        totalsCol.Item().PaddingTop(5).Row(r =>
                        {
                            r.RelativeItem().Text("Total Débitos:").FontSize(9).Bold();
                            r.RelativeItem().AlignRight().Text($"${totalDebitos:N2}")
                                .FontColor(Colors.Red.Darken2)
                                .FontSize(9)
                                .Bold();
                        });

                        totalsCol.Item().PaddingTop(5).BorderTop(1)
                            .BorderColor(Colors.Grey.Medium)
                            .PaddingTop(5).Row(r =>
                            {
                                r.RelativeItem().Text("Balance Neto:").FontSize(10).Bold();
                                var balanceNeto = totalCreditos + totalDebitos;
                                var colorBalance = balanceNeto >= 0 ? Colors.Green.Darken3 : Colors.Red.Darken3;
                                r.RelativeItem().AlignRight().Text($"${balanceNeto:N2}")
                                .FontColor(colorBalance)
                                .FontSize(10)
                                .Bold();
                            });
                    });
            });
        });
    }

    void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Column(col =>
        {
            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

            col.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem().AlignLeft().Text(text =>
                {
                    text.Span("Generado: ").FontSize(8);
                    text.Span($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}").FontSize(8);
                });

                row.RelativeItem().AlignCenter().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });

                row.RelativeItem().AlignRight().Text("Sistema Bancario")
                    .FontSize(8)
                    .Italic();
            });
        });
    }

    IContainer HeaderStyle(IContainer container)
    {
        return container
            .Background(Colors.Blue.Darken2)
            .Padding(8)
            .AlignCenter()
            .AlignMiddle()
            .DefaultTextStyle(x => x.Bold().FontSize(9).FontColor(Colors.White));
    }

    IContainer CellStyle(IContainer container)
    {
        return container
            .Padding(6)
            .BorderBottom(0.5f)
            .BorderColor(Colors.Grey.Lighten2)
            .AlignMiddle();
    }
}