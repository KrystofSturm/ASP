using Microsoft.AspNetCore.Mvc;
using Poznamky.Data;
using Poznamky.Models;
using System.Security.Cryptography.Xml;

namespace Poznamky.Controllers
{
    public class PoznamkaController : Controller
    {
        PoznamkyData Databaze {  get; set; }

        public PoznamkaController(PoznamkyData databaze) => Databaze = databaze;

        [HttpGet]
        public IActionResult Vypis()
        {
            int? UserID = HttpContext.Session.GetInt32("ID");
            if(UserID == null)
            {
                return RedirectToAction("Index", "Home");
            }

            UzivatelModel? userVerify = Databaze.Uzivatele.FirstOrDefault(user => user.Id == UserID);
            if(userVerify == null || userVerify.Id != UserID)
            {
                return RedirectToAction("Index", "Home");
            }

            List<PoznamkaModel> poznamky = Databaze.Poznamky.Where(poznaka => poznaka.UserID == UserID).ToList();
            poznamky.Sort((x, y) => x.Vlozeno.CompareTo(y.Vlozeno));


            return View(poznamky);
        }


        [HttpGet]
        public IActionResult Pridat()
        {
            int? userId = HttpContext.Session.GetInt32("ID");

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Pridat(string nadpis, string body)
        {
            int? userId = HttpContext.Session.GetInt32("ID");

            if(userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            PoznamkaModel poznamka = new PoznamkaModel
            {
                UserID = (int)userId,
                Title = nadpis,
                Body = body,
                Vlozeno = DateTime.Now
            };

            Databaze.Poznamky.Add(poznamka);
            Databaze.SaveChanges();

            return RedirectToAction("Vypis");
        }


        [HttpGet]
        public IActionResult Dulezita(int ID)
        {
            PoznamkaModel? poznamka = Databaze.Poznamky.Where(x => x.Id == ID).FirstOrDefault();
            if (poznamka == null)
            {
                return RedirectToAction("Vypis");
            }
            poznamka.Dulezita = !poznamka.Dulezita;
            Databaze.Update(poznamka);
            Databaze.SaveChanges();

            return RedirectToAction("Vypis");
        }

        [HttpGet]
        public IActionResult Smazat(int ID)
        {
            PoznamkaModel? poznamka = Databaze.Poznamky.Where(x => x.Id == ID).FirstOrDefault();
            if (poznamka == null)
            {
                return RedirectToAction("Vypis");
            }

            Databaze.Remove(poznamka);
            Databaze.SaveChanges();

            return RedirectToAction("Vypis");
        }
    }
}
