using Microsoft.AspNetCore.Mvc;
using Poznamky.Data;
using System.Diagnostics;
using BCrypt;
using Poznamky.Models;
using Microsoft.EntityFrameworkCore;


namespace Poznamky.Controllers
{
    public class UzivatelController : Controller
    {
        PoznamkyData Databaze { get; set; }

        public UzivatelController(PoznamkyData databaze) => Databaze = databaze;

        [HttpGet]
        public IActionResult registrace(string chyba = "")
        {
            return View();
        }

        [HttpPost]
        public IActionResult registrace(string? username, string? heslo, string? hesloConfirm, string? souhlas)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("registrace", new { chyba = "Nebylo zadano uživatelské jméno"});
            }
            if (string.IsNullOrEmpty(souhlas))
            {
                return RedirectToAction("registrace", new { chyba = "Nebyl poskytnut souhlas"});
            }
            if (string.IsNullOrEmpty(heslo) || string.IsNullOrEmpty(hesloConfirm))
            {
                return RedirectToAction("registrace", new { chyba = "Nebylo zadano heslo nebo potvrzeni hesla" });
            }
            if (heslo != hesloConfirm)
            {
                return RedirectToAction("registrace", new { chyba = "Hesla se neshoduji" });
            }

            UzivatelModel? uzivatelTest = Databaze.Uzivatele.FirstOrDefault(user => user.UserName == username);

            if (uzivatelTest != null)
            {
                return RedirectToAction("prihlaseni", new { chyba = "Tento uzivatel jiz existuje" });
            }

            UzivatelModel uzivatel = new UzivatelModel
            {
                UserName = username,
                Agreed = true,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(heslo)
            };

            Databaze.Uzivatele.Add(uzivatel);
            Databaze.SaveChanges();

            return RedirectToAction("prihlaseni");
        }



        [HttpGet]
        public IActionResult prihlaseni(string chyba = "") 
        {
            return View();
        }

        [HttpPost]
        public IActionResult prihlaseni(string username, string heslo)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("prihlaseni", new { chyba = "Nebylo zadano uživatelské jméno" });
            }

            UzivatelModel? uzivatel = Databaze.Uzivatele.FirstOrDefault(user => user.UserName == username);

            if(uzivatel == null)
            {
                return RedirectToAction("prihlaseni", new { chyba = "Tento uzivatel neexistuje" });
            }

            if(!BCrypt.Net.BCrypt.Verify(heslo, uzivatel.PasswordHash))
            {
                return RedirectToAction("prihlaseni", new { chyba = "Spatne heslo" });
            }

            HttpContext.Session.SetInt32("ID", uzivatel.Id);

            return RedirectToAction("Vypis", "Poznamka");
        }

        public IActionResult odhlaseni()
        {
            HttpContext.Session.Remove("ID");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult smazat(string chyba = "")
        {
            if (HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult smazat(string heslo, bool check)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if (userID == null || HttpContext.Session.GetInt32("ID") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            UzivatelModel? uzivatel = Databaze.Uzivatele.FirstOrDefault(user => user.Id == userID);
            if (uzivatel == null)
            {
                return RedirectToAction("odhlaseni");
            }
            if (!BCrypt.Net.BCrypt.Verify(heslo, uzivatel.PasswordHash))
            {
                return RedirectToAction("smazat", new { chyba = "Spatne heslo" });
            }

            Databaze.Uzivatele.Remove(uzivatel);
            Databaze.Poznamky.RemoveRange(Databaze.Poznamky.Where(poznamka => poznamka.UserID == userID));
            Databaze.SaveChanges();
            return RedirectToAction("odhlaseni");
        }

    }
}
