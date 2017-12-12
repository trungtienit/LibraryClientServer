using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Server;
using Server.Common;
using Spire.Pdf;

namespace Server.Common
{
    public class FileUtils
    {

        public String ConvertPdfPreview(string path)
        {
            string output = "";
            string fileFormat = path.Substring(path.LastIndexOf(".") + 1);
            if (fileFormat.ToUpper().Equals("DOC") || fileFormat.ToUpper().Equals("DOCX"))
                output = GetWordForPreview(path);
            if (fileFormat.ToUpper().Equals("PDF"))
                output = Split(path);
            return output;
        }

        public String GetWordForPreview(string path)
        {
            String nameFile = path.Substring(path.LastIndexOf("\\") + 1);
             nameFile = nameFile.Substring(0, nameFile.LastIndexOf("."));
            String output = DataBase.PATH_CACHE + "\\" + nameFile + ".pdf";
            if (File.Exists(output))
                return output;
            Spire.Doc.Document document = new Spire.Doc.Document();

            document.LoadFromFile(path);

            document.SaveToFile(output, Spire.Doc.FileFormat.PDF);

            System.Diagnostics.Process.Start(output);
            return output;
        }

        public string Split(string pdfFilePath)
        {
            if (!System.IO.Directory.Exists(DataBase.PATH_CACHE))
                System.IO.Directory.CreateDirectory(DataBase.PATH_CACHE);

            string outputPath = DataBase.PATH_CACHE;

            // Intialize a new PdfReader instance with the contents of the source Pdf file:  
            PdfReader reader = new PdfReader(pdfFilePath);

            int interval = 0;

            if (reader.NumberOfPages < 2) interval = 1;
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
            string pdfFileName = file.Name.Substring(0, file.Name.LastIndexOf("."));

            int pageNumber = 1;

            string newPdfFileName = string.Format(pdfFileName);

            return SplitAndSaveInterval(pdfFilePath, outputPath, pageNumber, interval, newPdfFileName);

        }

        public String SplitAndSaveInterval(string pdfFilePath, string outputPath, int startPage, int interval, string pdfFileName)
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
                    for (int pagenumber = startPage; pagenumber <= (startPage + interval); pagenumber++)
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
                    Console.WriteLine("File not opened " + E.StackTrace);
                }

            }
            return fileName;
        }
    }
}
