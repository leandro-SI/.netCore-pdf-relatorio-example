using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace RelatorioPDF.Config
{
    public class EventosDePagina : PdfPageEventHelper
    {
        public PdfContentByte Wdc { get; set; }
        public BaseFont FonteBaseRodape { get; set; }
        public iTextSharp.text.Font FonteRodape { get; set; }
        public int TotalPaginas { get; set; }

        public EventosDePagina(int totalPaginas)
        {
            FonteBaseRodape = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            FonteRodape = new iTextSharp.text.Font(FonteBaseRodape, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.Black);
            this.TotalPaginas = totalPaginas;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            this.Wdc = writer.DirectContent;
        }
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            AdicionarMomentoGeracaoRelatorio(writer, document);
            AdicionarNumerosDasPaginas(writer, document);
        }

        private void AdicionarMomentoGeracaoRelatorio(PdfWriter writer, Document document)
        {
            var textoMomentoGeracao = $"Gerado em {DateTime.Now.ToShortDateString()} às " + $"{DateTime.Now.ToShortTimeString()}";

            Wdc.BeginText();
            Wdc.SetFontAndSize(FonteRodape.BaseFont, FonteRodape.Size);
            Wdc.SetTextMatrix(document.LeftMargin, document.BottomMargin * 0.75f);
            Wdc.ShowText(textoMomentoGeracao);
            Wdc.EndText();
        }

        private void AdicionarNumerosDasPaginas(PdfWriter writer, Document document)
        {
            int paginaAtual = writer.PageNumber;
            var textoPaginacao = $"Página {paginaAtual} de {TotalPaginas}";

            float larguraTextoPaginacao = FonteBaseRodape.GetWidthPoint(textoPaginacao, FonteRodape.Size);

            var tamanhoPagina = document.PageSize;

            Wdc.BeginText();
            Wdc.SetFontAndSize(FonteRodape.BaseFont, FonteRodape.Size);
            Wdc.SetTextMatrix(tamanhoPagina.Width - document.RightMargin - larguraTextoPaginacao, document.BottomMargin * 0.75f);
            Wdc.ShowText(textoPaginacao);
            Wdc.EndText();

        }

    }
}
