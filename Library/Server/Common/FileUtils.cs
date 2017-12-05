using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Client;
using Client.Common;

namespace Server.Common
{
    public class FileUtils
    {

        public static string Split(string pdfFilePath)
        {
            if (!System.IO.Directory.Exists(DataBase.PATH_CACHE))
                System.IO.Directory.CreateDirectory(DataBase.PATH_CACHE);

            string outputPath = DataBase.PATH_CACHE;

            // Intialize a new PdfReader instance with the contents of the source Pdf file:  
            PdfReader reader = new PdfReader(pdfFilePath);

            int interval = 0;

            if (reader.NumberOfPages < 2) interval = 0;
            else if (reader.NumberOfPages < 10) interval = reader.NumberOfPages / 3;
            else interval = 5;
           
            FileInfo file = null;
            try
            {
                file = new FileInfo(pdfFilePath);
            }
            catch (Exception)
            {
                Console.WriteLine("File not exist");
                return "";
            }
            string pdfFileName = file.Name.Substring(0, file.Name.LastIndexOf(".")) + "-";

            int pageNumber = 1;

            string newPdfFileName = string.Format(pdfFileName + ServerManager.PREVIEW);


            return SplitAndSaveInterval(pdfFilePath, outputPath, pageNumber, interval, newPdfFileName);

        }

        public static String SplitAndSaveInterval(string pdfFilePath, string outputPath, int startPage, int interval, string pdfFileName)
        {
            String fileName = outputPath + "\\" + pdfFileName + ".pdf";
            if (File.Exists(fileName))
                return fileName;
            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                PdfReader.unethicalreading = true;
                Document document = new Document();
                PdfCopy copy = new PdfCopy(document, new FileStream(fileName, FileMode.Create));
                document.Open();
                try
                {
                    for (int pagenumber = startPage; pagenumber < (startPage + interval); pagenumber++)
                    {
                        if (reader.NumberOfPages >= pagenumber)
                        {
                            copy.AddPage(copy.GetImportedPage(reader, pagenumber));
                        }
                        else
                        {
                            break;
                        }
                    }
                    document.Close();
                }
                catch (Exception E)
                {
                    Console.WriteLine("File not opened "+E.StackTrace);
                }

            }
            return fileName;
        }
    }
}
