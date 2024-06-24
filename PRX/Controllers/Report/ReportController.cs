using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using PRX.Models.Haghighi;
using static System.Net.Mime.MediaTypeNames;
using PRX.Data;
using Xceed.Words.NET;

namespace PRX.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Reports")]
    public class ReportController : ControllerBase
    {
        private readonly PRXDbContext _context;

        public ReportController(PRXDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GenerateReport(int id)
        {
            var userProfile = _context.HaghighiUserProfiles.Find(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            var relationships = _context.HaghighiUserRelationships
              .Where(r => r.RequestId == userProfile.RequestId && !r.IsDeleted)
              .ToList();

            //var financialProfiles = _context.HaghighiUserProfiles.Find(1);

            var financialProfiles = _context.HaghighiUserFinancialProfiles
                .Where(fp => fp.RequestId == userProfile.RequestId && !fp.IsDeleted)
                .ToList();

            //var templatePath = "D:\\MMNazari\\Documents\\Novin\\PRX Docs\\فرم_كسب_اطلاعات_مشتريان_و_پرسشنامه_ارزيابي_توان_مشتري_حقیقی.docx";
            //var outputPath = $"output-{id}.docx";

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "D:\\MMNazari\\Documents\\Novin\\PRX Docs\\فرم_كسب_اطلاعات_مشتريان_و_پرسشنامه_ارزيابي_توان_مشتري_حقیقی.docx");
            var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDirectory);  // Ensure the output directory exists
            var outputPath = Path.Combine(outputDirectory, $"output-{id}.docx");

            // Read the template into a memory stream
            using (var memoryStream = new MemoryStream())
            {
                using (var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
                {
                    templateStream.CopyTo(memoryStream);
                }

                memoryStream.Position = 0; // Reset stream position

                // Open the document from the memory stream
                using (var wordDoc = WordprocessingDocument.Open(memoryStream, true))
                {
                    var body = wordDoc.MainDocumentPart.Document.Body;

                    ReplacePlaceholder(body, "نام:", userProfile.FirstName);
                    ReplacePlaceholder(body, "نام خانوادگی:", userProfile.LastName);
                    ReplacePlaceholder(body, "نام پدر:", userProfile.FathersName);
                    ReplacePlaceholder(body, "کدملی:", userProfile.NationalNumber);
                    ReplacePlaceholder(body, "تاریخ تولد:", userProfile.BirthDate.ToString("yyyy-MM-dd"));
                    ReplacePlaceholder(body, "محل صدور:", userProfile.BirthPlace);
                    ReplacePlaceholder(body, "شماره شناسنامه:", userProfile.BirthCertificateNumber);
                    ReplacePlaceholder(body, "وضعیت تأهل:", userProfile.MaritalStatus);
                    ReplacePlaceholder(body, "جنسیت:", userProfile.Gender);
                    ReplacePlaceholder(body, "کدپستی:", userProfile.PostalCode.ToString());
                    ReplacePlaceholder(body, "تلفن همراه:", userProfile.HomePhone);
                    ReplacePlaceholder(body, "تلفن منزل:", userProfile.HomePhone);  // Assuming HomePhone for "تلفن منزل"
                    ReplacePlaceholder(body, "دورنگار:", userProfile.Fax);
                    ReplacePlaceholder(body, "بهترین زمان برای تماس تلفنی:", userProfile.BestTimeToCall);
                    ReplacePlaceholder(body, "نشانی محل سکونت:", userProfile.ResidentialAddress);
                    ReplacePlaceholder(body, "پست الکترونیک:", userProfile.Email);

                    // Fill the relationships table
                    //FillRelationshipsTable(body, relationships);

                    //FillFinancialProfileTable(body, financialProfiles);

                    wordDoc.MainDocumentPart.Document.Save();


                }

                // Save the modified document to the output stream
                memoryStream.Position = 0; // Reset stream position before saving to output stream
                using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    memoryStream.CopyTo(outputStream);
                }
            }

            //using (var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            //using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
            //{
            //    using (var wordDoc = WordprocessingDocument.Open(templateStream, false))
            //    using (var outputDoc = WordprocessingDocument.Create(outputStream, wordDoc.DocumentType))
            //    {
            //        // Copy the template parts into the new document
            //        foreach (var part in wordDoc.Parts)
            //        {
            //            outputDoc.AddPart(part.OpenXmlPart, part.RelationshipId);
            //        }

            //        var body = outputDoc.MainDocumentPart.Document.Body;

            //        ReplacePlaceholder(body, "نام:", userProfile.FirstName);
            //        ReplacePlaceholder(body, "نام خانوادگی:", userProfile.LastName);
            //        ReplacePlaceholder(body, "نام پدر:", userProfile.FathersName);
            //        ReplacePlaceholder(body, "کدملی:", userProfile.NationalNumber);
            //        ReplacePlaceholder(body, "تاریخ تولد:", userProfile.BirthDate.ToString("yyyy-MM-dd"));
            //        ReplacePlaceholder(body, "محل صدور:", userProfile.BirthPlace);
            //        ReplacePlaceholder(body, "شماره شناسنامه:", userProfile.BirthCertificateNumber);
            //        ReplacePlaceholder(body, "وضعیت تأهل: مجرد متأهل", userProfile.MaritalStatus);
            //        ReplacePlaceholder(body, "جنسیت: مرد زن", userProfile.Gender);
            //        ReplacePlaceholder(body, "کدپستی:", userProfile.PostalCode.ToString());
            //        ReplacePlaceholder(body, "تلفن همراه:", userProfile.HomePhone);
            //        ReplacePlaceholder(body, "تلفن منزل:", userProfile.HomePhone);  // Assuming HomePhone for "تلفن منزل"
            //        ReplacePlaceholder(body, "دورنگار:", userProfile.Fax);
            //        ReplacePlaceholder(body, "بهترین زمان برای تماس تلفنی:", userProfile.BestTimeToCall);
            //        ReplacePlaceholder(body, "نشانی محل سکونت:", userProfile.ResidentialAddress);
            //        ReplacePlaceholder(body, "پست الکترونیک:", userProfile.Email);

            //        outputDoc.MainDocumentPart.Document.Save();
            //    }
            //}

            return PhysicalFile(outputPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", outputPath);
        }


        //[HttpGet("{id}")]
        //public IActionResult GenerateReport(int id)
        //{
        //    var userProfile = _context.HaghighiUserProfiles.Find(id);
        //    if (userProfile == null)
        //    {
        //        return NotFound();
        //    }

        //    var financialProfiles = _context.HaghighiUserFinancialProfiles
        //        .Where(fp => fp.RequestId == userProfile.RequestId && !fp.IsDeleted)
        //        .ToList();

        //    var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "D:\\MMNazari\\Documents\\Novin\\PRX Docs\\فرم_كسب_اطلاعات_مشتريان_و_پرسشنامه_ارزيابي_توان_مشتري_حقیقی.docx");
        //    var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
        //    Directory.CreateDirectory(outputDirectory);  // Ensure the output directory exists
        //    var outputPath = Path.Combine(outputDirectory, $"output-{id}.docx");





        //    // Load the template document using Xceed DocX
        //    using (var template = DocX.Load(templatePath))
        //    {
        //        // Replace placeholders with user profile data
        //        ReplacePlaceholder(template, "نام:", userProfile.FirstName);
        //        ReplacePlaceholder(template, "نام خانوادگی:", userProfile.LastName);
        //        ReplacePlaceholder(template, "نام پدر:", userProfile.FathersName);
        //        ReplacePlaceholder(template, "کدملی:", userProfile.NationalNumber);
        //        ReplacePlaceholder(template, "تاریخ تولد:", userProfile.BirthDate.ToString("yyyy-MM-dd"));
        //        ReplacePlaceholder(template, "محل صدور:", userProfile.BirthPlace);
        //        ReplacePlaceholder(template, "شماره شناسنامه:", userProfile.BirthCertificateNumber);
        //        ReplacePlaceholder(template, "وضعیت تأهل:", userProfile.MaritalStatus);
        //        ReplacePlaceholder(template, "جنسیت:", userProfile.Gender);
        //        ReplacePlaceholder(template, "کدپستی:", userProfile.PostalCode.ToString());
        //        ReplacePlaceholder(template, "تلفن همراه:", userProfile.HomePhone);
        //        ReplacePlaceholder(template, "تلفن منزل:", userProfile.HomePhone);  // Assuming HomePhone for "تلفن منزل"
        //        ReplacePlaceholder(template, "دورنگار:", userProfile.Fax);
        //        ReplacePlaceholder(template, "بهترین زمان برای تماس تلفنی:", userProfile.BestTimeToCall);
        //        ReplacePlaceholder(template, "نشانی محل سکونت:", userProfile.ResidentialAddress);
        //        ReplacePlaceholder(template, "پست الکترونیک:", userProfile.Email);

        //        // Find and fill the financial profiles table
        //        var table = template.Tables.FirstOrDefault(t => t.Rows.Any(r => r.Cells.Any(c => c.Paragraphs.Any(p => p.Text.Contains("مبلغ درآمد اصلی و مستمر")))));
        //        if (table != null)
        //        {
        //            // Clear existing rows except header row
        //            var rowsToRemove = table.Rows.Skip(1).ToList();
        //            foreach (var row in rowsToRemove)
        //            {
        //                row.Remove();
        //            }

        //            // Populate the table with financial profiles data
        //            foreach (var profile in financialProfiles)
        //            {
        //                var newRow = table.InsertRow();
        //                newRow.Cells[0].Paragraphs.First().Append(profile.MainContinuousIncome.ToString("N0"));
        //                newRow.Cells[1].Paragraphs.First().Append(profile.OtherIncomes.ToString("N0"));
        //                newRow.Cells[2].Paragraphs.First().Append(profile.SupportFromOthers.ToString("N0"));
        //                newRow.Cells[3].Paragraphs.First().Append(profile.ContinuousExpenses.ToString("N0"));
        //                newRow.Cells[4].Paragraphs.First().Append(profile.OccasionalExpenses.ToString("N0"));
        //                newRow.Cells[5].Paragraphs.First().Append(profile.ContributionToOthers.ToString("N0"));
        //                // Add more cells as needed
        //            }
        //        }

        //        // Save the modified template to the output path
        //        template.SaveAs(outputPath);
        //    }

        //    // Return the generated document as a physical file
        //    return PhysicalFile(outputPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        //}

        //// Helper method to replace placeholders in the document
        //private void ReplacePlaceholder(DocX document, string placeholder, string value)
        //{
        //    if (!string.IsNullOrEmpty(value))
        //    {
        //        document.ReplaceText(placeholder, value);
        //    }
        //}


        //private void ReplacePlaceholder(Body body, string placeholder, string value)
        //{
        //    foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
        //    {
        //        if (text.Text.Contains(placeholder))
        //        {
        //            text.Text = text.Text.Replace(placeholder, value);
        //        }
        //    }
        //}

        private void ReplacePlaceholder(Body body, string placeholder, string value)
        {
            foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
            {
                if (text.Text.Contains(placeholder))
                {
                    var run = text.Parent as Run;
                    if (run != null)
                    {
                        var newRun = run.CloneNode(true) as Run;
                        if (newRun != null)
                        {
                            var newTextElement = newRun.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Text>();

                            // Ensure newTextElement is not null
                            if (newTextElement == null)
                            {
                                newTextElement = new DocumentFormat.OpenXml.Wordprocessing.Text();
                                newRun.AppendChild(newTextElement);
                            }

                            newTextElement.Text = value;

                            // Append the new run after the current run
                            run.Parent.InsertAfter(newRun, run);
                        }
                    }
                }
            }
        }

        private void FillRelationshipsTable(Body body, List<HaghighiUserRelationships> relationships)
        {
            // Locate the table using the appropriate method (finding by position or by specific characteristics)
            var table = body.Descendants<Table>().FirstOrDefault();

            if (table != null)
            {
                var existingRows = table.Elements<TableRow>().ToList();
                var headerRow = existingRows.First();  // Keep the header row

                // Remove existing data rows except the header
                foreach (var row in existingRows.Skip(1).ToList())
                {
                    row.Remove();
                }

                // Add the relationship data to the table
                for (int i = 0; i < relationships.Count; i++)
                {
                    var relationship = relationships[i];

                    // Create a new row
                    var tableRow = new TableRow();

                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text((i + 1).ToString())))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.FullName)))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.RelationshipStatus)))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.BirthYear.ToString())))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.EducationLevel)))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.EmploymentStatus)))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.AverageMonthlyIncome.ToString("N0"))))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.AverageMonthlyExpense.ToString("N0"))))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.ApproximateAssets.ToString("N0"))))));
                    tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.ApproximateLiabilities.ToString("N0"))))));

                    // Append the row to the table
                    table.Append(tableRow);
                }
            }
        }

        //private void FillRelationshipsTable(Body body, List<HaghighiUserRelationships> relationships)
        //{
        //    // Find the table by a more specific method (e.g., by a placeholder or structure)
        //    var tables = body.Descendants<Table>().ToList();

        //    // Assuming the second table is the relationships table (you may need a more precise identifier)
        //    var table = tables.FirstOrDefault(); // Adjust this to find the correct table

        //    if (table != null)
        //    {
        //        var existingRows = table.Elements<TableRow>().ToList();
        //        var headerRow = existingRows.First();  // Keep the header row

        //        // Remove existing data rows except the header
        //        foreach (var row in existingRows.Skip(1).ToList())
        //        {
        //            row.Remove();
        //        }

        //        // Add the relationship data to the table
        //        foreach (var relationship in relationships)
        //        {
        //            var tableRow = new TableRow(
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text((relationships.IndexOf(relationship) + 1).ToString())))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.FullName)))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.RelationshipStatus)))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.BirthYear.ToString())))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.EducationLevel)))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.EmploymentStatus)))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.AverageMonthlyIncome.ToString("N0"))))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.AverageMonthlyExpense.ToString("N0"))))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.ApproximateAssets.ToString("N0"))))),
        //                new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.ApproximateLiabilities.ToString("N0")))))
        //            );

        //            // Append the row to the table
        //            table.Append(tableRow);
        //        }
        //    }
        //}

        //private void FillFinancialProfileTable(Body body, List<HaghighiUserFinancialProfile> financialProfiles)
        //{
        //    var tables = body.Descendants<Table>().ToList();

        //    // Example: Identify the table based on specific text in the first row's first cell
        //    var table = tables.FirstOrDefault(t =>
        //    {
        //        var firstRow = t.Elements<TableRow>().FirstOrDefault();
        //        if (firstRow != null)
        //        {
        //            var firstCellText = firstRow.Descendants<DocumentFormat.OpenXml.Drawing.Text>().FirstOrDefault()?.InnerText?.Trim();
        //            if (firstCellText != null && firstCellText.Contains("مبلغ درآمد اصلی و مستمر"))
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    });

        //    if (table != null)
        //    {
        //        var headerRow = table.Elements<TableRow>().FirstOrDefault(); // Assuming the first row is header

        //        if (headerRow != null)
        //        {
        //            // Clear existing rows except header
        //            var rowsToRemove = table.Elements<TableRow>().Skip(1).ToList();
        //            foreach (var row in rowsToRemove)
        //            {
        //                row.Remove();
        //            }

        //            // Populate the table with data from financial profiles
        //            foreach (var profile in financialProfiles)
        //            {
        //                var tableRow = new TableRow(
        //                    new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(profile.MainContinuousIncome.ToString("N0"))))),
        //                    new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(profile.OtherIncomes.ToString("N0"))))),
        //                    new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(profile.SupportFromOthers.ToString("N0"))))),
        //                    new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(profile.ContinuousExpenses.ToString("N0"))))),
        //                    new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(profile.OccasionalExpenses.ToString("N0"))))),
        //                    new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(profile.ContributionToOthers.ToString("N0")))))
        //                // Add more cells as needed
        //                );

        //                table.Append(tableRow);
        //            }
        //        }
        //    }
        //}

        private void FillFinancialProfileTable(string templatePath, string outputPath, List<HaghighiUserFinancialProfile> financialProfiles)
        {
            try
            {
                // Load the document
                using (DocX document = DocX.Load(templatePath))
                {
                    // Find the table by searching for a row with specific text in the first cell
                    var table = document.Tables.FirstOrDefault(t =>
                    {
                        var firstRow = t.Rows.FirstOrDefault();
                        if (firstRow != null)
                        {
                            var firstCellText = firstRow.Cells.FirstOrDefault()?.Paragraphs.FirstOrDefault()?.Text?.Trim();
                            if (firstCellText != null && firstCellText.Contains("مبلغ درآمد اصلی و مستمر"))
                            {
                                return true;
                            }
                        }
                        return false;
                    });

                    if (table != null)
                    {
                        // Clear existing rows except header
                        var rowsToRemove = table.Rows.Skip(1).ToList();
                        foreach (var row in rowsToRemove)
                        {
                            row.Remove();
                        }

                        // Populate the table with data from financial profiles
                        foreach (var profile in financialProfiles)
                        {
                            var tableRow = table.InsertRow();

                            tableRow.Cells[0].Paragraphs.First().Append(profile.MainContinuousIncome.ToString("N0"));
                            tableRow.Cells[1].Paragraphs.First().Append(profile.OtherIncomes.ToString("N0"));
                            tableRow.Cells[2].Paragraphs.First().Append(profile.SupportFromOthers.ToString("N0"));
                            tableRow.Cells[3].Paragraphs.First().Append(profile.ContinuousExpenses.ToString("N0"));
                            tableRow.Cells[4].Paragraphs.First().Append(profile.OccasionalExpenses.ToString("N0"));
                            tableRow.Cells[5].Paragraphs.First().Append(profile.ContributionToOthers.ToString("N0"));
                            // Add more cells as needed
                        }

                        // Save the document
                        document.SaveAs(outputPath);
                    }
                    else
                    {
                        Console.WriteLine("Financial profile table not found in the document.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
