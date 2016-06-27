using System;
using System.Collections.Generic;
using System.IO;
using iText.IO;
using iText.IO.Font;
using iText.IO.Image;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Wmf;
using iText.Kernel.Pdf.Colorspace;
using iText.Kernel.Pdf.Extgstate;
using iText.Kernel.Pdf.Function;
using iText.Kernel.Utils;
using iText.Test;
using iText.Test.Attributes;

namespace iText.Kernel.Pdf {
    public class PdfCanvasTest : ExtendedITextTest {
        public static readonly String sourceFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory + "/../../resources/itext/kernel/pdf/PdfCanvasTest/";

        public static readonly String destinationFolder = NUnit.Framework.TestContext.CurrentContext.TestDirectory
             + "/test/itext/kernel/pdf/PdfCanvasTest/";

        /// <summary>Paths to images.</summary>
        public static readonly String[] RESOURCES = new String[] { "Desert.jpg", "bulb.gif", "0047478.jpg", "itext.png"
             };

        [NUnit.Framework.TestFixtureSetUp]
        public static void BeforeClass() {
            CreateOrClearDestinationFolder(destinationFolder);
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void CreateSimpleCanvas() {
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(destinationFolder + "simpleCanvas.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.Rectangle(100, 100, 100, 100).Fill();
            canvas.Release();
            pdfDoc.Close();
            PdfReader reader = new PdfReader(destinationFolder + "simpleCanvas.pdf");
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(1, pdfDocument.GetNumberOfPages(), "Page count");
            PdfDictionary page = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void CreateSimpleCanvasWithDrawing() {
            String fileName = "simpleCanvasWithDrawing.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(destinationFolder + fileName, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.SaveState().SetLineWidth(30).MoveTo(36, 700).LineTo(300, 300).Stroke().RestoreState();
            canvas.SaveState().Rectangle(250, 500, 100, 100).Fill().RestoreState();
            canvas.SaveState().Circle(100, 400, 25).Fill().RestoreState();
            canvas.SaveState().RoundRectangle(100, 650, 100, 100, 10).Fill().RestoreState();
            canvas.SaveState().SetLineWidth(10).RoundRectangle(250, 650, 100, 100, 10).Stroke().RestoreState();
            canvas.SaveState().SetLineWidth(5).Arc(400, 650, 550, 750, 0, 180).Stroke().RestoreState();
            canvas.SaveState().SetLineWidth(5).MoveTo(400, 550).CurveTo(500, 570, 450, 450, 550, 550).Stroke().RestoreState
                ();
            canvas.Release();
            pdfDoc.Close();
            PdfReader reader = new PdfReader(destinationFolder + fileName);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(1, pdfDocument.GetNumberOfPages(), "Page count");
            PdfDictionary page = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void CreateSimpleCanvasWithText() {
            String fileName = "simpleCanvasWithText.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(destinationFolder + fileName, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            //Initialize canvas and write text to it
            canvas.SaveState().BeginText().MoveText(36, 750).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                ), 16).ShowText("Hello Helvetica!").EndText().RestoreState();
            canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA_BOLDOBLIQUE
                ), 16).ShowText("Hello Helvetica Bold Oblique!").EndText().RestoreState();
            canvas.SaveState().BeginText().MoveText(36, 650).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER
                ), 16).ShowText("Hello Courier!").EndText().RestoreState();
            canvas.SaveState().BeginText().MoveText(36, 600).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.TIMES_ITALIC
                ), 16).ShowText("Hello Times Italic!").EndText().RestoreState();
            canvas.SaveState().BeginText().MoveText(36, 550).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.SYMBOL
                ), 16).ShowText("Hello Ellada!").EndText().RestoreState();
            canvas.SaveState().BeginText().MoveText(36, 500).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.ZAPFDINGBATS
                ), 16).ShowText("Hello ZapfDingbats!").EndText().RestoreState();
            canvas.Release();
            pdfDoc.Close();
            PdfReader reader = new PdfReader(destinationFolder + fileName);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(1, pdfDocument.GetNumberOfPages(), "Page count");
            PdfDictionary page = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void CreateSimpleCanvasWithPageFlush() {
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(destinationFolder + "simpleCanvasWithPageFlush.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.Rectangle(100, 100, 100, 100).Fill();
            canvas.Release();
            page1.Flush();
            pdfDoc.Close();
            PdfReader reader = new PdfReader(destinationFolder + "simpleCanvasWithPageFlush.pdf");
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(1, pdfDocument.GetNumberOfPages(), "Page count");
            PdfDictionary page = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void CreateSimpleCanvasWithFullCompression() {
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(destinationFolder + "simpleCanvasWithFullCompression.pdf", FileMode.Create
                );
            PdfWriter writer = new PdfWriter(fos, new WriterProperties().SetFullCompressionMode(true));
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.Rectangle(100, 100, 100, 100).Fill();
            canvas.Release();
            pdfDoc.Close();
            PdfReader reader = new PdfReader(destinationFolder + "simpleCanvasWithFullCompression.pdf");
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(1, pdfDocument.GetNumberOfPages(), "Page count");
            PdfDictionary page = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void CreateSimpleCanvasWithPageFlushAndFullCompression() {
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(destinationFolder + "simpleCanvasWithPageFlushAndFullCompression.pdf", FileMode.Create
                );
            PdfWriter writer = new PdfWriter(fos, new WriterProperties().SetFullCompressionMode(true));
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            PdfPage page1 = pdfDoc.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.Rectangle(100, 100, 100, 100).Fill();
            canvas.Release();
            page1.Flush();
            pdfDoc.Close();
            PdfReader reader = new PdfReader(destinationFolder + "simpleCanvasWithPageFlushAndFullCompression.pdf");
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(1, pdfDocument.GetNumberOfPages(), "Page count");
            PdfDictionary page = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create1000PagesDocument() {
            int pageCount = 1000;
            String filename = destinationFolder + pageCount + "PagesDocument.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                    ), 72).ShowText(iText.IO.Util.JavaUtil.IntegerToString(i + 1)).EndText().RestoreState();
                canvas.Rectangle(100, 500, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(destinationFolder + "1000PagesDocument.pdf");
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create100PagesDocument() {
            int pageCount = 100;
            String filename = destinationFolder + pageCount + "PagesDocument.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                    ), 72).ShowText(iText.IO.Util.JavaUtil.IntegerToString(i + 1)).EndText().RestoreState();
                canvas.Rectangle(100, 500, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(filename);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create10PagesDocument() {
            int pageCount = 10;
            String filename = destinationFolder + pageCount + "PagesDocument.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                    ), 72).ShowText(iText.IO.Util.JavaUtil.IntegerToString(i + 1)).EndText().RestoreState();
                canvas.Rectangle(100, 500, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(filename);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create1000PagesDocumentWithText() {
            int pageCount = 1000;
            String filename = destinationFolder + "1000PagesDocumentWithText.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 650).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER
                    ), 16).ShowText("Page " + (i + 1)).EndText();
                canvas.Rectangle(100, 100, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(filename);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create1000PagesDocumentWithFullCompression() {
            int pageCount = 1000;
            String filename = destinationFolder + "1000PagesDocumentWithFullCompression.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos, new WriterProperties().SetFullCompressionMode(true));
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                    ), 72).ShowText(iText.IO.Util.JavaUtil.IntegerToString(i + 1)).EndText().RestoreState();
                canvas.Rectangle(100, 500, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(filename);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create100PagesDocumentWithFullCompression() {
            int pageCount = 100;
            String filename = destinationFolder + pageCount + "PagesDocumentWithFullCompression.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos, new WriterProperties().SetFullCompressionMode(true));
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                    ), 72).ShowText(iText.IO.Util.JavaUtil.IntegerToString(i + 1)).EndText().RestoreState();
                canvas.Rectangle(100, 500, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(filename);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create197PagesDocumentWithFullCompression() {
            int pageCount = 197;
            String filename = destinationFolder + pageCount + "PagesDocumentWithFullCompression.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos, new WriterProperties().SetFullCompressionMode(true));
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                    ), 72).ShowText(iText.IO.Util.JavaUtil.IntegerToString(i + 1)).EndText().RestoreState();
                canvas.Rectangle(100, 500, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(filename);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        [NUnit.Framework.Test]
        public virtual void Create10PagesDocumentWithFullCompression() {
            int pageCount = 10;
            String filename = destinationFolder + pageCount + "PagesDocumentWithFullCompression.pdf";
            String author = "Alexander Chingarev";
            String creator = "iText 6";
            String title = "Empty iText 6 Document";
            FileStream fos = new FileStream(filename, FileMode.Create);
            PdfWriter writer = new PdfWriter(fos, new WriterProperties().SetFullCompressionMode(true));
            PdfDocument pdfDoc = new PdfDocument(writer);
            pdfDoc.GetDocumentInfo().SetAuthor(author).SetCreator(creator).SetTitle(title);
            for (int i = 0; i < pageCount; i++) {
                PdfPage page = pdfDoc.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page);
                canvas.SaveState().BeginText().MoveText(36, 700).SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA
                    ), 72).ShowText(iText.IO.Util.JavaUtil.IntegerToString(i + 1)).EndText().RestoreState();
                canvas.Rectangle(100, 500, 100, 100).Fill();
                canvas.Release();
                page.Flush();
            }
            pdfDoc.Close();
            PdfReader reader = new PdfReader(filename);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary info = pdfDocument.GetDocumentInfo().GetPdfObject();
            NUnit.Framework.Assert.AreEqual(author, info.Get(PdfName.Author).ToString(), "Author");
            NUnit.Framework.Assert.AreEqual(creator, info.Get(PdfName.Creator).ToString(), "Creator");
            NUnit.Framework.Assert.AreEqual(title, info.Get(PdfName.Title).ToString(), "Title");
            NUnit.Framework.Assert.AreEqual(pageCount, pdfDocument.GetNumberOfPages(), "Page count");
            for (int i_1 = 1; i_1 <= pageCount; i_1++) {
                PdfDictionary page = pdfDocument.GetPage(i_1).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CopyPagesTest1() {
            String file1 = destinationFolder + "copyPages1_1.pdf";
            String file2 = destinationFolder + "copyPages1_2.pdf";
            PdfWriter writer1 = new PdfWriter(new FileStream(file1, FileMode.Create));
            PdfDocument pdfDoc1 = new PdfDocument(writer1);
            PdfPage page1 = pdfDoc1.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.Rectangle(100, 600, 100, 100);
            canvas.Fill();
            canvas.BeginText();
            canvas.SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER), 12);
            canvas.SetTextMatrix(1, 0, 0, 1, 100, 500);
            canvas.ShowText("Hello World!");
            canvas.EndText();
            canvas.Release();
            page1.Flush();
            pdfDoc1.Close();
            PdfReader reader1 = new PdfReader(new FileStream(file1, FileMode.Open, FileAccess.Read));
            pdfDoc1 = new PdfDocument(reader1);
            page1 = pdfDoc1.GetPage(1);
            FileStream fos2 = new FileStream(file2, FileMode.Create);
            PdfWriter writer2 = new PdfWriter(fos2);
            PdfDocument pdfDoc2 = new PdfDocument(writer2);
            PdfPage page2 = page1.CopyTo(pdfDoc2);
            pdfDoc2.AddPage(page2);
            page2.Flush();
            pdfDoc2.Close();
            PdfReader reader = new PdfReader(file2);
            PdfDocument pdfDocument = new PdfDocument(reader);
            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++) {
                PdfDictionary page = pdfDocument.GetPage(i).GetPdfObject();
                NUnit.Framework.Assert.AreEqual(PdfName.Page, page.Get(PdfName.Type));
            }
            reader.Close();
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary page_1 = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.IsNotNull(page_1.Get(PdfName.Parent));
            reader.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(file1, file2, destinationFolder, "diff_")
                );
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CopyPagesTest2() {
            String file1 = destinationFolder + "copyPages2_1.pdf";
            String file2 = destinationFolder + "copyPages2_2.pdf";
            PdfWriter writer1 = new PdfWriter(new FileStream(file1, FileMode.Create));
            PdfDocument pdfDoc1 = new PdfDocument(writer1);
            for (int i = 0; i < 10; i++) {
                PdfPage page1 = pdfDoc1.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page1);
                canvas.Rectangle(100, 600, 100, 100);
                canvas.Fill();
                canvas.BeginText();
                canvas.SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER), 12);
                canvas.SetTextMatrix(1, 0, 0, 1, 100, 500);
                canvas.ShowText(String.Format("Page_{0}", i + 1));
                canvas.EndText();
                canvas.Release();
                page1.Flush();
            }
            pdfDoc1.Close();
            pdfDoc1 = new PdfDocument(new PdfReader(new FileStream(file1, FileMode.Open, FileAccess.Read)));
            PdfWriter writer2 = new PdfWriter(new FileStream(file2, FileMode.Create));
            PdfDocument pdfDoc2 = new PdfDocument(writer2);
            for (int i_1 = 9; i_1 >= 0; i_1--) {
                PdfPage page2 = pdfDoc1.GetPage(i_1 + 1).CopyTo(pdfDoc2);
                pdfDoc2.AddPage(page2);
            }
            pdfDoc1.Close();
            pdfDoc2.Close();
            PdfReader reader = new PdfReader(file2);
            PdfDocument pdfDocument = new PdfDocument(reader);
            NUnit.Framework.Assert.AreEqual(false, reader.HasRebuiltXref(), "Rebuilt");
            PdfDictionary page = pdfDocument.GetPage(1).GetPdfObject();
            NUnit.Framework.Assert.IsNotNull(page.Get(PdfName.Parent));
            reader.Close();
            CompareTool cmpTool = new CompareTool();
            PdfDocument doc1 = new PdfDocument(new PdfReader(file1));
            PdfDocument doc2 = new PdfDocument(new PdfReader(file2));
            for (int i_2 = 0; i_2 < 10; i_2++) {
                PdfDictionary page1 = doc1.GetPage(i_2 + 1).GetPdfObject();
                PdfDictionary page2 = doc2.GetPage(10 - i_2).GetPdfObject();
                NUnit.Framework.Assert.IsTrue(cmpTool.CompareDictionaries(page1, page2));
            }
            doc1.Close();
            doc2.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CopyPagesTest3() {
            String file1 = destinationFolder + "copyPages3_1.pdf";
            String file2 = destinationFolder + "copyPages3_2.pdf";
            PdfWriter writer1 = new PdfWriter(new FileStream(file1, FileMode.Create));
            PdfDocument pdfDoc1 = new PdfDocument(writer1);
            PdfPage page1 = pdfDoc1.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.Rectangle(100, 600, 100, 100);
            canvas.Fill();
            canvas.BeginText();
            canvas.SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER), 12);
            canvas.SetTextMatrix(1, 0, 0, 1, 100, 500);
            canvas.ShowText("Hello World!!!");
            canvas.EndText();
            canvas.Release();
            page1.Flush();
            pdfDoc1.Close();
            pdfDoc1 = new PdfDocument(new PdfReader(new FileStream(file1, FileMode.Open, FileAccess.Read)));
            page1 = pdfDoc1.GetPage(1);
            PdfWriter writer2 = new PdfWriter(new FileStream(file2, FileMode.Create));
            PdfDocument pdfDoc2 = new PdfDocument(writer2);
            for (int i = 0; i < 10; i++) {
                PdfPage page2 = page1.CopyTo(pdfDoc2);
                pdfDoc2.AddPage(page2);
                if (i % 2 == 0) {
                    page2.Flush();
                }
            }
            pdfDoc1.Close();
            pdfDoc2.Close();
            CompareTool cmpTool = new CompareTool();
            PdfReader reader1 = new PdfReader(file1);
            PdfDocument doc1 = new PdfDocument(reader1);
            NUnit.Framework.Assert.AreEqual(false, reader1.HasRebuiltXref(), "Rebuilt");
            PdfDictionary p1 = doc1.GetPage(1).GetPdfObject();
            PdfReader reader2 = new PdfReader(file2);
            PdfDocument doc2 = new PdfDocument(reader2);
            NUnit.Framework.Assert.AreEqual(false, reader2.HasRebuiltXref(), "Rebuilt");
            for (int i_1 = 0; i_1 < 10; i_1++) {
                PdfDictionary p2 = doc2.GetPage(i_1 + 1).GetPdfObject();
                NUnit.Framework.Assert.IsTrue(cmpTool.CompareDictionaries(p1, p2));
            }
            doc1.Close();
            doc2.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CopyPagesTest4() {
            String file1 = destinationFolder + "copyPages4_1.pdf";
            FileStream fos1 = new FileStream(file1, FileMode.Create);
            PdfWriter writer1 = new PdfWriter(fos1);
            PdfDocument pdfDoc1 = new PdfDocument(writer1);
            for (int i = 0; i < 5; i++) {
                PdfPage page1 = pdfDoc1.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page1);
                canvas.Rectangle(100, 600, 100, 100);
                canvas.Fill();
                canvas.BeginText();
                canvas.SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER), 12);
                canvas.SetTextMatrix(1, 0, 0, 1, 100, 500);
                canvas.ShowText(String.Format("Page_{0}", i + 1));
                canvas.EndText();
                canvas.Release();
            }
            pdfDoc1.Close();
            pdfDoc1 = new PdfDocument(new PdfReader(new FileStream(file1, FileMode.Open, FileAccess.Read)));
            for (int i_1 = 0; i_1 < 5; i_1++) {
                FileStream fos2 = new FileStream(destinationFolder + String.Format("copyPages4_{0}.pdf", i_1 + 2), FileMode.Create
                    );
                PdfWriter writer2 = new PdfWriter(fos2);
                PdfDocument pdfDoc2 = new PdfDocument(writer2);
                PdfPage page2 = pdfDoc1.GetPage(i_1 + 1).CopyTo(pdfDoc2);
                pdfDoc2.AddPage(page2);
                pdfDoc2.Close();
            }
            pdfDoc1.Close();
            CompareTool cmpTool = new CompareTool();
            PdfReader reader1 = new PdfReader(file1);
            PdfDocument doc1 = new PdfDocument(reader1);
            NUnit.Framework.Assert.AreEqual(false, reader1.HasRebuiltXref(), "Rebuilt");
            for (int i_2 = 0; i_2 < 5; i_2++) {
                PdfDictionary page1 = doc1.GetPage(i_2 + 1).GetPdfObject();
                PdfDocument doc2 = new PdfDocument(new PdfReader(destinationFolder + String.Format("copyPages4_{0}.pdf", i_2
                     + 2)));
                PdfDictionary page = doc2.GetPage(1).GetPdfObject();
                NUnit.Framework.Assert.IsTrue(cmpTool.CompareDictionaries(page1, page));
                doc2.Close();
            }
            doc1.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CopyPagesTest5() {
            int documentCount = 3;
            for (int i = 0; i < documentCount; i++) {
                FileStream fos1 = new FileStream(destinationFolder + String.Format("copyPages5_{0}.pdf", i + 1), FileMode.Create
                    );
                PdfWriter writer1 = new PdfWriter(fos1);
                PdfDocument pdfDoc1 = new PdfDocument(writer1);
                PdfPage page1 = pdfDoc1.AddNewPage();
                PdfCanvas canvas = new PdfCanvas(page1);
                canvas.Rectangle(100, 600, 100, 100);
                canvas.Fill();
                canvas.BeginText();
                canvas.SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER), 12);
                canvas.SetTextMatrix(1, 0, 0, 1, 100, 500);
                canvas.ShowText(String.Format("Page_{0}", i + 1));
                canvas.EndText();
                canvas.Release();
                pdfDoc1.Close();
            }
            IList<PdfDocument> docs = new List<PdfDocument>();
            for (int i_1 = 0; i_1 < documentCount; i_1++) {
                FileStream fos1 = new FileStream(destinationFolder + String.Format("copyPages5_{0}.pdf", i_1 + 1), FileMode.Open
                    , FileAccess.Read);
                PdfDocument pdfDoc1 = new PdfDocument(new PdfReader(fos1));
                docs.Add(pdfDoc1);
            }
            FileStream fos2 = new FileStream(destinationFolder + "copyPages5_4.pdf", FileMode.Create);
            PdfWriter writer2 = new PdfWriter(fos2);
            PdfDocument pdfDoc2 = new PdfDocument(writer2);
            for (int i_2 = 0; i_2 < 3; i_2++) {
                pdfDoc2.AddPage(docs[i_2].GetPage(1).CopyTo(pdfDoc2));
            }
            pdfDoc2.Close();
            foreach (PdfDocument doc in docs) {
                doc.Close();
            }
            CompareTool cmpTool = new CompareTool();
            for (int i_3 = 0; i_3 < 3; i_3++) {
                PdfReader reader1 = new PdfReader(destinationFolder + String.Format("copyPages5_{0}.pdf", i_3 + 1));
                PdfDocument doc1 = new PdfDocument(reader1);
                NUnit.Framework.Assert.AreEqual(false, reader1.HasRebuiltXref(), "Rebuilt");
                PdfReader reader2 = new PdfReader(destinationFolder + "copyPages5_4.pdf");
                PdfDocument doc2 = new PdfDocument(reader2);
                NUnit.Framework.Assert.AreEqual(false, reader2.HasRebuiltXref(), "Rebuilt");
                PdfDictionary page1 = doc1.GetPage(1).GetPdfObject();
                PdfDictionary page2 = doc2.GetPage(i_3 + 1).GetPdfObject();
                NUnit.Framework.Assert.IsTrue(cmpTool.CompareDictionaries(page1, page2));
                doc1.Close();
                doc2.Close();
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void CopyPagesTest6() {
            String file1 = destinationFolder + "copyPages6_1.pdf";
            String file2 = destinationFolder + "copyPages6_2.pdf";
            String file3 = destinationFolder + "copyPages6_3.pdf";
            String file1_upd = destinationFolder + "copyPages6_1_upd.pdf";
            FileStream fos1 = new FileStream(file1, FileMode.Create);
            PdfWriter writer1 = new PdfWriter(fos1);
            PdfDocument pdfDoc1 = new PdfDocument(writer1);
            PdfPage page1 = pdfDoc1.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page1);
            canvas.Rectangle(100, 600, 100, 100);
            canvas.Fill();
            canvas.BeginText();
            canvas.SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.COURIER), 12);
            canvas.SetTextMatrix(1, 0, 0, 1, 100, 500);
            canvas.ShowText("Hello World!");
            canvas.EndText();
            canvas.Release();
            pdfDoc1.Close();
            pdfDoc1 = new PdfDocument(new PdfReader(new FileStream(file1, FileMode.Open, FileAccess.Read)));
            FileStream fos2 = new FileStream(file2, FileMode.Create);
            PdfWriter writer2 = new PdfWriter(fos2);
            PdfDocument pdfDoc2 = new PdfDocument(writer2);
            pdfDoc2.AddPage(pdfDoc1.GetPage(1).CopyTo(pdfDoc2));
            pdfDoc2.Close();
            pdfDoc2 = new PdfDocument(new PdfReader(new FileStream(file2, FileMode.Open, FileAccess.Read)));
            FileStream fos3 = new FileStream(file3, FileMode.Create);
            PdfWriter writer3 = new PdfWriter(fos3);
            PdfDocument pdfDoc3 = new PdfDocument(writer3);
            pdfDoc3.AddPage(pdfDoc2.GetPage(1).CopyTo(pdfDoc3));
            pdfDoc3.Close();
            pdfDoc3 = new PdfDocument(new PdfReader(new FileStream(file3, FileMode.Open, FileAccess.Read)));
            pdfDoc1.Close();
            pdfDoc1 = new PdfDocument(new PdfReader(new FileStream(file1, FileMode.Open, FileAccess.Read)), new PdfWriter
                (new FileStream(file1_upd, FileMode.Create)));
            pdfDoc1.AddPage(pdfDoc3.GetPage(1).CopyTo(pdfDoc1));
            pdfDoc1.Close();
            pdfDoc2.Close();
            pdfDoc3.Close();
            CompareTool cmpTool = new CompareTool();
            for (int i = 0; i < 3; i++) {
                PdfReader reader1 = new PdfReader(file1);
                PdfDocument doc1 = new PdfDocument(reader1);
                NUnit.Framework.Assert.AreEqual(false, reader1.HasRebuiltXref(), "Rebuilt");
                PdfReader reader2 = new PdfReader(file2);
                PdfDocument doc2 = new PdfDocument(reader2);
                NUnit.Framework.Assert.AreEqual(false, reader2.HasRebuiltXref(), "Rebuilt");
                PdfReader reader3 = new PdfReader(file3);
                PdfDocument doc3 = new PdfDocument(reader3);
                NUnit.Framework.Assert.AreEqual(false, reader3.HasRebuiltXref(), "Rebuilt");
                PdfReader reader4 = new PdfReader(file1_upd);
                PdfDocument doc4 = new PdfDocument(reader4);
                NUnit.Framework.Assert.AreEqual(false, reader4.HasRebuiltXref(), "Rebuilt");
                NUnit.Framework.Assert.IsTrue(cmpTool.CompareDictionaries(doc1.GetPage(1).GetPdfObject(), doc4.GetPage(2).
                    GetPdfObject()));
                NUnit.Framework.Assert.IsTrue(cmpTool.CompareDictionaries(doc4.GetPage(2).GetPdfObject(), doc2.GetPage(1).
                    GetPdfObject()));
                NUnit.Framework.Assert.IsTrue(cmpTool.CompareDictionaries(doc2.GetPage(1).GetPdfObject(), doc4.GetPage(1).
                    GetPdfObject()));
                doc1.Close();
                doc2.Close();
                doc3.Close();
                doc4.Close();
            }
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MarkedContentTest1() {
            String message = "";
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfWriter writer = new PdfWriter(baos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.BeginMarkedContent(new PdfName("Tag1"));
            canvas.EndMarkedContent();
            try {
                canvas.EndMarkedContent();
            }
            catch (PdfException e) {
                message = e.Message;
            }
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.AreEqual(PdfException.UnbalancedBeginEndMarkedContentOperators, message);
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void MarkedContentTest2() {
            FileStream fos = new FileStream(destinationFolder + "markedContentTest2.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            Dictionary<PdfName, PdfObject> tmpMap = new Dictionary<PdfName, PdfObject>();
            tmpMap[new PdfName("Tag")] = new PdfNumber(2);
            PdfDictionary tag2 = new PdfDictionary(tmpMap);
            tmpMap = new Dictionary<PdfName, PdfObject>();
            tmpMap[new PdfName("Tag")] = ((PdfNumber)new PdfNumber(3).MakeIndirect(document));
            PdfDictionary tag3 = new PdfDictionary(tmpMap);
            canvas.BeginMarkedContent(new PdfName("Tag1")).EndMarkedContent().BeginMarkedContent(new PdfName("Tag2"), 
                tag2).EndMarkedContent().BeginMarkedContent(new PdfName("Tag3"), (PdfDictionary)((PdfDictionary)tag3.MakeIndirect
                (document))).EndMarkedContent();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "markedContentTest2.pdf"
                , sourceFolder + "cmp_markedContentTest2.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void GraphicsStateTest1() {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfWriter writer = new PdfWriter(baos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.SetLineWidth(3);
            canvas.SaveState();
            canvas.SetLineWidth(5);
            NUnit.Framework.Assert.AreEqual(5, canvas.GetGraphicsState().GetLineWidth(), 0);
            canvas.RestoreState();
            NUnit.Framework.Assert.AreEqual(3, canvas.GetGraphicsState().GetLineWidth(), 0);
            PdfExtGState egs = new PdfExtGState();
            egs.GetPdfObject().Put(PdfName.LW, new PdfNumber(2));
            canvas.SetExtGState(egs);
            NUnit.Framework.Assert.AreEqual(2, canvas.GetGraphicsState().GetLineWidth(), 0);
            canvas.Release();
            document.Close();
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest01() {
            FileStream fos = new FileStream(destinationFolder + "colorTest01.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.SetFillColor(DeviceRgb.RED).Rectangle(50, 500, 50, 50).Fill();
            canvas.SetFillColor(DeviceRgb.GREEN).Rectangle(150, 500, 50, 50).Fill();
            canvas.SetFillColor(DeviceRgb.BLUE).Rectangle(250, 500, 50, 50).Fill();
            canvas.SetLineWidth(5);
            canvas.SetStrokeColor(DeviceCmyk.CYAN).Rectangle(50, 400, 50, 50).Stroke();
            canvas.SetStrokeColor(DeviceCmyk.MAGENTA).Rectangle(150, 400, 50, 50).Stroke();
            canvas.SetStrokeColor(DeviceCmyk.YELLOW).Rectangle(250, 400, 50, 50).Stroke();
            canvas.SetStrokeColor(DeviceCmyk.BLACK).Rectangle(350, 400, 50, 50).Stroke();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest01.pdf", sourceFolder
                 + "cmp_colorTest01.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest02() {
            FileStream fos = new FileStream(destinationFolder + "colorTest02.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            writer.SetCompressionLevel(CompressionConstants.NO_COMPRESSION);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            PdfDeviceCs.Rgb rgb = new PdfDeviceCs.Rgb();
            Color red = Color.MakeColor(rgb, new float[] { 1, 0, 0 });
            Color green = Color.MakeColor(rgb, new float[] { 0, 1, 0 });
            Color blue = Color.MakeColor(rgb, new float[] { 0, 0, 1 });
            PdfDeviceCs.Cmyk cmyk = new PdfDeviceCs.Cmyk();
            Color cyan = Color.MakeColor(cmyk, new float[] { 1, 0, 0, 0 });
            Color magenta = Color.MakeColor(cmyk, new float[] { 0, 1, 0, 0 });
            Color yellow = Color.MakeColor(cmyk, new float[] { 0, 0, 1, 0 });
            Color black = Color.MakeColor(cmyk, new float[] { 0, 0, 0, 1 });
            canvas.SetFillColor(red).Rectangle(50, 500, 50, 50).Fill();
            canvas.SetFillColor(green).Rectangle(150, 500, 50, 50).Fill();
            canvas.SetFillColor(blue).Rectangle(250, 500, 50, 50).Fill();
            canvas.SetLineWidth(5);
            canvas.SetStrokeColor(cyan).Rectangle(50, 400, 50, 50).Stroke();
            canvas.SetStrokeColor(magenta).Rectangle(150, 400, 50, 50).Stroke();
            canvas.SetStrokeColor(yellow).Rectangle(250, 400, 50, 50).Stroke();
            canvas.SetStrokeColor(black).Rectangle(350, 400, 50, 50).Stroke();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest02.pdf", sourceFolder
                 + "cmp_colorTest02.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest03() {
            FileStream fos = new FileStream(destinationFolder + "colorTest03.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            writer.SetCompressionLevel(CompressionConstants.NO_COMPRESSION);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            CalGray calGray1 = new CalGray(new float[] { 0.9505f, 1.0000f, 1.0890f }, 0.5f);
            canvas.SetFillColor(calGray1).Rectangle(50, 500, 50, 50).Fill();
            CalGray calGray2 = new CalGray(new float[] { 0.9505f, 1.0000f, 1.0890f }, null, 2.222f, 0.5f);
            canvas.SetFillColor(calGray2).Rectangle(150, 500, 50, 50).Fill();
            CalRgb calRgb = new CalRgb(new float[] { 0.9505f, 1.0000f, 1.0890f }, null, new float[] { 1.8000f, 1.8000f
                , 1.8000f }, new float[] { 0.4497f, 0.2446f, 0.0252f, 0.3163f, 0.6720f, 0.1412f, 0.1845f, 0.0833f, 0.9227f
                 }, new float[] { 1f, 0.5f, 0f });
            canvas.SetFillColor(calRgb).Rectangle(50, 400, 50, 50).Fill();
            Lab lab1 = new Lab(new float[] { 0.9505f, 1.0000f, 1.0890f }, null, new float[] { -128, 127, -128, 127 }, 
                new float[] { 1f, 0.5f, 0f });
            canvas.SetFillColor(lab1).Rectangle(50, 300, 50, 50).Fill();
            Lab lab2 = new Lab((PdfCieBasedCs.Lab)lab1.GetColorSpace(), new float[] { 0f, 0.5f, 0f });
            canvas.SetFillColor(lab2).Rectangle(150, 300, 50, 50).Fill();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest03.pdf", sourceFolder
                 + "cmp_colorTest03.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest04() {
            //Create document with 3 colored rectangles in memory.
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfWriter writer = new PdfWriter(baos);
            writer.SetCompressionLevel(CompressionConstants.NO_COMPRESSION);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            FileStream streamGray = new FileStream(sourceFolder + "BlackWhite.icc", FileMode.Open, FileAccess.Read);
            FileStream streamRgb = new FileStream(sourceFolder + "CIERGB.icc", FileMode.Open, FileAccess.Read);
            FileStream streamCmyk = new FileStream(sourceFolder + "USWebUncoated.icc", FileMode.Open, FileAccess.Read);
            IccBased gray = new IccBased(streamGray, new float[] { 0.5f });
            IccBased rgb = new IccBased(streamRgb, new float[] { 1.0f, 0.5f, 0f });
            IccBased cmyk = new IccBased(streamCmyk, new float[] { 1.0f, 0.5f, 0f, 0f });
            canvas.SetFillColor(gray).Rectangle(50, 500, 50, 50).Fill();
            canvas.SetFillColor(rgb).Rectangle(150, 500, 50, 50).Fill();
            canvas.SetFillColor(cmyk).Rectangle(250, 500, 50, 50).Fill();
            canvas.Release();
            document.Close();
            //Copies page from created document to new document.
            //This is not strictly necessary for ICC-based colors paces test, but this is an additional test for copy functionality.
            byte[] bytes = baos.ToArray();
            PdfReader reader = new PdfReader(new MemoryStream(bytes));
            document = new PdfDocument(reader);
            FileStream fos = new FileStream(destinationFolder + "colorTest04.pdf", FileMode.Create);
            writer = new PdfWriter(fos);
            PdfDocument newDocument = new PdfDocument(writer);
            newDocument.AddPage(document.GetPage(1).CopyTo(newDocument));
            newDocument.Close();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest04.pdf", sourceFolder
                 + "cmp_colorTest04.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest05() {
            FileStream fos = new FileStream(destinationFolder + "colorTest05.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            FileStream streamGray = new FileStream(sourceFolder + "BlackWhite.icc", FileMode.Open, FileAccess.Read);
            FileStream streamRgb = new FileStream(sourceFolder + "CIERGB.icc", FileMode.Open, FileAccess.Read);
            FileStream streamCmyk = new FileStream(sourceFolder + "USWebUncoated.icc", FileMode.Open, FileAccess.Read);
            PdfCieBasedCs.IccBased gray = (PdfCieBasedCs.IccBased)new IccBased(streamGray).GetColorSpace();
            PdfCieBasedCs.IccBased rgb = (PdfCieBasedCs.IccBased)new IccBased(streamRgb).GetColorSpace();
            PdfCieBasedCs.IccBased cmyk = (PdfCieBasedCs.IccBased)new IccBased(streamCmyk).GetColorSpace();
            PdfResources resources = page.GetResources();
            resources.SetDefaultGray(gray);
            resources.SetDefaultRgb(rgb);
            resources.SetDefaultCmyk(cmyk);
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.SetFillColorGray(0.5f).Rectangle(50, 500, 50, 50).Fill();
            canvas.SetFillColorRgb(1.0f, 0.5f, 0f).Rectangle(150, 500, 50, 50).Fill();
            canvas.SetFillColorCmyk(1.0f, 0.5f, 0f, 0f).Rectangle(250, 500, 50, 50).Fill();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest05.pdf", sourceFolder
                 + "cmp_colorTest05.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest06() {
            byte[] bytes = new byte[256 * 3];
            int k = 0;
            for (int i = 0; i < 256; i++) {
                bytes[k++] = (byte)i;
                bytes[k++] = (byte)i;
                bytes[k++] = (byte)i;
            }
            FileStream fos = new FileStream(destinationFolder + "colorTest06.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            writer.SetCompressionLevel(CompressionConstants.NO_COMPRESSION);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfSpecialCs.Indexed indexed = new PdfSpecialCs.Indexed(PdfName.DeviceRGB, 255, new PdfString(iText.IO.Util.JavaUtil.GetStringForBytes
                (bytes, "UTF-8")));
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.SetFillColor(new Indexed(indexed, 85)).Rectangle(50, 500, 50, 50).Fill();
            canvas.SetFillColor(new Indexed(indexed, 127)).Rectangle(150, 500, 50, 50).Fill();
            canvas.SetFillColor(new Indexed(indexed, 170)).Rectangle(250, 500, 50, 50).Fill();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest06.pdf", sourceFolder
                 + "cmp_colorTest06.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest07() {
            FileStream fos = new FileStream(destinationFolder + "colorTest07.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            writer.SetCompressionLevel(CompressionConstants.NO_COMPRESSION);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfFunction.Type4 function = new PdfFunction.Type4(new PdfArray(new float[] { 0, 1 }), new PdfArray(new float
                [] { 0, 1, 0, 1, 0, 1 }), "{0 0}".GetBytes());
            PdfSpecialCs.Separation separation = new PdfSpecialCs.Separation("MyRed", new PdfDeviceCs.Rgb(), function);
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.SetFillColor(new Separation(separation, 0.25f)).Rectangle(50, 500, 50, 50).Fill();
            canvas.SetFillColor(new Separation(separation, 0.5f)).Rectangle(150, 500, 50, 50).Fill();
            canvas.SetFillColor(new Separation(separation, 0.75f)).Rectangle(250, 500, 50, 50).Fill();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest07.pdf", sourceFolder
                 + "cmp_colorTest07.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void ColorTest08() {
            FileStream fos = new FileStream(destinationFolder + "colorTest08.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            writer.SetCompressionLevel(CompressionConstants.NO_COMPRESSION);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfFunction.Type4 function = new PdfFunction.Type4(new PdfArray(new float[] { 0, 1, 0, 1 }), new PdfArray(
                new float[] { 0, 1, 0, 1, 0, 1 }), "{0}".GetBytes());
            List<String> tmpArray = new List<String>(2);
            tmpArray.Add("MyRed");
            tmpArray.Add("MyGreen");
            PdfSpecialCs.DeviceN deviceN = new PdfSpecialCs.DeviceN(tmpArray, new PdfDeviceCs.Rgb(), function);
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.SetFillColor(new DeviceN(deviceN, new float[] { 0, 0 })).Rectangle(50, 500, 50, 50).Fill();
            canvas.SetFillColor(new DeviceN(deviceN, new float[] { 0, 1 })).Rectangle(150, 500, 50, 50).Fill();
            canvas.SetFillColor(new DeviceN(deviceN, new float[] { 1, 0 })).Rectangle(250, 500, 50, 50).Fill();
            canvas.Release();
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "colorTest08.pdf", sourceFolder
                 + "cmp_colorTest08.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WmfImageTest01() {
            FileStream fos = new FileStream(destinationFolder + "wmfImageTest01.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            ImageData img = new WmfImageData(sourceFolder + "example.wmf");
            canvas.AddImage(img, 0, 0, 0.1f, false);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "wmfImageTest01.pdf", 
                sourceFolder + "cmp_wmfImageTest01.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WmfImageTest02() {
            FileStream fos = new FileStream(destinationFolder + "wmfImageTest02.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            ImageData img = new WmfImageData(sourceFolder + "butterfly.wmf");
            canvas.AddImage(img, 0, 0, 1, false);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "wmfImageTest02.pdf", 
                sourceFolder + "cmp_wmfImageTest02.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WmfImageTest03() {
            FileStream fos = new FileStream(destinationFolder + "wmfImageTest03.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            ImageData img = new WmfImageData(sourceFolder + "type1.wmf");
            canvas.AddImage(img, 0, 0, 1, false);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "wmfImageTest03.pdf", 
                sourceFolder + "cmp_wmfImageTest03.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void WmfImageTest04() {
            FileStream fos = new FileStream(destinationFolder + "wmfImageTest04.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            ImageData img = new WmfImageData(sourceFolder + "type0.wmf");
            canvas.AddImage(img, 0, 0, 1, false);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "wmfImageTest04.pdf", 
                sourceFolder + "cmp_wmfImageTest04.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void GifImageTest01() {
            FileStream fos = new FileStream(destinationFolder + "gifImageTest01.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            ImageData img = ImageDataFactory.Create(sourceFolder + "2-frames.gif");
            canvas.AddImage(img, 100, 100, 200, false);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "gifImageTest01.pdf", 
                sourceFolder + "cmp_gifImageTest01.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void GifImageTest02() {
            FileStream fos = new FileStream(destinationFolder + "gifImageTest02.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            Stream @is = new FileStream(sourceFolder + "2-frames.gif", FileMode.Open, FileAccess.Read);
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            int reads = @is.Read();
            while (reads != -1) {
                baos.Write(reads);
                reads = @is.Read();
            }
            PdfCanvas canvas = new PdfCanvas(page);
            ImageData img = ImageDataFactory.CreateGifFrame(baos.ToArray(), 1);
            canvas.AddImage(img, 100, 100, 200, false);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "gifImageTest02.pdf", 
                sourceFolder + "cmp_gifImageTest02.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void GifImageTest03() {
            FileStream fos = new FileStream(destinationFolder + "gifImageTest03.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            Stream @is = new FileStream(sourceFolder + "2-frames.gif", FileMode.Open, FileAccess.Read);
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            int reads = @is.Read();
            while (reads != -1) {
                baos.Write(reads);
                reads = @is.Read();
            }
            PdfCanvas canvas = new PdfCanvas(page);
            ImageData img = ImageDataFactory.CreateGifFrame(baos.ToArray(), 2);
            canvas.AddImage(img, 100, 100, 200, false);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "gifImageTest03.pdf", 
                sourceFolder + "cmp_gifImageTest03.pdf", destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void GifImageTest04() {
            FileStream fos = new FileStream(destinationFolder + "gifImageTest04.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            Stream @is = new FileStream(sourceFolder + "2-frames.gif", FileMode.Open, FileAccess.Read);
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            int reads = @is.Read();
            while (reads != -1) {
                baos.Write(reads);
                reads = @is.Read();
            }
            PdfCanvas canvas = new PdfCanvas(page);
            try {
                ImageDataFactory.CreateGifFrame(baos.ToArray(), 3);
                NUnit.Framework.Assert.Fail("IOException expected");
            }
            catch (iText.IO.IOException) {
            }
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        public virtual void GifImageTest05() {
            FileStream fos = new FileStream(destinationFolder + "gifImageTest05.pdf", FileMode.Create);
            PdfWriter writer = new PdfWriter(fos);
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            Stream @is = new FileStream(sourceFolder + "animated_fox_dog.gif", FileMode.Open, FileAccess.Read);
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            int reads = @is.Read();
            while (reads != -1) {
                baos.Write(reads);
                reads = @is.Read();
            }
            PdfCanvas canvas = new PdfCanvas(page);
            IList<ImageData> frames = ImageDataFactory.CreateGifFrames(baos.ToArray(), new int[] { 1, 2, 5 });
            float y = 600;
            foreach (ImageData img in frames) {
                canvas.AddImage(img, 100, y, 200, false);
                y -= 200;
            }
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + "gifImageTest05.pdf", 
                sourceFolder + "cmp_gifImageTest05.pdf", destinationFolder, "diff_"));
        }

        //    @Test
        //    public void kernedTextTest01() throws IOException, InterruptedException {
        //        FileOutputStream fos = new FileOutputStream(destinationFolder + "kernedTextTest01.pdf");
        //        PdfWriter writer = new PdfWriter(fos);
        //        PdfDocument document = new PdfDocument(writer);
        //        PdfPage page = document.addNewPage();
        //
        //        PdfCanvas canvas = new PdfCanvas(page);
        //        String kernableText = "AVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAVAV";
        //        PdfFont font = PdfFont.createFont(document, FontConstants.HELVETICA);
        //        canvas.beginText().moveText(50, 600).setFontAndSize(font, 12).showText("Kerning:-" + kernableText).endText();
        //        canvas.beginText().moveText(50, 650).setFontAndSize(font, 12).showTextKerned("Kerning:+" + kernableText).endText();
        //
        //        document.close();
        //
        //        Assert.assertNull(new CompareTool().compareByContent(destinationFolder + "kernedTextTest01.pdf", sourceFolder + "cmp_kernedTextTest01.pdf", destinationFolder, "diff_"));
        //    }
        /*@Test
        public void ccittImageTest01() throws IOException, InterruptedException {
        String filename = "ccittImage01.pdf";
        PdfWriter writer = new PdfWriter(new FileOutputStream(destinationFolder + filename));
        PdfDocument document = new PdfDocument(writer);
        
        PdfPage page = document.addNewPage();
        PdfCanvas canvas = new PdfCanvas(page);
        
        String text = "Call me Ishmael. Some years ago--never mind how long "
        + "precisely --having little or no money in my purse, and nothing "
        + "particular to interest me on shore, I thought I would sail about "
        + "a little and see the watery part of the world.";
        
        BarcodePDF417 barcode = new BarcodePDF417();
        barcode.setText(text);
        barcode.paintCode();
        
        byte g4[] = CCITTG4Encoder.compress(barcode.getOutBits(), barcode.getBitColumns(), barcode.getCodeRows());
        RawImage img = (RawImage) ImageDataFactory.create(barcode.getBitColumns(), barcode.getCodeRows(), false, RawImage.CCITTG4, 0, g4, null);
        img.setTypeCcitt(RawImage.CCITTG4);
        canvas.addImage(img, 100, 100, false);
        
        document.close();
        
        Assert.assertNull(new CompareTool().compareByContent(destinationFolder + filename, sourceFolder + "cmp_" + filename, destinationFolder, "diff_"));
        }*/
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(LogMessageConstant.IMAGE_HAS_JBIG2DECODE_FILTER)]
        [LogMessage(LogMessageConstant.IMAGE_HAS_JPXDECODE_FILTER)]
        [LogMessage(LogMessageConstant.IMAGE_HAS_MASK)]
        [LogMessage(LogMessageConstant.IMAGE_SIZE_CANNOT_BE_MORE_4KB)]
        public virtual void InlineImagesTest01() {
            String filename = "inlineImages01.pdf";
            PdfWriter writer = new PdfWriter(new FileStream(destinationFolder + filename, FileMode.Create));
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            canvas.AddImage(ImageDataFactory.Create(sourceFolder + "Desert.jpg"), 36, 700, 100, true);
            canvas.AddImage(ImageDataFactory.Create(sourceFolder + "bulb.gif"), 36, 600, 100, true);
            canvas.AddImage(ImageDataFactory.Create(sourceFolder + "smpl.bmp"), 36, 500, 100, true);
            canvas.AddImage(ImageDataFactory.Create(sourceFolder + "itext.png"), 36, 460, 100, true);
            canvas.AddImage(ImageDataFactory.Create(sourceFolder + "0047478.jpg"), 36, 300, 100, true);
            canvas.AddImage(ImageDataFactory.Create(sourceFolder + "map.jp2"), 36, 200, 100, true);
            canvas.AddImage(ImageDataFactory.Create(sourceFolder + "amb.jb2"), 36, 30, 100, true);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder
                 + "cmp_" + filename, destinationFolder, "diff_"));
        }

        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.Exception"/>
        [NUnit.Framework.Test]
        [LogMessage(LogMessageConstant.IMAGE_HAS_JBIG2DECODE_FILTER)]
        [LogMessage(LogMessageConstant.IMAGE_HAS_JPXDECODE_FILTER)]
        [LogMessage(LogMessageConstant.IMAGE_HAS_MASK)]
        [LogMessage(LogMessageConstant.IMAGE_SIZE_CANNOT_BE_MORE_4KB)]
        public virtual void InlineImagesTest02() {
            String filename = "inlineImages02.pdf";
            PdfWriter writer = new PdfWriter(new FileStream(destinationFolder + filename, FileMode.Create));
            PdfDocument document = new PdfDocument(writer);
            PdfPage page = document.AddNewPage();
            PdfCanvas canvas = new PdfCanvas(page);
            Stream stream = UrlUtil.OpenStream(UrlUtil.ToURL(sourceFolder + "Desert.jpg"));
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            StreamUtil.TransferBytes(stream, baos);
            canvas.AddImage(ImageDataFactory.Create(baos.ToArray()), 36, 700, 100, true);
            stream = UrlUtil.OpenStream(UrlUtil.ToURL(sourceFolder + "bulb.gif"));
            baos = new ByteArrayOutputStream();
            StreamUtil.TransferBytes(stream, baos);
            canvas.AddImage(ImageDataFactory.Create(baos.ToArray()), 36, 600, 100, true);
            stream = UrlUtil.OpenStream(UrlUtil.ToURL(sourceFolder + "smpl.bmp"));
            baos = new ByteArrayOutputStream();
            StreamUtil.TransferBytes(stream, baos);
            canvas.AddImage(ImageDataFactory.Create(baos.ToArray()), 36, 500, 100, true);
            stream = UrlUtil.OpenStream(UrlUtil.ToURL(sourceFolder + "itext.png"));
            baos = new ByteArrayOutputStream();
            StreamUtil.TransferBytes(stream, baos);
            canvas.AddImage(ImageDataFactory.Create(baos.ToArray()), 36, 460, 100, true);
            stream = UrlUtil.OpenStream(UrlUtil.ToURL(sourceFolder + "0047478.jpg"));
            baos = new ByteArrayOutputStream();
            StreamUtil.TransferBytes(stream, baos);
            canvas.AddImage(ImageDataFactory.Create(baos.ToArray()), 36, 300, 100, true);
            stream = UrlUtil.OpenStream(UrlUtil.ToURL(sourceFolder + "map.jp2"));
            baos = new ByteArrayOutputStream();
            StreamUtil.TransferBytes(stream, baos);
            canvas.AddImage(ImageDataFactory.Create(baos.ToArray()), 36, 200, 100, true);
            stream = UrlUtil.OpenStream(UrlUtil.ToURL(sourceFolder + "amb.jb2"));
            baos = new ByteArrayOutputStream();
            StreamUtil.TransferBytes(stream, baos);
            canvas.AddImage(ImageDataFactory.Create(baos.ToArray()), 36, 30, 100, true);
            document.Close();
            NUnit.Framework.Assert.IsNull(new CompareTool().CompareByContent(destinationFolder + filename, sourceFolder
                 + "cmp_" + filename, destinationFolder, "diff_"));
        }
    }
}