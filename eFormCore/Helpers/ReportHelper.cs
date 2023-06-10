/*
The MIT License (MIT)

Copyright (c) 2007 - 2020 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;
using ImageMagick;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace Microting.eForm.Helpers
{
    public static class ReportHelper
    {
        public static void SearchAndReplace(SortedDictionary<string, string> valuesToReplace,
            WordprocessingDocument wordDoc)
        {
            SearchAndReplaceHeaders(wordDoc, valuesToReplace);

            SearchAndReplaceBody(wordDoc, valuesToReplace);

            SearchAndReplaceFooters(wordDoc, valuesToReplace);
        }

        private static void SearchAndReplaceHeaders(WordprocessingDocument wordDoc,
            SortedDictionary<string, string> valuesToReplace)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "SearchAndReplaceHeaders called"));
            foreach (HeaderPart headerPart in wordDoc.MainDocumentPart.HeaderParts)
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(headerPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                docText = SearchAndReplace(docText, valuesToReplace);

                using (StreamWriter sw = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        private static void SearchAndReplaceBody(WordprocessingDocument wordDoc,
            SortedDictionary<string, string> valuesToReplace)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "SearchAndReplaceBody called"));
            string docText = null;
            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();
            }

            docText = SearchAndReplace(docText, valuesToReplace);

            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(docText);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
        }

        private static void SearchAndReplaceFooters(WordprocessingDocument wordDoc,
            SortedDictionary<string, string> valuesToReplace)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "SearchAndReplaceFooters called"));
            foreach (FooterPart footerPart in wordDoc.MainDocumentPart.FooterParts)
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(footerPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                docText = SearchAndReplace(docText, valuesToReplace);

                using (StreamWriter sw = new StreamWriter(footerPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
        }

        private static string SearchAndReplace(string docText, SortedDictionary<string, string> valuesToReplace)
        {
            foreach (var fieldValue in valuesToReplace.Reverse())
            {
                WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper.SearchAndReplace",
                    $"replacing {fieldValue.Key} with {fieldValue.Value}"));
                if (fieldValue.Value != null)
                {
                    Regex regexText = new Regex(fieldValue.Key);
                    if (fieldValue.Value != "&#10004;")
                    {
                        docText = regexText.Replace(docText, fieldValue.Value
                            .Replace("&", "&amp;"));
                    }
                    else
                    {
                        docText = regexText.Replace(docText, fieldValue.Value);
                    }
                }
                else
                {
                    Regex regexText = new Regex(fieldValue.Key);
                    docText = regexText.Replace(docText, "");
                }
            }

            Regex regexText2 = new Regex("FreeSans");
            docText = regexText2.Replace(docText, "Arial");

            return docText;
        }

        public static void InsertSignature(WordprocessingDocument wordDoc, List<KeyValuePair<string, string>> pictures)
        {
            foreach (var keyValuePair in pictures)
            {
                WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper.InsertSignature",
                    $"inserting signature {keyValuePair.Key}"));
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                long iWidth = 0;
                long iHeight = 0;

                var bytes = Convert.FromBase64String(keyValuePair.Value);

                using var image = new MagickImage(bytes);
                iWidth = image.Width;
                iHeight = image.Height;

                iWidth = (long)Math.Round((decimal)iWidth * 9525);
                iHeight = (long)Math.Round((decimal)iHeight * 9525);

                double maxWidthCm = 16.5;
                const int emusPerCm = 360000;
                long maxWidthEmus = (long)(maxWidthCm * emusPerCm);
                if (iWidth > maxWidthEmus)
                {
                    var ratio = (iHeight * 1.0m) / iWidth;
                    iWidth = maxWidthEmus;
                    iHeight = (long)(iWidth * ratio);
                }

                imagePart.FeedData(new MemoryStream(bytes));

                AddSignatureToParagraph(wordDoc, mainPart.GetIdOfPart(imagePart), iWidth, iHeight, keyValuePair.Key);
            }

            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper.InsertSignature", "Done inserting signatures"));
        }

        public static void InsertImages(WordprocessingDocument wordDoc,
            List<KeyValuePair<string, List<string>>> pictures)
        {
            string currentHeader = "";
            List<Paragraph> foundEntriesToClearText = new List<Paragraph>();

            foreach (var keyValuePair in pictures)
            {
                WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper.InsertImages",
                    $"inserting image {keyValuePair.Key}"));

                var lookupKey = keyValuePair.Value[2];
                lookupKey = $"FPictures_{lookupKey}";

                var paragraphs = wordDoc.MainDocumentPart.Document.Body.Descendants<Paragraph>();
                var paragraph = paragraphs.FirstOrDefault(p => p.InnerText.Contains(lookupKey));

                if (paragraph != null)
                {
                    InsertPicture(keyValuePair.Value, wordDoc, paragraph);
                    foundEntriesToClearText.Add(paragraph);
                }
                else
                {
                    if (currentHeader != keyValuePair.Key)
                    {
                        if (currentHeader != "")
                        {
                            SectionProperties sectPr =
                                (SectionProperties)wordDoc.MainDocumentPart.Document.Body.ChildElements.Last();

                            wordDoc.MainDocumentPart.Document.Body.InsertBefore(
                                new Paragraph(
                                    new Run(
                                        new Break { Type = BreakValues.Page }
                                    )),
                                sectPr
                            );
                        }

                        InsertHeader(keyValuePair.Key, wordDoc, currentHeader);
                    }

                    currentHeader = keyValuePair.Key;
                    InsertPicture(keyValuePair.Value, wordDoc);
                }
            }

            foreach (var paragraph in foundEntriesToClearText)
            {
                try
                {
                    var run = paragraph.Descendants<Run>().FirstOrDefault();
                    var text = run.Descendants<Text>().FirstOrDefault();
                    text.Text = "";
                } catch (Exception e)
                {
                    WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper.InsertImages",
                        $"Error clearing text: {e.Message}"));
                }

            }

            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper.InsertImages", "Done inserting images"));
        }

        public static void ConvertToPdf(string docxFileName, string outputFolder)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "ConvertToPdf called"));
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "ConvertToPdf docxFileName: " + docxFileName));
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "ConvertToPdf outputFolder: " + outputFolder));
            try
            {
                outputFolder += "/";
                using Process pdfProcess = new Process();
                pdfProcess.StartInfo.UseShellExecute = false;
                pdfProcess.StartInfo.RedirectStandardOutput = true;
                pdfProcess.StartInfo.FileName = "soffice";
                pdfProcess.StartInfo.Arguments = $" --headless --convert-to pdf {docxFileName} --outdir {outputFolder}";
                pdfProcess.Start();
                string output = pdfProcess.StandardOutput.ReadToEnd();
                Trace.WriteLine(output);
                pdfProcess.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static void InsertHeader(string header, WordprocessingDocument wordDoc, string currentHeader)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "InsertHeader called"));
            // If currentHeader is not equal to new header, insert new header.
            try
            {
                if (header != currentHeader)
                {
                    currentHeader = header;
                    SectionProperties sectPr =
                        (SectionProperties)wordDoc.MainDocumentPart.Document.Body.ChildElements.Last();

                    wordDoc.MainDocumentPart.Document.Body.InsertBefore(new Paragraph(
                            new Run(
                                new RunProperties(
                                    new RunFonts { Ascii = "Arial", HighAnsi = "Arial" }
                                ),
                                new Text(currentHeader)
                            )
                        ),
                        sectPr
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void InsertPicture(List<string> values, WordprocessingDocument wordDoc,
            Paragraph paragraph = null)
        {
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

            long iWidth = 0;
            long iHeight = 0;
            var bytes = Convert.FromBase64String(values[0]);

            using var image = new MagickImage(bytes);
            iWidth = image.Width;
            iHeight = image.Height;

            iWidth = (long)Math.Round((decimal)iWidth * 9525);
            iHeight = (long)Math.Round((decimal)iHeight * 9525);

            double maxWidthCm = 16.5;
            double maxHeightCm = 20.0;
            const int emusPerCm = 360000;
            long maxWidthEmus = (long)(maxWidthCm * emusPerCm);
            if (iWidth > maxWidthEmus)
            {
                var ratio = (iHeight * 1.0m) / iWidth;
                iWidth = maxWidthEmus;
                iHeight = (long)(iWidth * ratio);
            }

            long maxHeightEmus = (long)(maxHeightCm * emusPerCm);
            if (iHeight > maxHeightEmus)
            {
                var ratio = (iWidth * 1.0m) / iHeight;
                iHeight = maxHeightEmus;
                iWidth = (long)(iHeight * ratio);
            }

            imagePart.FeedData(new MemoryStream(bytes));

            if (!string.IsNullOrEmpty(values[1]))
            {
                if (paragraph == null)
                {
                    var rel = wordDoc.MainDocumentPart.AddHyperlinkRelationship(new Uri(values[1]), true);
                    SectionProperties sectPr =
                        (SectionProperties)wordDoc.MainDocumentPart.Document.Body.ChildElements.Last();

                    wordDoc.MainDocumentPart.Document.Body.InsertBefore(new Paragraph(
                            new Hyperlink(
                                    new Run(
                                        new RunProperties(
                                            new RunStyle { Val = "InternetLink" },
                                            new RunFonts { Ascii = "Arial", HighAnsi = "Arial" },
                                            new Color
                                            {
                                                Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF"
                                            },
                                            new Underline { Val = UnderlineValues.Single }
                                        ),
                                        new Text(values[1])
                                    )
                                )
                                { History = OnOffValue.FromBoolean(true), Id = rel.Id }
                        ),
                        sectPr
                    );
                }
                else
                {
                    var rel = wordDoc.MainDocumentPart.AddHyperlinkRelationship(new Uri(values[1]), true);
                    paragraph.Append(new Hyperlink(
                            new Run(
                                new RunProperties(
                                    new RunStyle { Val = "InternetLink" },
                                    new RunFonts { Ascii = "Arial", HighAnsi = "Arial" },
                                    new Color
                                    {
                                        Val = "365F91", ThemeColor = ThemeColorValues.Accent1, ThemeShade = "BF"
                                    },
                                    new Underline { Val = UnderlineValues.Single }
                                ),
                                new Text(values[1])
                            )
                        )
                        { History = OnOffValue.FromBoolean(true), Id = rel.Id }
                    );
                }
            }

            AddImageToBody(wordDoc, mainPart.GetIdOfPart(imagePart), iWidth, iHeight, paragraph);
        }

        public static void ValidateWordDocument(string filepath)
        {
            using (WordprocessingDocument wordprocessingDocument =
                   WordprocessingDocument.Open(filepath, true))
            {
                try
                {
                    OpenXmlValidator validator = new OpenXmlValidator();
                    int count = 0;
                    foreach (ValidationErrorInfo error in
                             validator.Validate(wordprocessingDocument))
                    {
                        count++;
                        Console.WriteLine("Error " + count);
                        Console.WriteLine("Description: " + error.Description);
                        Console.WriteLine("ErrorType: " + error.ErrorType);
                        Console.WriteLine("Id: " + error.Id);
                        Console.WriteLine("Node: " + error.Node);
                        // Console.WriteLine("Node InnerXML: " + error.Node.InnerXml);
                        // Console.WriteLine("Node InnerText: " + error.Node.InnerText);
                        Console.WriteLine("Path: " + error.Path.XPath);
                        Console.WriteLine("Part: " + error.Part.Uri);
                        Console.WriteLine("-------------------------------------------");
                    }

                    Console.WriteLine("count={0}", count);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                wordprocessingDocument.Dispose();
            }
        }

        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId, Int64Value cx,
            Int64Value cy, Paragraph paragraph = null)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "AddImageToBody called"));
            // Define the reference of the image.
            var element =
                new Drawing(
                    new DW.Inline(
                        new DW.Extent { Cx = cx, Cy = cy },
                        new DW.EffectExtent
                        {
                            LeftEdge = 0L, TopEdge = 0L,
                            RightEdge = 0L, BottomEdge = 0L
                        },
                        new DW.DocProperties
                        {
                            Id = (UInt32Value)1U,
                            Name = "Picture"
                        },
                        new DW.NonVisualGraphicFrameDrawingProperties(
                            new A.GraphicFrameLocks { NoChangeAspect = true }),
                        new A.Graphic(
                            new A.GraphicData(
                                new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties
                                        {
                                            Id = (UInt32Value)0U,
                                            Name = "New Bitmap Image.jpg"
                                        },
                                        new PIC.NonVisualPictureDrawingProperties()),
                                    new PIC.BlipFill(
                                        new A.Blip(
                                            new A.BlipExtensionList(
                                                new A.BlipExtension
                                                {
                                                    Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                })
                                        )
                                        {
                                            Embed = relationshipId,
                                            CompressionState =
                                                A.BlipCompressionValues.Print
                                        },
                                        new A.Stretch(
                                            new A.FillRectangle())),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset { X = 0L, Y = 0L },
                                            new A.Extents { Cx = 990000L, Cy = 792000L }),
                                        new A.PresetGeometry(
                                            new A.AdjustValueList()
                                        ) { Preset = A.ShapeTypeValues.Rectangle }))
                            ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                    )
                    {
                        DistanceFromTop = (UInt32Value)0U,
                        DistanceFromBottom = (UInt32Value)0U,
                        DistanceFromLeft = (UInt32Value)0U,
                        DistanceFromRight = (UInt32Value)0U, EditId = "50D07946"
                    });

            // Append the reference to body, the element should be in a Run.
            SectionProperties sectPr = (SectionProperties)wordDoc.MainDocumentPart.Document.Body.ChildElements.Last();

            if (paragraph != null)
            {
                paragraph.Append(new Run(element));
            }
            else
            {
                wordDoc.MainDocumentPart.Document.Body.InsertBefore(new Paragraph(new Run(element)), sectPr);
            }
        }

        private static void AddSignatureToParagraph(WordprocessingDocument wordDoc, string relationshipId,
            Int64Value cx, Int64Value cy, string tagToReplace)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "AddImageToBody called"));
            // Define the reference of the image.
            var element =
                new Drawing(
                    new DW.Inline(
                        new DW.Extent { Cx = cx, Cy = cy },
                        new DW.EffectExtent
                        {
                            LeftEdge = 0L, TopEdge = 0L,
                            RightEdge = 0L, BottomEdge = 0L
                        },
                        new DW.DocProperties
                        {
                            Id = (UInt32Value)1U,
                            Name = "Picture"
                        },
                        new DW.NonVisualGraphicFrameDrawingProperties(
                            new A.GraphicFrameLocks { NoChangeAspect = true }),
                        new A.Graphic(
                            new A.GraphicData(
                                new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties
                                        {
                                            Id = (UInt32Value)0U,
                                            Name = "New Bitmap Image.jpg"
                                        },
                                        new PIC.NonVisualPictureDrawingProperties()),
                                    new PIC.BlipFill(
                                        new A.Blip(
                                            new A.BlipExtensionList(
                                                new A.BlipExtension
                                                {
                                                    Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                })
                                        )
                                        {
                                            Embed = relationshipId,
                                            CompressionState =
                                                A.BlipCompressionValues.Print
                                        },
                                        new A.Stretch(
                                            new A.FillRectangle())),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset { X = 0L, Y = 0L },
                                            new A.Extents { Cx = 990000L, Cy = 792000L }),
                                        new A.PresetGeometry(
                                            new A.AdjustValueList()
                                        ) { Preset = A.ShapeTypeValues.Rectangle }))
                            ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                    )
                    {
                        DistanceFromTop = (UInt32Value)0U,
                        DistanceFromBottom = (UInt32Value)0U,
                        DistanceFromLeft = (UInt32Value)0U,
                        DistanceFromRight = (UInt32Value)0U, EditId = "50D07946"
                    });

            var tagNode = wordDoc.MainDocumentPart.Document.Body.Elements<Paragraph>()
                .FirstOrDefault(f => f.InnerText.Contains(tagToReplace));
            if (tagNode != null)
            {
                tagNode.InsertBeforeSelf(new Paragraph(new Run(element)));
                tagNode.Remove();
            }
        }

        private static void WriteDebugConsoleLogEntry(LogEntry logEntry)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[DBG] {logEntry.Type}: {logEntry.Message}");
            Console.ForegroundColor = oldColor;
        }

        private static void WriteErrorConsoleLogEntry(LogEntry logEntry)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERR] {logEntry.Type}: {logEntry.Message}");
            Console.ForegroundColor = oldColor;
        }
    }
}