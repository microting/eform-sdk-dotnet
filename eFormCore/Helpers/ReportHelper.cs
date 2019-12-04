using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using Paragraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace Microting.eForm.Helpers
{
    public static class ReportHelper
    {
        public static void SearchAndReplace(string fullPathToDocument, SortedDictionary<string, string> valuesToReplace, string outputFileName)
        {
            File.Copy(fullPathToDocument, outputFileName);
            
            WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputFileName, true);

            SearchAndReplaceHeaders(wordDoc, valuesToReplace);
            
            SearchAndReplaceBody(wordDoc, valuesToReplace);

            SearchAndReplaceFooters(wordDoc, valuesToReplace);

            wordDoc.Save();
            wordDoc.Close();
            wordDoc.Dispose();
        }

        private static void SearchAndReplaceHeaders(WordprocessingDocument wordDoc, 
            SortedDictionary<string, string> valuesToReplace)
        {
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
                if (fieldValue.Value != null)
                {  
                    Regex regexText = new Regex(fieldValue.Key);
                    docText = regexText.Replace(docText, fieldValue.Value);  
                }
                else
                {
                    Regex regexText = new Regex(fieldValue.Key);
                    docText = regexText.Replace(docText, "");  
                }
            }

            return docText;
        }
        
        public static void InsertSignature(string fullPathToDocument, List<KeyValuePair<string, string>> pictures)
        {
            WordprocessingDocument wordDoc = WordprocessingDocument.Open(fullPathToDocument, true);

            foreach (var keyValuePair in pictures)
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

                long iWidth = 0;
                long iHeight = 0;
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(keyValuePair.Value))
                {
                    iWidth = bmp.Width;
                    iHeight = bmp.Height;
                }            
                
                iWidth = (long)Math.Round((decimal)iWidth * 9525);
                iHeight = (long)Math.Round((decimal)iHeight * 9525);
                
                double maxWidthCm = 16.5;
                const int emusPerCm = 360000;
                long maxWidthEmus = (long)(maxWidthCm * emusPerCm);
                if (iWidth > maxWidthEmus) {
                    var ratio = (iHeight * 1.0m) / iWidth;
                    iWidth = maxWidthEmus;
                    iHeight = (long)(iWidth * ratio);
                }
                

                using (FileStream stream = new FileStream(keyValuePair.Value, FileMode.Open)) {
                    imagePart.FeedData(stream);
                }

                AddSignatureToParagraph(wordDoc, mainPart.GetIdOfPart(imagePart), iWidth, iHeight, keyValuePair.Key);
            }
            
            wordDoc.Save();
            wordDoc.Close();
            wordDoc.Dispose();
        }

        public static void InsertImages(string fullPathToDocument, List<KeyValuePair<string, List<string>>> pictures)
        {
            WordprocessingDocument wordDoc = WordprocessingDocument.Open(fullPathToDocument, true);
            string currentHeader = "";

            foreach (var keyValuePair in pictures)
            {
                if (currentHeader != keyValuePair.Key)
                {
                    if (currentHeader != "")
                    {
                        Body body = wordDoc.MainDocumentPart.Document.Body;

                        Paragraph para = body.AppendChild(new Paragraph());
                        Run run = para.AppendChild(new Run());
                        Break pageBreak = run.AppendChild(new Break());
                        pageBreak.Type = BreakValues.Page;
                    }
                    InsertHeader(keyValuePair.Key, wordDoc, currentHeader);
                }
                
                currentHeader = keyValuePair.Key;
                InsertPicture(keyValuePair.Value, wordDoc);

            }
            
            wordDoc.Save();
            wordDoc.Close();
            wordDoc.Dispose();
        }

        public static void ConvertToPdf(string docxFileName, string outputFolder)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "ConvertToPdf called"));
            try
            {
                using (Process pdfProcess = new Process())
                {
                    pdfProcess.StartInfo.UseShellExecute = false;
                    pdfProcess.StartInfo.RedirectStandardOutput = true;
                    pdfProcess.StartInfo.FileName = "soffice";
                    pdfProcess.StartInfo.Arguments = $" --headless --convert-to pdf {docxFileName} --outdir {outputFolder}";
                    pdfProcess.Start();
                    string output = pdfProcess.StandardOutput.ReadToEnd();
                    Trace.WriteLine(output);
                    pdfProcess.WaitForExit();
                    
                }
                
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
            if (header != currentHeader)
            {
                currentHeader = header;
                Body body = wordDoc.MainDocumentPart.Document.Body;

                Paragraph para = body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text(currentHeader));
                RunProperties runProperties = new RunProperties();
                runProperties.Append(new Bold());
                runProperties.Append(new RunFonts { Ascii = "Tahoma"});
                run.RunProperties = runProperties;

            }
        }

        public static void InsertPicture(List<string> values, WordprocessingDocument wordDoc)
        {
           
            MainDocumentPart mainPart = wordDoc.MainDocumentPart;
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

            long iWidth = 0;
            long iHeight = 0;
            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(values[0]))
            {
                iWidth = bmp.Width;
                iHeight = bmp.Height;
            }            
            
            iWidth = (long)Math.Round((decimal)iWidth * 9525);
            iHeight = (long)Math.Round((decimal)iHeight * 9525);
            
            double maxWidthCm = 16.5;
            double maxHeightCm = 20.0;
            const int emusPerCm = 360000;
            long maxWidthEmus = (long)(maxWidthCm * emusPerCm);
            if (iWidth > maxWidthEmus) {
                var ratio = (iHeight * 1.0m) / iWidth;
                iWidth = maxWidthEmus;
                iHeight = (long)(iWidth * ratio);
            }

            long maxHeightEmus = (long) (maxHeightCm * emusPerCm);
            if (iHeight > maxHeightEmus)
            {
                var ratio = (iWidth * 1.0m) / iHeight;
                iHeight = maxHeightEmus;
                iWidth = (long) (iHeight * ratio);
            }
            

            using (FileStream stream = new FileStream(values[0], FileMode.Open)) {
                imagePart.FeedData(stream);
            }

            if (!string.IsNullOrEmpty(values[1]))
            {
                var rel = wordDoc.MainDocumentPart.AddHyperlinkRelationship(new Uri(values[1]), true);
            
                wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(
                        new Hyperlink(
                                new Run(
                                    new RunProperties(
                                        new RunStyle() { Val = "Hyperlink"},
                                        new Underline() { Val = UnderlineValues.Single},
                                        new Color() { ThemeColor = ThemeColorValues.Hyperlink}
                                    ),
                                    new Text(values[1])
                                )
                            )
                            {History = OnOffValue.FromBoolean(true), Id = rel.Id}
                    )
                );
            }
            
            AddImageToBody(wordDoc, mainPart.GetIdOfPart(imagePart), iWidth, iHeight);
        }

        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId, Int64Value cx, Int64Value cy)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "AddImageToBody called"));
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = cx, Cy = cy },
                         new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, 
                             RightEdge = 0L, BottomEdge = 0L },
                         new DW.DocProperties() { Id = (UInt32Value)1U, 
                             Name = "Picture" },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties() 
                                            { Id = (UInt32Value)0U, 
                                                Name = "New Bitmap Image.jpg" },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension() 
                                                    { Uri = 
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}" })
                                         ) 
                                         { Embed = relationshipId, 
                                             CompressionState = 
                                             A.BlipCompressionValues.Print },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         ) { Preset = A.ShapeTypeValues.Rectangle }))
                             ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     ) { DistanceFromTop = (UInt32Value)0U, 
                         DistanceFromBottom = (UInt32Value)0U, 
                         DistanceFromLeft = (UInt32Value)0U, 
                         DistanceFromRight = (UInt32Value)0U, EditId = "50D07946" });
            
            // Append the reference to body, the element should be in a Run.
           wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        private static void AddSignatureToParagraph(WordprocessingDocument wordDoc, string relationshipId,
            Int64Value cx, Int64Value cy, string tagToReplace)
        {
            WriteDebugConsoleLogEntry(new LogEntry(2, "ReportHelper", "AddImageToBody called"));
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = cx, Cy = cy },
                         new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, 
                             RightEdge = 0L, BottomEdge = 0L },
                         new DW.DocProperties() { Id = (UInt32Value)1U, 
                             Name = "Picture" },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties() 
                                            { Id = (UInt32Value)0U, 
                                                Name = "New Bitmap Image.jpg" },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension() 
                                                    { Uri = 
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}" })
                                         ) 
                                         { Embed = relationshipId, 
                                             CompressionState = 
                                             A.BlipCompressionValues.Print },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         ) { Preset = A.ShapeTypeValues.Rectangle }))
                             ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     ) { DistanceFromTop = (UInt32Value)0U, 
                         DistanceFromBottom = (UInt32Value)0U, 
                         DistanceFromLeft = (UInt32Value)0U, 
                         DistanceFromRight = (UInt32Value)0U, EditId = "50D07946" });
            
            var tagNode = wordDoc.MainDocumentPart.Document.Body.Elements<Paragraph>().FirstOrDefault(f => f.InnerText.Contains(tagToReplace));
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