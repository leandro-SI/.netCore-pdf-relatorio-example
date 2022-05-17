using iTextSharp.text;
using iTextSharp.text.pdf;
using RelatorioPDF.Config;
using RelatorioPDF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RelatorioPDF
{
    class Program
    {        

        static void Main(string[] args)
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            var _pathtemp = "";

            //gerando lista de pessoas
            for (var i = 0; i < 5; i++)
            {
                Pessoa pessoa = new Pessoa();
                pessoa.Nome = "Leandro Cesar";
                pessoa.Idade = 30;
                pessoa.DataNascimento = DateTime.Now;
                pessoa.Sexo = "Masculino";
                pessoa.Ativo = "SIM";
                pessoas.Add(pessoa);
            }

            //cálculo da quantidade total de páginas
            int totalPaginas = 1;
            int totalLinhas = pessoas.Count;
            if (totalLinhas > 24)
            {
                totalPaginas += (int)Math.Ceiling((totalLinhas - 24) / 29F);
            }

            var pxPorMin = 72 / 25.2F;
            var pdf = new Document(PageSize.A4, 15 * pxPorMin, 15 * pxPorMin, 15 * pxPorMin, 20 * pxPorMin);

            //configuração
            var nomeArquivo = $"relatorio.{DateTime.Now.ToString("yyy.MM.dd.HH.mm.ss")}.pdf";

            if (!System.IO.Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"\\temp")))
            {
                System.IO.Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"temp"));
            }

            var caminhoPDF = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"temp", nomeArquivo);
            _pathtemp = caminhoPDF;

            using (var stream = new FileStream(caminhoPDF, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(pdf, stream);

                writer.PageEvent = new EventosDePagina(totalPaginas);

                pdf.Open();

                var fontBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                //título
                var fontParagrafo = new iTextSharp.text.Font(fontBase, 32, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                var titulo = new Paragraph("Relatório de Pessoas\n\n", fontParagrafo);
                titulo.Alignment = Element.ALIGN_LEFT;
                titulo.SpacingAfter = 4;
                pdf.Add(titulo);

                //cliente
                var fontCliente = new iTextSharp.text.Font(fontBase, 10, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                var textoCliente = new Paragraph("Cliente: " + "Leandro Cesar" + "\n\n", fontCliente);
                textoCliente.Alignment = Element.ALIGN_LEFT;
                textoCliente.SpacingAfter = 4;
                pdf.Add(textoCliente);

                //adição da imagem
                //var caminhoImagem = Path.GetFullPath("temp\\dev.jfif");

                var caminhoImagem = @"\Imagem\dev.jfif";

                if (System.IO.File.Exists(caminhoImagem))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoImagem);
                    float razaoAlturaLargura = logo.Width / logo.Height;
                    float alturaLogo = 50;
                    float larguraLogo = alturaLogo * razaoAlturaLargura;
                    logo.ScaleToFit(larguraLogo, alturaLogo);
                    var margemEsquerda = pdf.PageSize.Width - pdf.RightMargin - larguraLogo;
                    var margemTopo = pdf.PageSize.Height - pdf.TopMargin - 54;
                    logo.SetAbsolutePosition(margemEsquerda, margemTopo);
                    writer.DirectContent.AddImage(logo, false);
                }

                //adição da tabela de dados
                var tabela = new PdfPTable(5);
                float[] larguraColunas = { 1f, 1.5f, 1.5f, 1f, 1f };
                tabela.SetWidths(larguraColunas);
                tabela.DefaultCell.BorderWidth = 0;
                tabela.WidthPercentage = 100;

                Configuracoes.CriarCelulaTexto(tabela, "Nome", PdfPCell.ALIGN_LEFT, true);
                Configuracoes.CriarCelulaTexto(tabela, "Idade", PdfPCell.ALIGN_LEFT, true);
                Configuracoes.CriarCelulaTexto(tabela, "Data Nascimento", PdfPCell.ALIGN_LEFT, true);
                Configuracoes.CriarCelulaTexto(tabela, "Sexo", PdfPCell.ALIGN_LEFT, true);
                Configuracoes.CriarCelulaTexto(tabela, "Ativo", PdfPCell.ALIGN_LEFT, true);


                foreach (var e in pessoas)
                {
                    Configuracoes.CriarCelulaTexto(tabela, e.Nome, PdfPCell.ALIGN_LEFT, false);
                    Configuracoes.CriarCelulaTexto(tabela, e.Idade.ToString(), PdfPCell.ALIGN_LEFT, false);
                    Configuracoes.CriarCelulaTexto(tabela, e.DataNascimento.ToString(), PdfPCell.ALIGN_LEFT, false);
                    Configuracoes.CriarCelulaTexto(tabela, e.Sexo, PdfPCell.ALIGN_LEFT, false);
                    Configuracoes.CriarCelulaTexto(tabela, e.Ativo, PdfPCell.ALIGN_LEFT, false);

                }

                pdf.Add(tabela);
                pdf.Close();

                if (System.IO.File.Exists(caminhoPDF))
                {

                    Process.Start(new ProcessStartInfo()
                    {
                        Arguments = $"/c start {caminhoPDF}",
                        FileName = "cmd.exe",
                        CreateNoWindow = true
                    });

                }

            };

            Console.ReadKey();
        }
    }
}
