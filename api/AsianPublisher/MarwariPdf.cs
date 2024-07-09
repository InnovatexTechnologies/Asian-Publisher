using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsianPublisher
{
    public static class MarwariPdf
    {
        public static BaseColor ToColor(this string color)
        {
            var arrColorFragments = color?.Split(',').Select(sFragment => { int.TryParse(sFragment, out int fragment); return fragment; }).ToArray();
            switch (arrColorFragments?.Length)
            {
                case 3:
                    return new BaseColor(arrColorFragments[0], arrColorFragments[1], arrColorFragments[2]);
                case 4:
                    return new BaseColor(arrColorFragments[0], arrColorFragments[1], arrColorFragments[2], arrColorFragments[3]);
                default:
                    return new BaseColor(0, 0, 0);
            }
        }
        public static void DrawLine(PdfContentByte cb, BaseColor color, float lineWidth, float x1, float y1, float x2, float y2)
        {
            cb.SetColorStroke(color);
            cb.SetLineWidth(lineWidth);
            cb.MoveTo(x1, y1);
            cb.LineTo(x2, y2);
            cb.Stroke();
        }
        public static void DrawLine(PdfContentByte cb, string color, float lineWidth, float x1, float y1, float x2, float y2)
        {
            cb.SetColorStroke(color.ToColor());
            cb.SetLineWidth(lineWidth);
            cb.MoveTo(x1, y1);
            cb.LineTo(x2, y2);
            cb.Stroke();
        }
        public static void DrawRectangle(PdfContentByte cb, BaseColor fillcolor, BaseColor bordercolor, float lbx, float lby, float rbx, float rby, float rtx, float rty, float ltx, float lty, float thickness)
        {
            cb.SetColorFill(fillcolor);//orange
            cb.SetLineWidth(thickness);
            cb.SetColorStroke(bordercolor);
            cb.MoveTo(lbx, lby);//lftbotm
            cb.LineTo(rbx, rby);//rytbtm
            cb.LineTo(rtx, rty);//ryttop
            cb.LineTo(ltx, lty);//lefttop
            cb.FillStroke();
            cb.Fill();
        }
        public static void DrawRectangle(PdfContentByte cb, string fillcolor, string bordercolor, float lbx, float lby, float rbx, float rby, float rtx, float rty, float ltx, float lty, float thickness)
        {
            cb.SetColorFill(fillcolor.ToColor());//orange
            cb.SetLineWidth(thickness);
            cb.SetColorStroke(bordercolor.ToColor());
            cb.MoveTo(lbx, lby);//lftbotm
            cb.LineTo(rbx, rby);//rytbtm
            cb.LineTo(rtx, rty);//ryttop
            cb.LineTo(ltx, lty);//lefttop
            cb.FillStroke();
            cb.Fill();
        }
        public static void DrawCircle(PdfContentByte cb, BaseColor fillcolor, BaseColor bordercolor, float lineWidth, float x, float y, float r)
        {
            cb.SetColorFill(fillcolor);
            cb.SetColorStroke(bordercolor);
            cb.SetLineWidth(lineWidth);
            cb.Circle(x, y, r);
            cb.FillStroke();
            cb.Fill();
        }
        public static void DrawCircle(PdfContentByte cb, string fillcolor, string bordercolor, float lineWidth, float x, float y, float r)
        {
            cb.SetColorFill(fillcolor.ToColor());
            cb.SetColorStroke(bordercolor.ToColor());
            cb.SetLineWidth(lineWidth);
            cb.Circle(x, y, r);
            cb.FillStroke();
            cb.Fill();
        }
        public static void DrawText(PdfContentByte cb, string fontstyle, string fontfamily, string color, float FontSize, int Alignment, string Text, float x, float y, float rotation)
        {
            cb.BeginText();
            BaseFont bf;
            if (fontfamily != "")
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                string path = AppDomain.CurrentDomain.BaseDirectory;
                bf = BaseFont.CreateFont(path + "Fonts\\" + fontfamily + ".ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            }
            else
            {
                if (fontstyle == "")
                {
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                }
                else
                {
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.EMBEDDED);
                }
            }
            cb.SetColorFill(color.ToColor());
            cb.SetFontAndSize(bf, FontSize);
            cb.ShowTextAligned(Alignment, Text, x, y, rotation);
            cb.EndText();
        }
        public static void DrawText(PdfContentByte cb, string fontstyle, string fontfamily, BaseColor color, float FontSize, int Alignment, string Text, float x, float y, float rotation)
        {
            cb.BeginText();
            BaseFont bf;
            if (fontfamily != "")
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                string path = AppDomain.CurrentDomain.BaseDirectory;
                bf = BaseFont.CreateFont(path + "Fonts\\" + fontfamily + ".ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            }
            else
            {
                if (fontstyle == "")
                {
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
                }
                else
                {
                    bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.EMBEDDED);
                }
            }
            cb.SetColorFill(color);
            cb.SetFontAndSize(bf, FontSize);
            cb.ShowTextAligned(Alignment, Text, x, y, rotation);
            cb.EndText();
        }
        public static void DrawColumnText(PdfContentByte cb, Phrase phrase, float llx, float lly, float urx, float ury, float lineSpace, int alignment)
        {
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(phrase, llx, lly, urx, ury, lineSpace, alignment);
            ct.Go(false);
        }
        public static void DrawColumnText(PdfContentByte cb, string text, string fontname, int size, int style, string color, float llx, float lly, float urx, float ury, float lineSpace, int alignment)
        {

            Phrase phrase;
            if (fontname != "")
            {
                string fontname1 = "Fonts\\" + fontname + ".ttf";
                phrase = new Phrase(new Chunk(text.Replace("<br/>", System.Environment.NewLine), FontFactory.GetFont(fontname1, size, style, color.ToColor())));
            }
            else
            {
                if (style == 0)
                {
                    phrase = new Phrase(new Chunk(text.Replace("<br/>", System.Environment.NewLine), FontFactory.GetFont(BaseFont.HELVETICA, size, style, color.ToColor())));
                }
                else if (style == 1)
                {
                    phrase = new Phrase(new Chunk(text.Replace("<br/>", System.Environment.NewLine), FontFactory.GetFont(BaseFont.HELVETICA_BOLD, size, style, color.ToColor())));
                }
                else
                {
                    phrase = new Phrase(new Chunk(text.Replace("<br/>", System.Environment.NewLine), FontFactory.GetFont(BaseFont.HELVETICA, size, style, color.ToColor())));
                }
            }

            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(phrase, llx, lly, urx, ury, lineSpace, alignment);
            ct.Go(false);
        }
        public static void AddCell(PdfPTable table, string text, bool isBold, int fontSize, int colspan = 1)
        {
            Font font = new Font(Font.FontFamily.HELVETICA, fontSize);
            if (isBold) font.SetStyle(Font.BOLD);

            PdfPCell cell = new PdfPCell(new Phrase(text, font))
            {
                Colspan = colspan,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Padding = 5
            };
            table.AddCell(cell);
        }
    }
}
