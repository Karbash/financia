﻿
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;
using System.Reflection;

namespace Financia.Application.UseCases.Expenses.Reports.Pdf.Fonts
{
    public class ExpenseReportFontResolver : IFontResolver
    {
        public byte[]? GetFont(string faceName)
        {
            var stream = ReadFontFile(faceName);

            if (stream == null)
            {
                stream = ReadFontFile(FontHelper.DEFAULT_FONT);
            }

            var length = (int)stream!.Length;
            
            var data = new byte[length];

            stream.Read( buffer: data , offset: 0, count: length );

            return data;

        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
        {
            return new FontResolverInfo(familyName);
        }

        private Stream? ReadFontFile(string faceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream($"Financia.Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf");
        }
    }
}
