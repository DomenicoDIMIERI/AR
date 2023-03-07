using java.awt;
using java.awt.image;
using java.io;
using javax.imageio;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.pdmodel.common;
using org.apache.pdfbox.pdmodel.edit;
using org.apache.pdfbox.pdmodel.graphics.xobject;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PDFUtils1
{

    public static System.Drawing.Image GetImageFromPDF(String fileName, int pageNum)
    {
        //Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
        //pdf.LoadFromFile(fileName);
        //System.Drawing.Image ret = pdf.SaveAsImage(page, 300, 300);
        //pdf.Dispose();
        //return ret;

        Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument(fileName);
        System.Drawing.Image img  = pdf.SaveAsImage (pageNum , 300, 300);
        pdf.Close();
        return img;
    }

    public static BufferedImage GetImageFromPDF(PDDocument doc, int pageNum)
    {
        java.util.List pages = doc.getDocumentCatalog().getAllPages();
        PDPage page = (PDPage) pages.get(pageNum);
        BufferedImage image = page.convertToImage();
        doc.close();
        return image;
    }

    public static String pdfFromImage(BufferedImage bimg)
    {
        String f1 = System.IO.Path.GetTempFileName() + ".pdf"; 

        float width = bimg.getWidth();
        float height = bimg.getHeight();

        PDDocument document = new PDDocument();
        PDPage page = new PDPage(new PDRectangle(width, height));
        document.addPage(page);

        PDJpeg pimg = new PDJpeg(document, bimg); // new FileInputStream(img1));
        PDPageContentStream contentStream = new PDPageContentStream(document, page);
        contentStream.drawImage(pimg, 0, 0);
        contentStream.close();
       
        document.save(f1);
        document.close();

        return f1;
    }

    public static BufferedImage cloneImage(BufferedImage bi)
    {
        ColorModel cm = bi.getColorModel();
        bool isAlphaPremultiplied = cm.isAlphaPremultiplied();
        WritableRaster raster = bi.copyData(null);
        return new BufferedImage(cm, raster, isAlphaPremultiplied, null);
    }

    public static String[] SplitInto2Images(BufferedImage img) {
        int w = (int) img.getWidth();
        int h = (int) (img.getHeight() / 2);

        BufferedImage img1 = cloneImage(img);
        
        Graphics2D graph = img.createGraphics();
        graph.setColor(Color.BLACK);
        graph.fill(new Rectangle(0, h, w, h));
        graph.dispose();


        BufferedImage img2 = cloneImage(img);
        graph = img.createGraphics();
        graph.setColor(Color.BLACK);
        graph.fill(new Rectangle(0, 0, w, h));
        graph.dispose();

        String[] ret = new string[] { pdfFromImage(img1), pdfFromImage(img2) };

        //img1.flush();
        //img2.flush();

        return ret;
    }

    public static String[] save2Jpeg(System.Drawing.Image img)
    {
        int w = (int)img.Width;
        int h = (int)(img.Height / 2);

        System.Drawing.Image img1 = (System.Drawing.Image) img.Clone();
        System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(img1);
        g.FillRectangle(System.Drawing.Brushes .Yellow , 0, h, w, h);
        g.Dispose();
        String f1 = System.IO.Path.GetTempFileName(); // + ".jpg";
        img1.Save(f1, System.Drawing.Imaging.ImageFormat.Jpeg);
        img1.Dispose();

        img1 = (System.Drawing.Image)img.Clone();
        g = System.Drawing.Graphics.FromImage(img1);
        g.FillRectangle(System.Drawing.Brushes.Yellow, 0, 0, w, h);
        g.Dispose();
        String f2 = System.IO.Path.GetTempFileName(); // + ".jpg";
        img1.Save(f2, System.Drawing.Imaging.ImageFormat.Jpeg);
        img1.Dispose();

        String[] ret = new string[] { f1, f2 };

        return ret;
    }

    public static String imageToPDF(String jName, String pName)
    {
        //FileInputStream ins = new FileInputStream(jName);
        //BufferedImage bimg = ImageIO.read(ins);
        //float width = bimg.getWidth();
        //float height = bimg.getHeight();

        //PDDocument document = new PDDocument();
        //PDPage page = new PDPage(new PDRectangle(width, height));
        //document.addPage(page);

        //PDJpeg pimg = new PDJpeg(document, bimg); // new FileInputStream(img1));
        //PDPageContentStream contentStream = new PDPageContentStream(document, page);
        //contentStream.drawImage(pimg, 0, 0);
        //contentStream.close();

        //document.save(pName);
        //document.close();

        //return pName;

        PdfImage image = PdfImage.FromFile(jName);
        float width = image.Width * 0.75f;
        float height = image.Height * 0.75f;

        PdfDocument doc = new PdfDocument();
        PdfPageBase page = doc.Pages.Add();

        float fx = (page.Canvas.ClientSize.Width / image.Width);
        float fy = (page.Canvas.ClientSize.Height  / image.Height);
        float f = (fx < fy) ? fx : fy;

        page.Canvas.DrawImage(image, 0, 0, page.Canvas.ClientSize.Width, page.Canvas.ClientSize.Height);

        doc.SaveToFile(pName );
        doc.Close();

        return pName;
    }

    public static String[] get2PDF(String[] jNames)
    {
        String p1 = System.IO.Path.GetTempFileName(); // + ".pdf";
        String p2 = System.IO.Path.GetTempFileName(); // + ".pdf";
        return new String[] { imageToPDF (jNames[0], p1), imageToPDF(jNames[1], p2) };
    }


    public static String[] SplitInto2PDF(String pdfName, int pageNum)
    {
        System.Drawing.Image img = GetImageFromPDF(pdfName, pageNum);
        String[] jNames = save2Jpeg(img);
        String[] pNames = get2PDF(jNames);
        DeleteFile(jNames[0]);
        DeleteFile(jNames[1]);
        return pNames;
    }

    public static void DeleteFile(String fName)
    {
        try
        {
            System.IO.File.Delete(fName);
        } catch (Exception ex)
        {
            return;
        }
    }

}
