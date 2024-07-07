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
using OfficeOpenXml;
using System.Reflection;

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

        //[HttpGet("export")]
        //public async Task<IActionResult> Export()
        //{
        //    var memoryStream = new MemoryStream();

        //    using (var package = new ExcelPackage(memoryStream))
        //    {
        //        // Add sheet and data for Users
        //        var userSheet = package.Workbook.Worksheets.Add("Users");
        //        var userData = await _context.Users.ToListAsync();
        //        AddDataToSheet(userSheet, userData);

        //        package.Save();
        //    }

        //    memoryStream.Position = 0;
        //    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    var fileName = "Users.xlsx";
        //    return File(memoryStream, contentType, fileName);
        //}

        //private void AddDataToSheet<T>(ExcelWorksheet sheet, List<T> data)
        //{
        //    // Add header
        //    var properties = typeof(T).GetProperties();
        //    for (var i = 0; i < properties.Length; i++)
        //    {
        //        sheet.Cells[1, i + 1].Value = properties[i].Name;
        //    }

        //    // Add data
        //    for (var row = 0; row < data.Count; row++)
        //    {
        //        for (var col = 0; col < properties.Length; col++)
        //        {
        //            sheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(data[row]);
        //        }
        //    }
        //}

        [HttpGet("ExportAll")]
        public async Task<IActionResult> Export()
        {
            var memoryStream = new MemoryStream();

            using (var package = new ExcelPackage(memoryStream))
            {
                //// Add sheet and data for Users
                //var userSheet = package.Workbook.Worksheets.Add("Users");
                //var userData = await _context.Users.ToListAsync();
                //AddDataToSheet(userSheet, userData, GetUserColumnMappings());

                // Add sheet and data for Users
                var userSheet = package.Workbook.Worksheets.Add("کاربران");
                var userData = await _context.Users.ToListAsync() ;
                AddDataToSheet(userSheet, userData, GetUserColumnMappings());

                var orderSheet = package.Workbook.Worksheets.Add("بدهی ها");
                var orderData = await _context.UserDebts.ToListAsync();
                AddDataToSheet(orderSheet, orderData, GetUserDebtColumnMappings());

                // Add sheet and data for UserFinancialChanges
                var userFinancialChangesSheet = package.Workbook.Worksheets.Add("تغییر احتمالی درآمد");
                var userFinancialChangesData = await _context.UserFinancialChanges.ToListAsync();
                AddDataToSheet(userFinancialChangesSheet, userFinancialChangesData, GetUserFinancialChangesColumnMappings());

                // Add sheet and data for UserDocuments
                var userDocumentSheet = package.Workbook.Worksheets.Add("اسناد");
                var userDocumentData = await _context.UserDocuments.ToListAsync();
                AddDataToSheet(userDocumentSheet, userDocumentData, GetUserDocumentColumnMappings());

                // Add sheet and data for UserDeposits
                var userDepositSheet = package.Workbook.Worksheets.Add("واریزی ها");
                var userDepositData = await _context.UserDeposits.ToListAsync();
                AddDataToSheet(userDepositSheet, userDepositData, GetUserDepositColumnMappings());

                // Add sheet and data for UserAssets
                var userAssetSheet = package.Workbook.Worksheets.Add("دارایی ها");
                var userAssetData = await _context.UserAssets.ToListAsync();
                AddDataToSheet(userAssetSheet, userAssetData, GetUserAssetColumnMappings());

                // Add sheet and data for UserFuturePlans
                var userFuturePlansSheet = package.Workbook.Worksheets.Add("برنامه های آینده");
                var userFuturePlansData = await _context.UserFuturePlans.ToListAsync();
                AddDataToSheet(userFuturePlansSheet, userFuturePlansData, GetUserFuturePlansColumnMappings());

                // Add sheet and data for UserInvestments
                var userInvestmentSheet = package.Workbook.Worksheets.Add("مبلغ سرمایه گذاری");
                var userInvestmentData = await _context.UserInvestments.ToListAsync();
                AddDataToSheet(userInvestmentSheet, userInvestmentData, GetUserInvestmentColumnMappings());

                // Add sheet and data for UserInvestmentExperiences
                var userInvestmentExperienceSheet = package.Workbook.Worksheets.Add("سوابق سرمایه گذاری");
                var userInvestmentExperienceData = await _context.UserInvestmentExperiences.ToListAsync();
                AddDataToSheet(userInvestmentExperienceSheet, userInvestmentExperienceData, GetUserInvestmentExperienceColumnMappings());

                // Add sheet and data for UserMoreInformation
                var userMoreInformationSheet = package.Workbook.Worksheets.Add("اطلاعات اضافه");
                var userMoreInformationData = await _context.UserMoreInformations.ToListAsync();
                AddDataToSheet(userMoreInformationSheet, userMoreInformationData, GetUserMoreInformationColumnMappings());

                // Add sheet and data for UserWithdrawals
                var userWithdrawalSheet = package.Workbook.Worksheets.Add("برداشت ها");
                var userWithdrawalData = await _context.UserWithdrawals.ToListAsync();
                AddDataToSheet(userWithdrawalSheet, userWithdrawalData, GetUserWithdrawalColumnMappings());

                // Add sheet and data for UserTestScores
                var userTestScoreSheet = package.Workbook.Worksheets.Add("نتیجه آزمون");
                var userTestScoreData = await _context.UserTestScores.ToListAsync();
                AddDataToSheet(userTestScoreSheet, userTestScoreData, GetUserTestScoreColumnMappings());

                // Add sheet and data for HaghighiUserBankInfos
                var haghighiUserBankInfoSheet = package.Workbook.Worksheets.Add("اطلاعات بانکی کاربر حقیقی");
                var haghighiUserBankInfoData = await _context.HaghighiUserBankInfos.ToListAsync();
                AddDataToSheet(haghighiUserBankInfoSheet, haghighiUserBankInfoData, GetHaghighiUserBankInfoColumnMappings());

                // Add sheet and data for HaghighiUserEducationStatuses
                var haghighiUserEducationStatusSheet = package.Workbook.Worksheets.Add("آخرین مدرک تحصیلی کاربر حقیقی");
                var haghighiUserEducationStatusData = await _context.HaghighiUserEducationStatuses.ToListAsync();
                AddDataToSheet(haghighiUserEducationStatusSheet, haghighiUserEducationStatusData, GetHaghighiUserEducationStatusColumnMappings());

                // Add sheet and data for HaghighiUserFinancialProfiles
                var haghighiUserFinancialProfileSheet = package.Workbook.Worksheets.Add("وضعیت درآمدی کاربر حقیقی");
                var haghighiUserFinancialProfileData = await _context.HaghighiUserFinancialProfiles.ToListAsync();
                AddDataToSheet(haghighiUserFinancialProfileSheet, haghighiUserFinancialProfileData, GetHaghighiUserFinancialProfileColumnMappings());

                // Add sheet and data for HaghighiUserRelationships
                var haghighiUserRelationshipsSheet = package.Workbook.Worksheets.Add("افراد تحت تکفل کاربر حقیقی");
                var haghighiUserRelationshipsData = await _context.HaghighiUserRelationships.ToListAsync();
                AddDataToSheet(haghighiUserRelationshipsSheet, haghighiUserRelationshipsData, GetHaghighiUserRelationshipsColumnMappings());

                // Add sheet and data for HaghighiUserProfiles
                var haghighiUserProfileSheet = package.Workbook.Worksheets.Add("مشخصات کاربر حقیقی");
                var haghighiUserProfileData = await _context.HaghighiUserProfiles.ToListAsync();
                AddDataToSheet(haghighiUserProfileSheet, haghighiUserProfileData, GetHaghighiUserProfileColumnMappings());

                // Add sheet and data for HaghighiUserEmploymentHistories
                var haghighiUserEmploymentHistorySheet = package.Workbook.Worksheets.Add("سوابق شغلی کاربر حقیقی");
                var haghighiUserEmploymentHistoryData = await _context.HaghighiUserEmploymentHistories.ToListAsync();
                AddDataToSheet(haghighiUserEmploymentHistorySheet, haghighiUserEmploymentHistoryData, GetHaghighiUserEmploymentHistoryColumnMappings());

                // Add sheet and data for HoghooghiUserInvestmentDepartmentStaff
                var hoghooghiUserInvestmentDepartmentStaffSheet = package.Workbook.Worksheets.Add("مشخصات کارکنان کاربر حقوقی");
                var hoghooghiUserInvestmentDepartmentStaffData = await _context.HoghooghiUserInvestmentDepartmentStaff.ToListAsync();
                AddDataToSheet(hoghooghiUserInvestmentDepartmentStaffSheet, hoghooghiUserInvestmentDepartmentStaffData, GetHoghooghiUserInvestmentDepartmentStaffColumnMappings());

                // Add sheet and data for HoghooghiUserCompaniesWithMajorInvestors
                var hoghooghiUserCompaniesWithMajorInvestorsSheet = package.Workbook.Worksheets.Add("شرکت های سهامدار کاربر حقوقی");
                var hoghooghiUserCompaniesWithMajorInvestorsData = await _context.HoghooghiUserCompaniesWithMajorInvestors.ToListAsync();
                AddDataToSheet(hoghooghiUserCompaniesWithMajorInvestorsSheet, hoghooghiUserCompaniesWithMajorInvestorsData, GetHoghooghiUserCompaniesWithMajorInvestorsColumnMappings());

                // Add sheet and data for HoghooghiUserBoardOfDirectors
                var hoghooghiUserBoardOfDirectorsSheet = package.Workbook.Worksheets.Add("مشخصات هیِات مدیره کاربر حقوقی");
                var hoghooghiUserBoardOfDirectorsData = await _context.HoghooghiUserBoardOfDirectors.ToListAsync();
                AddDataToSheet(hoghooghiUserBoardOfDirectorsSheet, hoghooghiUserBoardOfDirectorsData, GetHoghooghiUserBoardOfDirectorsColumnMappings());

                // Add sheet and data for HoghooghiUserAssetIncomeStatusTwoYearsAgo
                var hoghooghiUserAssetIncomeStatusTwoYearsAgoSheet = package.Workbook.Worksheets.Add("وضعیت درآمدی دو سال گذشته کاربر حقوقی");
                var hoghooghiUserAssetIncomeStatusTwoYearsAgoData = await _context.HoghooghiUsersAssets.ToListAsync();
                AddDataToSheet(hoghooghiUserAssetIncomeStatusTwoYearsAgoSheet, hoghooghiUserAssetIncomeStatusTwoYearsAgoData, GetHoghooghiUserAssetIncomeStatusTwoYearsAgoColumnMappings());

                // Add sheet and data for HoghooghiUsers
                var hoghooghiUserSheet = package.Workbook.Worksheets.Add("مشخصات کاربر حقوقی");
                var hoghooghiUserData = await _context.HoghooghiUsers.ToListAsync();
                AddDataToSheet(hoghooghiUserSheet, hoghooghiUserData, GetHoghooghiUserColumnMappings());

                package.Save();
            }

            memoryStream.Position = 0;
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "PRXTables.xlsx";
            return File(memoryStream, contentType, fileName);
        }

        [HttpGet("ExportById/{userId}/{requestId}")]
        public async Task<IActionResult> ExportByUserId(int userId, int requestId)
        {
            var memoryStream = new MemoryStream();

            using (var package = new ExcelPackage(memoryStream))
            {
                // Add sheet and data for Users
                var userSheet = package.Workbook.Worksheets.Add("کاربران");
                var userData = userId > 0 ? await _context.Users.Where(u => u.Id == userId).ToListAsync() : await _context.Users.ToListAsync();
                AddDataToSheet(userSheet, userData, GetUserColumnMappings());

                var orderSheet = package.Workbook.Worksheets.Add("بدهی ها");
                var orderData = requestId > 0 ? await _context.UserDebts.Where(o => o.RequestId == requestId).ToListAsync() : await _context.UserDebts.ToListAsync();
                AddDataToSheet(orderSheet, orderData, GetUserDebtColumnMappings());

                // Add sheet and data for UserFinancialChanges
                var userFinancialChangesSheet = package.Workbook.Worksheets.Add("تغییر احتمالی درآمد");
                var userFinancialChangesData = requestId > 0 ? await _context.UserFinancialChanges.Where(ufc => ufc.RequestId == requestId).ToListAsync() : await _context.UserFinancialChanges.ToListAsync();
                AddDataToSheet(userFinancialChangesSheet, userFinancialChangesData, GetUserFinancialChangesColumnMappings());

                // Add sheet and data for UserDocuments
                var userDocumentSheet = package.Workbook.Worksheets.Add("اسناد");
                var userDocumentData = requestId > 0 ? await _context.UserDocuments.Where(ud => ud.RequestId == requestId).ToListAsync() : await _context.UserDocuments.ToListAsync();
                AddDataToSheet(userDocumentSheet, userDocumentData, GetUserDocumentColumnMappings());

                // Add sheet and data for UserDeposits
                var userDepositSheet = package.Workbook.Worksheets.Add("واریزی ها");
                var userDepositData = requestId > 0 ? await _context.UserDeposits.Where(ud => ud.RequestId == requestId).ToListAsync() : await _context.UserDeposits.ToListAsync();
                AddDataToSheet(userDepositSheet, userDepositData, GetUserDepositColumnMappings());

                // Add sheet and data for UserAssets
                var userAssetSheet = package.Workbook.Worksheets.Add("دارایی ها");
                var userAssetData = requestId > 0 ? await _context.UserAssets.Where(ua => ua.RequestId == requestId).ToListAsync() : await _context.UserAssets.ToListAsync();
                AddDataToSheet(userAssetSheet, userAssetData, GetUserAssetColumnMappings());

                // Add sheet and data for UserFuturePlans
                var userFuturePlansSheet = package.Workbook.Worksheets.Add("برنامه های آینده");
                var userFuturePlansData = requestId > 0 ? await _context.UserFuturePlans.Where(ufp => ufp.RequestId == requestId).ToListAsync() : await _context.UserFuturePlans.ToListAsync();
                AddDataToSheet(userFuturePlansSheet, userFuturePlansData, GetUserFuturePlansColumnMappings());

                // Add sheet and data for UserInvestments
                var userInvestmentSheet = package.Workbook.Worksheets.Add("مبلغ سرمایه گذاری");
                var userInvestmentData = requestId > 0 ? await _context.UserInvestments.Where(ui => ui.RequestId == requestId).ToListAsync() : await _context.UserInvestments.ToListAsync();
                AddDataToSheet(userInvestmentSheet, userInvestmentData, GetUserInvestmentColumnMappings());

                // Add sheet and data for UserInvestmentExperiences
                var userInvestmentExperienceSheet = package.Workbook.Worksheets.Add("سوابق سرمایه گذاری");
                var userInvestmentExperienceData = requestId > 0 ? await _context.UserInvestmentExperiences.Where(uie => uie.RequestId == requestId).ToListAsync() : await _context.UserInvestmentExperiences.ToListAsync();
                AddDataToSheet(userInvestmentExperienceSheet, userInvestmentExperienceData, GetUserInvestmentExperienceColumnMappings());

                // Add sheet and data for UserMoreInformation
                var userMoreInformationSheet = package.Workbook.Worksheets.Add("اطلاعات اضافه");
                var userMoreInformationData = requestId > 0 ? await _context.UserMoreInformations.Where(umi => umi.RequestId == requestId).ToListAsync() : await _context.UserMoreInformations.ToListAsync();
                AddDataToSheet(userMoreInformationSheet, userMoreInformationData, GetUserMoreInformationColumnMappings());

                // Add sheet and data for UserWithdrawals
                var userWithdrawalSheet = package.Workbook.Worksheets.Add("برداشت ها");
                var userWithdrawalData = requestId > 0 ? await _context.UserWithdrawals.Where(uw => uw.RequestId == requestId).ToListAsync() : await _context.UserWithdrawals.ToListAsync();
                AddDataToSheet(userWithdrawalSheet, userWithdrawalData, GetUserWithdrawalColumnMappings());

                // Add sheet and data for UserTestScores
                var userTestScoreSheet = package.Workbook.Worksheets.Add("نتیجه آزمون");
                var userTestScoreData = requestId > 0 ? await _context.UserTestScores.Where(uts => uts.RequestId == requestId).ToListAsync() : await _context.UserTestScores.ToListAsync();
                AddDataToSheet(userTestScoreSheet, userTestScoreData, GetUserTestScoreColumnMappings());

                // Add sheet and data for HaghighiUserBankInfos
                var haghighiUserBankInfoSheet = package.Workbook.Worksheets.Add("اطلاعات بانکی کاربر حقیقی");
                var haghighiUserBankInfoData = requestId > 0 ? await _context.HaghighiUserBankInfos.Where(hubi => hubi.RequestId == requestId).ToListAsync() : await _context.HaghighiUserBankInfos.ToListAsync();
                AddDataToSheet(haghighiUserBankInfoSheet, haghighiUserBankInfoData, GetHaghighiUserBankInfoColumnMappings());

                // Add sheet and data for HaghighiUserEducationStatuses
                var haghighiUserEducationStatusSheet = package.Workbook.Worksheets.Add("آخرین مدرک تحصیلی کاربر حقیقی");
                var haghighiUserEducationStatusData = requestId > 0 ? await _context.HaghighiUserEducationStatuses.Where(hues => hues.RequestId == requestId).ToListAsync() : await _context.HaghighiUserEducationStatuses.ToListAsync();
                AddDataToSheet(haghighiUserEducationStatusSheet, haghighiUserEducationStatusData, GetHaghighiUserEducationStatusColumnMappings());

                // Add sheet and data for HaghighiUserFinancialProfiles
                var haghighiUserFinancialProfileSheet = package.Workbook.Worksheets.Add("وضعیت درآمدی کاربر حقیقی");
                var haghighiUserFinancialProfileData = requestId > 0 ? await _context.HaghighiUserFinancialProfiles.Where(hufp => hufp.RequestId == requestId).ToListAsync() : await _context.HaghighiUserFinancialProfiles.ToListAsync();
                AddDataToSheet(haghighiUserFinancialProfileSheet, haghighiUserFinancialProfileData, GetHaghighiUserFinancialProfileColumnMappings());

                // Add sheet and data for HaghighiUserRelationships
                var haghighiUserRelationshipsSheet = package.Workbook.Worksheets.Add("افراد تحت تکفل کاربر حقیقی");
                var haghighiUserRelationshipsData = requestId > 0 ? await _context.HaghighiUserRelationships.Where(hur => hur.RequestId == requestId).ToListAsync() : await _context.HaghighiUserRelationships.ToListAsync();
                AddDataToSheet(haghighiUserRelationshipsSheet, haghighiUserRelationshipsData, GetHaghighiUserRelationshipsColumnMappings());

                // Add sheet and data for HaghighiUserProfiles
                var haghighiUserProfileSheet = package.Workbook.Worksheets.Add("مشخصات کاربر حقیقی");
                var haghighiUserProfileData = requestId > 0 ? await _context.HaghighiUserProfiles.Where(hup => hup.RequestId == requestId).ToListAsync() : await _context.HaghighiUserProfiles.ToListAsync();
                AddDataToSheet(haghighiUserProfileSheet, haghighiUserProfileData, GetHaghighiUserProfileColumnMappings());

                // Add sheet and data for HaghighiUserEmploymentHistories
                var haghighiUserEmploymentHistorySheet = package.Workbook.Worksheets.Add("سوابق شغلی کاربر حقیقی");
                var haghighiUserEmploymentHistoryData = requestId > 0 ? await _context.HaghighiUserEmploymentHistories.Where(hueh => hueh.RequestId == requestId).ToListAsync() : await _context.HaghighiUserEmploymentHistories.ToListAsync();
                AddDataToSheet(haghighiUserEmploymentHistorySheet, haghighiUserEmploymentHistoryData, GetHaghighiUserEmploymentHistoryColumnMappings());

                // Add sheet and data for HoghooghiUserInvestmentDepartmentStaff
                var hoghooghiUserInvestmentDepartmentStaffSheet = package.Workbook.Worksheets.Add("مشخصات کارکنان کاربر حقوقی");
                var hoghooghiUserInvestmentDepartmentStaffData = requestId > 0 ? await _context.HoghooghiUserInvestmentDepartmentStaff.Where(huid => huid.RequestId == requestId).ToListAsync() : await _context.HoghooghiUserInvestmentDepartmentStaff.ToListAsync();
                AddDataToSheet(hoghooghiUserInvestmentDepartmentStaffSheet, hoghooghiUserInvestmentDepartmentStaffData, GetHoghooghiUserInvestmentDepartmentStaffColumnMappings());

                // Add sheet and data for HoghooghiUserCompaniesWithMajorInvestors
                var hoghooghiUserCompaniesWithMajorInvestorsSheet = package.Workbook.Worksheets.Add("شرکت های سهامدار کاربر حقوقی");
                var hoghooghiUserCompaniesWithMajorInvestorsData = requestId > 0 ? await _context.HoghooghiUserCompaniesWithMajorInvestors.Where(hucwmi => hucwmi.RequestId == requestId).ToListAsync() : await _context.HoghooghiUserCompaniesWithMajorInvestors.ToListAsync();
                AddDataToSheet(hoghooghiUserCompaniesWithMajorInvestorsSheet, hoghooghiUserCompaniesWithMajorInvestorsData, GetHoghooghiUserCompaniesWithMajorInvestorsColumnMappings());

                // Add sheet and data for HoghooghiUserBoardOfDirectors
                var hoghooghiUserBoardOfDirectorsSheet = package.Workbook.Worksheets.Add("مشخصات هیِات مدیره کاربر حقوقی");
                var hoghooghiUserBoardOfDirectorsData = requestId > 0 ? await _context.HoghooghiUserBoardOfDirectors.Where(hubod => hubod.RequestId == requestId).ToListAsync() : await _context.HoghooghiUserBoardOfDirectors.ToListAsync();
                AddDataToSheet(hoghooghiUserBoardOfDirectorsSheet, hoghooghiUserBoardOfDirectorsData, GetHoghooghiUserBoardOfDirectorsColumnMappings());

                // Add sheet and data for HoghooghiUserAssetIncomeStatusTwoYearsAgo
                var hoghooghiUserAssetIncomeStatusTwoYearsAgoSheet = package.Workbook.Worksheets.Add("وضعیت درآمدی دو سال گذشته کاربر حقوقی");
                var hoghooghiUserAssetIncomeStatusTwoYearsAgoData = requestId > 0 ? await _context.HoghooghiUsersAssets.Where(huaisty => huaisty.RequestId == requestId).ToListAsync() : await _context.HoghooghiUsersAssets.ToListAsync();
                AddDataToSheet(hoghooghiUserAssetIncomeStatusTwoYearsAgoSheet, hoghooghiUserAssetIncomeStatusTwoYearsAgoData, GetHoghooghiUserAssetIncomeStatusTwoYearsAgoColumnMappings());

                // Add sheet and data for HoghooghiUsers
                var hoghooghiUserSheet = package.Workbook.Worksheets.Add("مشخصات کاربر حقوقی");
                var hoghooghiUserData = requestId > 0 ? await _context.HoghooghiUsers.Where(hu => hu.RequestId == requestId).ToListAsync() : await _context.HoghooghiUsers.ToListAsync();
                AddDataToSheet(hoghooghiUserSheet, hoghooghiUserData, GetHoghooghiUserColumnMappings());

                package.Save();
            }

            memoryStream.Position = 0;
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"PRXTables-User{userId}-Request{requestId}.xlsx";
            return File(memoryStream, contentType, fileName);
        }

        //[HttpGet("ExportByRequestId/{requestId}")]
        //public async Task<IActionResult> ExportByRequestId(int requestId)
        //{
        //    var memoryStream = new MemoryStream();

        //    using (var package = new ExcelPackage(memoryStream))
        //    {
        //        // Add sheet and data for Orders
        //        var orderSheet = package.Workbook.Worksheets.Add("Debts");
        //        var orderData = requestId > 0 ? await _context.UserDebts.Where(o => o.RequestId == requestId).ToListAsync() : await _context.UserDebts.ToListAsync();
        //        AddDataToSheet(orderSheet, orderData, GetUserDebtColumnMappings());


        //        package.Save();
        //    }

        //    memoryStream.Position = 0;
        //    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    var fileName = $"PRXTables-Request{requestId}.xlsx";
        //    return File(memoryStream, contentType, fileName);
        //}

        //private void AddDataToSheet<T>(ExcelWorksheet sheet, List<T> data, Dictionary<string, string> columnMappings)
        //{
        //    // Add header
        //    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //                              .Where(p => !p.PropertyType.IsClass || p.PropertyType == typeof(string))
        //                              .ToList();
        //    for (var i = 0; i < properties.Count; i++)
        //    {
        //        var propertyName = properties[i].Name;
        //        sheet.Cells[1, i + 1].Value = columnMappings.ContainsKey(propertyName) ? columnMappings[propertyName] : propertyName;
        //    }

        //    // Add data
        //    for (var row = 0; row < data.Count; row++)
        //    {
        //        for (var col = 0; col < properties.Count; col++)
        //        {
        //            sheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(data[row]);
        //        }
        //    }
        //}

        private void AddDataToSheet<T>(ExcelWorksheet sheet, List<T> data, Dictionary<string, string> columnMappings)
        {
            // Add header
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                      .Where(p => !p.PropertyType.IsClass || p.PropertyType == typeof(string))
                                      .ToList();
            for (var i = 0; i < properties.Count; i++)
            {
                var propertyName = properties[i].Name;
                sheet.Cells[1, i + 1].Value = columnMappings.ContainsKey(propertyName) ? columnMappings[propertyName] : propertyName;
                // Customize header font
                sheet.Cells[1, i + 1].Style.Font.Bold = true;
                sheet.Cells[1, i + 1].Style.Font.Size = 12;
                sheet.Cells[1, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                sheet.Cells[1, i + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }

            // Add data
            for (var row = 0; row < data.Count; row++)
            {
                for (var col = 0; col < properties.Count; col++)
                {
                    sheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(data[row]);
                    // Customize data cell format
                    sheet.Cells[row + 2, col + 1].Style.Font.Size = 10;
                    sheet.Cells[row + 2, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[row + 2, col + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }
            }

            // Auto-fit columns and set row heights
            for (var col = 1; col <= properties.Count; col++)
            {
                sheet.Column(col).AutoFit();
            }

            for (var row = 1; row <= data.Count + 1; row++)
            {
                sheet.Row(row).Height = 20;
            }
        }

        private Dictionary<string, string> GetUserColumnMappings()
        {
            return new Dictionary<string, string>
        {
            { "Id", "شناسه کاربر" },
            { "Password", "رمز عبور هش شده" },
            { "Username", "نام کاربری" },
            { "PhoneNumber", "شماره تلفن" },
            { "ReferenceCode", "کد معرف" },
            { "Role", "نقش" },
            { "IsDeleted", "خذف شده" },
            { "FirstName", "نام" },
            { "LastName", "نام خانوادگی" },
            { "BirthCertificateNumber", "کد ملی" }
        };
        }

        private Dictionary<string, string> GetUserDebtColumnMappings()
        {
            return new Dictionary<string, string>
        {
            { "Id", "شناسه کاربر" },
            { "RequestId", "شناسه درخواست " },
            { "DebtTitle", "عنوان بدهی" },
            { "DebtAmount", "مبلغ بدهی" },
            { "DebtDueDate", "تاریخ بازگشت" },
            { "DebtRepaymentPercentage", "درصد بدهی" },
            { "IsComplete", "کامل شده" },
            { "IsDeleted", "حذف شده" }
        };
        }

        private Dictionary<string, string> GetUserAssetColumnMappings()
        {
            return new Dictionary<string, string>
        {
        { "Id", "شناسه دارایی" },
        { "RequestId", "شناسه درخواست" },
        { "AssetTypeId", "شناسه نوع دارایی" },
        { "AssetValue", "ارزش دارایی" },
        { "AssetPercentage", "درصد دارایی" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
        };
        }

        private Dictionary<string, string> GetUserDepositColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه سپرده" },
        { "RequestId", "شناسه درخواست" },
        { "DepositAmount", "مبلغ سپرده" },
        { "DepositDate", "تاریخ سپرده" },
        { "DepositSource", "منبع سپرده" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserDocumentColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه سند" },
        { "RequestId", "شناسه درخواست" },
        { "DocumentType", "نوع سند" },
        { "FilePath", "مسیر فایل" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserFinancialChangesColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه تغییر مالی" },
        { "RequestId", "شناسه درخواست" },
        { "Description", "توضیحات" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserFuturePlansColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه طرح های آینده" },
        { "RequestId", "شناسه درخواست" },
        { "Description", "توضیحات" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserInvestmentColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه سرمایه گذاری" },
        { "RequestId", "شناسه درخواست" },
        { "Amount", "مقدار" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserInvestmentExperienceColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه تجربه سرمایه گذاری" },
        { "RequestId", "شناسه درخواست" },
        { "InvestmentType", "نوع سرمایه گذاری" },
        { "InvestmentAmount", "مقدار سرمایه گذاری" },
        { "InvestmentDurationMonths", "مدت زمان سرمایه گذاری (ماه)" },
        { "ProfitLossAmount", "مقدار سود/زیان" },
        { "ProfitLossDescription", "توضیحات سود/زیان" },
        { "ConversionReason", "دلیل تغییر" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserMoreInformationColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه اطلاعات بیشتر" },
        { "RequestId", "شناسه درخواست" },
        { "Info", "اطلاعات" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserWithdrawalColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه برداشت" },
        { "RequestId", "شناسه درخواست" },
        { "WithdrawalAmount", "مقدار برداشت" },
        { "WithdrawalDate", "تاریخ برداشت" },
        { "WithdrawalReason", "دلیل برداشت" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetUserTestScoreColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه نمره آزمون" },
        { "RequestId", "شناسه درخواست" },
        { "QuizScore", "نمره آزمون" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHaghighiUserBankInfoColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه اطلاعات بانکی" },
        { "RequestId", "شناسه درخواست" },
        { "TradeCode", "کد معاملاتی" },
        { "SejamCode", "کد سجام" },
        { "BankName", "نام بانک" },
        { "BranchCode", "کد شعبه" },
        { "BranchName", "نام شعبه" },
        { "BranchCity", "نام شهر محل شعبه" },
        { "AccountNumber", "شماره حساب" },
        { "IBAN", "شماره شبا" },
        { "AccountType", "نوع حساب" },
        { "CapitalAmount", "میزان سرمایه" },
        { "CapitalType", "نوع سرمایه" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHaghighiUserEducationStatusColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه وضعیت تحصیلی" },
        { "RequestId", "شناسه درخواست" },
        { "LastDegree", "آخرین مدرک تحصیلی" },
        { "FieldOfStudy", "رشته تحصیلی" },
        { "GraduationYear", "سال فارغ‌التحصیلی" },
        { "IssuingAuthority", "مرجع صادر کننده" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHaghighiUserFinancialProfileColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه پروفایل مالی" },
        { "RequestId", "شناسه درخواست" },
        { "MainContinuousIncome", "درآمد اصلی مستمر" },
        { "OtherIncomes", "سایر درآمدها" },
        { "SupportFromOthers", "حمایت از دیگران" },
        { "ContinuousExpenses", "هزینه‌های مستمر" },
        { "OccasionalExpenses", "هزینه‌های گاه‌به‌گاه" },
        { "ContributionToOthers", "کمک به دیگران" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHaghighiUserRelationshipsColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه روابط" },
        { "RequestId", "شناسه درخواست" },
        { "FullName", "نام کامل" },
        { "RelationshipStatus", "وضعیت رابطه" },
        { "BirthYear", "سال تولد" },
        { "EducationLevel", "سطح تحصیلات" },
        { "EmploymentStatus", "وضعیت شغلی" },
        { "AverageMonthlyIncome", "درآمد ماهانه متوسط" },
        { "AverageMonthlyExpense", "هزینه ماهانه متوسط" },
        { "ApproximateAssets", "دارایی‌های تقریبی" },
        { "ApproximateLiabilities", "بدهی‌های تقریبی" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHaghighiUserProfileColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه پروفایل" },
        { "RequestId", "شناسه درخواست" },
        { "FirstName", "نام" },
        { "LastName", "نام خانوادگی" },
        { "FathersName", "نام پدر" },
        { "NationalNumber", "شماره ملی" },
        { "BirthDate", "تاریخ تولد" },
        { "BirthPlace", "محل تولد" },
        { "BirthCertificateNumber", "شماره شناسنامه" },
        { "MaritalStatus", "وضعیت تاهل" },
        { "Gender", "جنسیت" },
        { "PostalCode", "کد پستی" },
        { "HomePhone", "تلفن ثابت" },
        { "Fax", "فکس" },
        { "BestTimeToCall", "بهترین زمان برای تماس" },
        { "ResidentialAddress", "آدرس محل سکونت" },
        { "Email", "ایمیل" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHaghighiUserEmploymentHistoryColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه سوابق شغلی" },
        { "RequestId", "شناسه درخواست" },
        { "EmployerLocation", "محل کارفرما" },
        { "MainActivity", "فعالیت اصلی" },
        { "Position", "سمت" },
        { "StartDate", "تاریخ شروع" },
        { "EndDate", "تاریخ پایان" },
        { "WorkAddress", "آدرس محل کار" },
        { "WorkPhone", "تلفن محل کار" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHoghooghiUserBoardOfDirectorsColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه اعضای هیئت مدیره" },
        { "RequestId", "شناسه درخواست" },
        { "FullName", "نام کامل" },
        { "Position", "سمت" },
        { "EducationalLevel", "سطح تحصیلات" },
        { "FieldOfStudy", "رشته تحصیلی" },
        { "ExecutiveExperience", "تجربه اجرایی" },
        { "FamiliarityWithCapitalMarket", "آشنایی با بازار سرمایه" },
        { "PersonalInvestmentExperienceInStockExchange", "تجربه سرمایه‌گذاری شخصی در بورس" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHoghooghiUserCompaniesWithMajorInvestorsColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه شرکت‌ها با سرمایه‌گذاران عمده" },
        { "RequestId", "شناسه درخواست" },
        { "CompanyName", "نام شرکت" },
        { "CompanySubject", "موضوع شرکت" },
        { "PercentageOfTotal", "درصد از کل" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHoghooghiUserInvestmentDepartmentStaffColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه پرسنل دپارتمان سرمایه‌گذاری" },
        { "RequestId", "شناسه درخواست" },
        { "FullName", "نام کامل" },
        { "Position", "سمت" },
        { "EducationalLevel", "سطح تحصیلات" },
        { "FieldOfStudy", "رشته تحصیلی" },
        { "ExecutiveExperience", "تجربه اجرایی" },
        { "FamiliarityWithCapitalMarket", "آشنایی با بازار سرمایه" },
        { "PersonalInvestmentExperienceInStockExchange", "تجربه سرمایه‌گذاری شخصی در بورس" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHoghooghiUserColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه حقوقی" },
        { "RequestId", "شناسه درخواست" },
        { "Name", "نام" },
        { "RegistrationNumber", "شماره ثبت" },
        { "RegistrationDate", "تاریخ ثبت" },
        { "RegistrationLocation", "محل ثبت" },
        { "NationalId", "شناسه ملی" },
        { "MainActivityBasedOnCharter", "فعالیت اصلی بر اساس اساسنامه" },
        { "MainActivityBasedOnPastThreeYearsPerformance", "فعالیت اصلی بر اساس عملکرد سه سال گذشته" },
        { "PostalCode", "کد پستی" },
        { "LandlinePhone", "تلفن ثابت" },
        { "Fax", "فکس" },
        { "BestTimeToCall", "بهترین زمان برای تماس" },
        { "Address", "آدرس" },
        { "Email", "ایمیل" },
        { "RepresentativeName", "نام نماینده" },
        { "RepresentativeNationalId", "شناسه ملی نماینده" },
        { "RepresentativeMobilePhone", "تلفن همراه نماینده" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        private Dictionary<string, string> GetHoghooghiUserAssetIncomeStatusTwoYearsAgoColumnMappings()
        {
            return new Dictionary<string, string>
    {
        { "Id", "شناسه وضعیت دارایی و درآمد دو سال پیش" },
        { "RequestId", "شناسه درخواست" },
        { "FiscalYear", "سال مالی" },
        { "RegisteredCapital", "سرمایه ثبت‌شده" },
        { "ApproximateAssetValue", "ارزش تقریبی دارایی" },
        { "TotalLiabilities", "کل بدهی‌ها" },
        { "TotalInvestments", "کل سرمایه‌گذاری‌ها" },
        { "OperationalIncome", "درآمد عملیاتی" },
        { "OtherIncome", "درآمدهای دیگر" },
        { "OperationalExpenses", "هزینه‌های عملیاتی" },
        { "OtherExpenses", "هزینه‌های دیگر" },
        { "OperationalProfitOrLoss", "سود یا زیان عملیاتی" },
        { "NetProfitOrLoss", "سود یا زیان خالص" },
        { "AccumulatedProfitOrLoss", "سود یا زیان انباشته" },
        { "IsComplete", "کامل شده" },
        { "IsDeleted", "حذف شده" }
    };
        }

        //[HttpGet("{id}")]
        //public IActionResult GenerateReport(int id)
        //{
        //    var userProfile = _context.HaghighiUserProfiles.Find(id);
        //    if (userProfile == null)
        //    {
        //        return NotFound();
        //    }

        //    var relationships = _context.HaghighiUserRelationships
        //      .Where(r => r.RequestId == userProfile.RequestId && !r.IsDeleted)
        //      .ToList();

        //    //var financialProfiles = _context.HaghighiUserProfiles.Find(1);

        //    var financialProfiles = _context.HaghighiUserFinancialProfiles
        //        .Where(fp => fp.RequestId == userProfile.RequestId && !fp.IsDeleted)
        //        .ToList();

        //    //var templatePath = "D:\\MMNazari\\Documents\\Novin\\PRX Docs\\فرم_كسب_اطلاعات_مشتريان_و_پرسشنامه_ارزيابي_توان_مشتري_حقیقی.docx";
        //    //var outputPath = $"output-{id}.docx";

        //    var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "D:\\MMNazari\\Documents\\Novin\\PRX Docs\\فرم_كسب_اطلاعات_مشتريان_و_پرسشنامه_ارزيابي_توان_مشتري_حقیقی.docx");
        //    var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
        //    Directory.CreateDirectory(outputDirectory);  // Ensure the output directory exists
        //    var outputPath = Path.Combine(outputDirectory, $"output-{id}.docx");

        //    // Read the template into a memory stream
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
        //        {
        //            templateStream.CopyTo(memoryStream);
        //        }

        //        memoryStream.Position = 0; // Reset stream position

        //        // Open the document from the memory stream
        //        using (var wordDoc = WordprocessingDocument.Open(memoryStream, true))
        //        {
        //            var body = wordDoc.MainDocumentPart.Document.Body;

        //            ReplacePlaceholder(body, "نام:", userProfile.FirstName);
        //            ReplacePlaceholder(body, "نام خانوادگی:", userProfile.LastName);
        //            ReplacePlaceholder(body, "نام پدر:", userProfile.FathersName);
        //            ReplacePlaceholder(body, "کدملی:", userProfile.NationalNumber);
        //            ReplacePlaceholder(body, "تاریخ تولد:", userProfile.BirthDate.ToString("yyyy-MM-dd"));
        //            ReplacePlaceholder(body, "محل صدور:", userProfile.BirthPlace);
        //            ReplacePlaceholder(body, "شماره شناسنامه:", userProfile.BirthCertificateNumber);
        //            ReplacePlaceholder(body, "وضعیت تأهل:", userProfile.MaritalStatus);
        //            ReplacePlaceholder(body, "جنسیت:", userProfile.Gender);
        //            ReplacePlaceholder(body, "کدپستی:", userProfile.PostalCode.ToString());
        //            ReplacePlaceholder(body, "تلفن همراه:", userProfile.HomePhone);
        //            ReplacePlaceholder(body, "تلفن منزل:", userProfile.HomePhone);  // Assuming HomePhone for "تلفن منزل"
        //            ReplacePlaceholder(body, "دورنگار:", userProfile.Fax);
        //            ReplacePlaceholder(body, "بهترین زمان برای تماس تلفنی:", userProfile.BestTimeToCall);
        //            ReplacePlaceholder(body, "نشانی محل سکونت:", userProfile.ResidentialAddress);
        //            ReplacePlaceholder(body, "پست الکترونیک:", userProfile.Email);

        //            // Fill the relationships table
        //            //FillRelationshipsTable(body, relationships);

        //            //FillFinancialProfileTable(body, financialProfiles);

        //            wordDoc.MainDocumentPart.Document.Save();


        //        }

        //        // Save the modified document to the output stream
        //        memoryStream.Position = 0; // Reset stream position before saving to output stream
        //        using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //        {
        //            memoryStream.CopyTo(outputStream);
        //        }
        //    }

        //    //using (var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
        //    //using (var outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
        //    //{
        //    //    using (var wordDoc = WordprocessingDocument.Open(templateStream, false))
        //    //    using (var outputDoc = WordprocessingDocument.Create(outputStream, wordDoc.DocumentType))
        //    //    {
        //    //        // Copy the template parts into the new document
        //    //        foreach (var part in wordDoc.Parts)
        //    //        {
        //    //            outputDoc.AddPart(part.OpenXmlPart, part.RelationshipId);
        //    //        }

        //    //        var body = outputDoc.MainDocumentPart.Document.Body;

        //    //        ReplacePlaceholder(body, "نام:", userProfile.FirstName);
        //    //        ReplacePlaceholder(body, "نام خانوادگی:", userProfile.LastName);
        //    //        ReplacePlaceholder(body, "نام پدر:", userProfile.FathersName);
        //    //        ReplacePlaceholder(body, "کدملی:", userProfile.NationalNumber);
        //    //        ReplacePlaceholder(body, "تاریخ تولد:", userProfile.BirthDate.ToString("yyyy-MM-dd"));
        //    //        ReplacePlaceholder(body, "محل صدور:", userProfile.BirthPlace);
        //    //        ReplacePlaceholder(body, "شماره شناسنامه:", userProfile.BirthCertificateNumber);
        //    //        ReplacePlaceholder(body, "وضعیت تأهل: مجرد متأهل", userProfile.MaritalStatus);
        //    //        ReplacePlaceholder(body, "جنسیت: مرد زن", userProfile.Gender);
        //    //        ReplacePlaceholder(body, "کدپستی:", userProfile.PostalCode.ToString());
        //    //        ReplacePlaceholder(body, "تلفن همراه:", userProfile.HomePhone);
        //    //        ReplacePlaceholder(body, "تلفن منزل:", userProfile.HomePhone);  // Assuming HomePhone for "تلفن منزل"
        //    //        ReplacePlaceholder(body, "دورنگار:", userProfile.Fax);
        //    //        ReplacePlaceholder(body, "بهترین زمان برای تماس تلفنی:", userProfile.BestTimeToCall);
        //    //        ReplacePlaceholder(body, "نشانی محل سکونت:", userProfile.ResidentialAddress);
        //    //        ReplacePlaceholder(body, "پست الکترونیک:", userProfile.Email);

        //    //        outputDoc.MainDocumentPart.Document.Save();
        //    //    }
        //    //}

        //    return PhysicalFile(outputPath, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", outputPath);
        //}


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

        //private void ReplacePlaceholder(Body body, string placeholder, string value)
        //{
        //    foreach (var text in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
        //    {
        //        if (text.Text.Contains(placeholder))
        //        {
        //            var run = text.Parent as Run;
        //            if (run != null)
        //            {
        //                var newRun = run.CloneNode(true) as Run;
        //                if (newRun != null)
        //                {
        //                    var newTextElement = newRun.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.Text>();

        //                    // Ensure newTextElement is not null
        //                    if (newTextElement == null)
        //                    {
        //                        newTextElement = new DocumentFormat.OpenXml.Wordprocessing.Text();
        //                        newRun.AppendChild(newTextElement);
        //                    }

        //                    newTextElement.Text = value;

        //                    // Append the new run after the current run
        //                    run.Parent.InsertAfter(newRun, run);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void FillRelationshipsTable(Body body, List<HaghighiUserRelationships> relationships)
        //{
        //    // Locate the table using the appropriate method (finding by position or by specific characteristics)
        //    var table = body.Descendants<Table>().FirstOrDefault();

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
        //        for (int i = 0; i < relationships.Count; i++)
        //        {
        //            var relationship = relationships[i];

        //            // Create a new row
        //            var tableRow = new TableRow();

        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text((i + 1).ToString())))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.FullName)))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.RelationshipStatus)))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.BirthYear.ToString())))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.EducationLevel)))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.EmploymentStatus)))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.AverageMonthlyIncome.ToString("N0"))))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.AverageMonthlyExpense.ToString("N0"))))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.ApproximateAssets.ToString("N0"))))));
        //            tableRow.Append(new TableCell(new Paragraph(new Run(new DocumentFormat.OpenXml.Drawing.Text(relationship.ApproximateLiabilities.ToString("N0"))))));

        //            // Append the row to the table
        //            table.Append(tableRow);
        //        }
        //    }
        //}

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

        //private void FillFinancialProfileTable(string templatePath, string outputPath, List<HaghighiUserFinancialProfile> financialProfiles)
        //{
        //    try
        //    {
        //        // Load the document
        //        using (DocX document = DocX.Load(templatePath))
        //        {
        //            // Find the table by searching for a row with specific text in the first cell
        //            var table = document.Tables.FirstOrDefault(t =>
        //            {
        //                var firstRow = t.Rows.FirstOrDefault();
        //                if (firstRow != null)
        //                {
        //                    var firstCellText = firstRow.Cells.FirstOrDefault()?.Paragraphs.FirstOrDefault()?.Text?.Trim();
        //                    if (firstCellText != null && firstCellText.Contains("مبلغ درآمد اصلی و مستمر"))
        //                    {
        //                        return true;
        //                    }
        //                }
        //                return false;
        //            });

        //            if (table != null)
        //            {
        //                // Clear existing rows except header
        //                var rowsToRemove = table.Rows.Skip(1).ToList();
        //                foreach (var row in rowsToRemove)
        //                {
        //                    row.Remove();
        //                }

        //                // Populate the table with data from financial profiles
        //                foreach (var profile in financialProfiles)
        //                {
        //                    var tableRow = table.InsertRow();

        //                    tableRow.Cells[0].Paragraphs.First().Append(profile.MainContinuousIncome.ToString("N0"));
        //                    tableRow.Cells[1].Paragraphs.First().Append(profile.OtherIncomes.ToString("N0"));
        //                    tableRow.Cells[2].Paragraphs.First().Append(profile.SupportFromOthers.ToString("N0"));
        //                    tableRow.Cells[3].Paragraphs.First().Append(profile.ContinuousExpenses.ToString("N0"));
        //                    tableRow.Cells[4].Paragraphs.First().Append(profile.OccasionalExpenses.ToString("N0"));
        //                    tableRow.Cells[5].Paragraphs.First().Append(profile.ContributionToOthers.ToString("N0"));
        //                    // Add more cells as needed
        //                }

        //                // Save the document
        //                document.SaveAs(outputPath);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Financial profile table not found in the document.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //}
    }
}
