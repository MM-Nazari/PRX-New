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
using PRX.Utils;

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
                var haghighiUserBankInfoSheet = package.Workbook.Worksheets.Add("اطلاعات بانکی کاربر ");
                var haghighiUserBankInfoData = await _context.UserBankInfos.ToListAsync();
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

            // Check if the combination of userId and requestId exists in the Requests table
            var requestExists = await _context.Requests
                .AnyAsync(r => r.UserId == userId && r.Id == requestId);

            if (!requestExists)
            {
                return NotFound(new { Message = ResponseMessages.RequestForUserNotFound });
            }


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
                var haghighiUserBankInfoSheet = package.Workbook.Worksheets.Add("اطلاعات بانکی کاربر ");
                var haghighiUserBankInfoData = requestId > 0 ? await _context.UserBankInfos.Where(hubi => hubi.RequestId == requestId).ToListAsync() : await _context.UserBankInfos.ToListAsync();
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

    }
}
