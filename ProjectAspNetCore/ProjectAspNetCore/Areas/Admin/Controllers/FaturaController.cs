using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using ProjectAspNetCore.Areas.Admin.Models;
using ProjectAspNetCore.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FaturaController : Controller
    {
        public IActionResult ExportStaticExcelFaturaList()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Fatura Listesi");
                worksheet.Cell(1, 1).Value = "Fatura Id";
                worksheet.Cell(1, 2).Value = "Fatura Dönemi";
                worksheet.Cell(1, 3).Value = "Fatura Tutarı";
                worksheet.Cell(1, 4).Value = "Fatura Ödendi mi?";
                worksheet.Cell(1, 5).Value = "Açıklama";

                int FaturaRowCount = 2;

                foreach (var item in GetFaturaList())
                {
                    worksheet.Cell(FaturaRowCount, 1).Value = item.FaturaId;
                    worksheet.Cell(FaturaRowCount, 2).Value = item.FaturaDonemi;
                    worksheet.Cell(FaturaRowCount, 3).Value = item.FaturaTutar;
                    worksheet.Cell(FaturaRowCount, 4).Value = item.FaturaOdendiMi;
                    worksheet.Cell(FaturaRowCount, 5).Value = item.FaturaAciklama;
                    FaturaRowCount++;
                }

                using(var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","Fatura.xlsx");
                }
            }
           
        }
        public List<FaturaModel> GetFaturaList()
        {
            List<FaturaModel> fm = new List<FaturaModel>
            {
                new FaturaModel{FaturaId=1,FaturaDonemi="Ocak"},
                new FaturaModel{FaturaId=2,FaturaAciklama="Türk Telekom" },
                new FaturaModel{FaturaId=3,FaturaAciklama="TT" },
                new FaturaModel{FaturaId=4,FaturaAciklama="tÜRKt"},
                new FaturaModel{FaturaId=5,FaturaAciklama="ADDAD" }

             };
            return fm;
        }
        public IActionResult FaturaListExcel()
        {
            return View();
        }
        public IActionResult ExportDynamicExcelFaturaList()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Fatura Listesi");
                worksheet.Cell(1, 1).Value = "Fatura Id";
                worksheet.Cell(1, 2).Value = "Fatura Sahibi Id";
                worksheet.Cell(1, 3).Value = "Fatura Dönemi";
                worksheet.Cell(1, 4).Value = "Fatura Tutarı";
                worksheet.Cell(1, 5).Value = "Fatura Ödendi mi?";
                worksheet.Cell(1, 6).Value = "Açıklama";

                int FaturaRowCount = 2;

                foreach (var item in FaturaTitleList())
                {
                    worksheet.Cell(FaturaRowCount, 1).Value = item.FatId;
                    worksheet.Cell(FaturaRowCount, 2).Value = item.FatSahibi;
                    worksheet.Cell(FaturaRowCount, 3).Value = item.FatDonemi;
                    worksheet.Cell(FaturaRowCount, 4).Value = item.FatTutar + "TL";
                    worksheet.Cell(FaturaRowCount, 5).Value = item.FatOdendiMi;
                    worksheet.Cell(FaturaRowCount, 6).Value = item.FatAciklama;
                    FaturaRowCount++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Fatura.xlsx");
                }
            }
        }
        public List<FaturaModel2> FaturaTitleList()
        {
            List<FaturaModel2> fm = new List<FaturaModel2>();
            using(var c = new ProjectContext())
            {
                fm = c.FaturaBilgiler.Select(x => new FaturaModel2
                {
                    FatId=x.Id,
                    FatSahibi= (int)x.CustomerId,
                    FatDonemi=x.Ad,
                    FatTutar=x.Fiyat,
                    FatOdendiMi=x.FaturaOdendiMi,
                    FatAciklama=x.Aciklama
                  
                }).ToList();
            }
            return fm;
        }
        public IActionResult FaturaTitleListExcel()
        {
            return View();
        }
    }
}
