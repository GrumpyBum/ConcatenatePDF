using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace PdfBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            AppInfo();

            while (true)
            {
                Console.WriteLine("1. Join 2 PDF Files");
                Console.WriteLine("2. Join Multiple PDF Files");
                Console.WriteLine("3. View Application Information");
                Console.WriteLine("4. Get Help or View Read Me");
                Console.WriteLine("5. Exit Application");
                Console.WriteLine();

                Console.Write("Menu Selection: ");
                String mainMenuInput = Console.ReadLine();
                int returnValue = 0;
                try
                {
                    int mainMenu = Int16.Parse(mainMenuInput);
                    returnValue = mainMenu;
                }
                catch
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Input!");
                    Console.WriteLine();
                }

                int exitApplication = 0;
                switch (returnValue)
                {
                    case 1:
                        DualRun();
                        break;
                    case 2:
                        Console.WriteLine("This Feature is NOT Currently Available (Sorry)");
                        // Console.WriteLine("Press and key to Exit");
                        Console.ReadLine();
                        break;
                    case 3:
                        AppInfo();
                        break;
                    case 5:
                        Console.WriteLine("Goodbye");
                        exitApplication++;
                        break;
                }
                if (exitApplication > 0)
                {
                    break;
                }
            }
        }

        private static void Concat2PDFs(string File1, string File2, string OutputFile)
        {
            string[] fileArray = new string[3];
            fileArray[0] = File1;
            fileArray[1] = File2;

            PdfReader reader = null;

            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(OutputFile, System.IO.FileMode.Create));

            //output file Open  
            sourceDocument.Open();


            //files list wise Loop  
            for (int g = 0; g < fileArray.Length - 1; g++)
            {
                int pages = TotalPageCount(fileArray[g]);

                reader = new PdfReader(fileArray[g]);
                //Add pages in new file  
                for (int i = 1; i <= pages; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }

                reader.Close();
            }
            //save the output file  
            sourceDocument.Close();
        }

        private static int TotalPageCount(string file)
        {
            using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }

        private static void DualRun()
        {
            String pdfOut = null;
            String pdfOne = null;
            String pdfTwo = null;

            while (true)
            {
                Console.WriteLine();
                if (String.IsNullOrEmpty(pdfOut))
                {
                    Console.WriteLine("Output File: Not Defined");
                }
                else
                {
                    Console.WriteLine("Output File: " + pdfOut);
                }

                Console.WriteLine("First Document: Not Defined");
                Console.WriteLine("Second Document: Not Defined");

                Console.WriteLine();
                Console.WriteLine("1. Define Output File");
                Console.WriteLine("2. Define First Document");
                Console.WriteLine("3. Define Second Document");
                Console.WriteLine("4. Build New PDF Document");
                Console.WriteLine("5. Get Help or View Read Me");
                Console.WriteLine("6. Exit Application");
                Console.WriteLine();

                int nextTask = 0;
                while (true)
                {
                    Console.Write("Menu Selection: ");
                    String newInput = Console.ReadLine();
                    int returnValue = 0;
                    try
                    {
                        int result = Int16.Parse(newInput);
                        returnValue = result;
                    }
                    catch
                    {
                        Console.WriteLine();
                        Console.WriteLine("Invalid Input!");
                        Console.WriteLine();
                    }
                    if (returnValue >= 1)
                    {
                        nextTask = returnValue;
                        break;
                    }
                }

                int exitApplication = 0;
                switch (nextTask)
                {
                    case 1:
                        Console.Write("Please Enter Output Path: ");
                        String dataInput = Console.ReadLine();
                        Console.WriteLine();

                        if (dataInput.EndsWith(".pdf"))
                        {
                            if (File.Exists(dataInput))
                            {
                                Console.WriteLine("This File Already Exists!");
                                Console.Write("Overwrite this File? (Y/N): ");
                                String overWrite = Console.ReadLine();
                                if ((overWrite.ToUpper()).StartsWith("Y") || (overWrite.ToUpper()).Equals("Y"))
                                {
                                    pdfOut = dataInput;
                                }
                                else if (overWrite.StartsWith("N") || (overWrite.ToUpper()).Equals("N"))
                                {
                                    Console.Write("Please Enter Output Path: ");
                                    dataInput = Console.ReadLine();
                                    if (File.Exists(dataInput))
                                    {
                                        Console.WriteLine("Error, Please Check Input!");
                                        break;
                                    }
                                    else
                                    {
                                        pdfOut = dataInput;
                                    }
                                }
                            }
                            else
                            {
                                pdfOut = dataInput;
                            }
                        }
                        else
                        {
                            Console.WriteLine("User Defined Output File is NOT a PDF, Please Reenter Data!");
                        }
                        
                        break;
                    case 4:
                        if (String.IsNullOrEmpty(pdfOut) || String.IsNullOrEmpty(pdfOne) || String.IsNullOrEmpty(pdfTwo)) {
                            Console.WriteLine("Data is Missing, Please Review! PDF Not Built!");
                        }
                        else
                        {
                            try
                            {
                                Concat2PDFs(pdfOne, pdfTwo, pdfOut);
                                if (File.Exists(pdfOut))
                                {
                                    Console.WriteLine("PDF File Created Successfully!");
                                }
                                else
                                {
                                    Console.WriteLine("PDF File Creation Failed!");
                                }
                                Console.WriteLine();
                                Console.Write("Exit Application? (Y/N) :");
                                String exitApp = Console.ReadLine();
                                if ((exitApp.ToUpper()).StartsWith("Y") || (exitApp.ToUpper()).Equals("Y"))
                                {
                                    exitApplication = 1;
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Error Occured, Please See Help Menu");
                            }
                        }
                        break;
                    case 6:
                        exitApplication = 1;
                        break;
                }
                if (exitApplication > 0)
                {
                    break;
                }
            }
        }

        private static void AppInfo()
        {
            Console.WriteLine("PDF Concatenate v0.1.0 Alpha - 27th August 2020 to 27th August 2020");
            Console.WriteLine("            Updated in C# for .NET Framework 4.8 with Visual Studio 2019 v16.7.2");
            Console.WriteLine("            Tasked to Merge PDF Documents into a Single PDF as Required");
            Console.WriteLine();
            Console.WriteLine("Built by GrumpyBum for Dad (With Love) - https://github.com/GrumpyBum");
            Console.WriteLine("With Credit to iTextSharp NuGet - https://github.com/schourode/iTextSharp-LGPL");
            // Please do not remove credits, to modify further please fork over build new. Thank you.
            // Console.WriteLine("With Credit, Forked from GrumpyBum - https://github.com/GrumpyBum");
            Console.WriteLine();
        }
    }
}
