using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("wizyta")]
    [ApiController]
    public class WizytaController : ControllerBase
    {
        public static List<Wizyta> wizyty = [
            new Wizyta(){
                Id = 1,
                dataWizyty = new DateTime(2025,4,10),
                opisWizyty = "kontrola",
                cenaWizyty = 15.0,
                zwierzeNaWizycie = ZwierzeController.Zwierzeta[0]
            }
        ];

        //pobranie listy wizyt danego zwierzecia
        [HttpGet]
        [Route("{zwierze}")]
        public IActionResult GetListaWizytZwierzecia(Zwierze zwierze)
        {
            var listaWizytZwierzecia = wizyty.Where(w => w.zwierzeNaWizycie == zwierze).ToList();

            if (listaWizytZwierzecia.Count == 0)
            {
                return NotFound($"Nie znaleziono zadnej wizyty dla zwierzecia {zwierze}");
            }
            return Ok(listaWizytZwierzecia);
        }

        //dodanie nowej wizyty
        [HttpPost]
        public IActionResult AddNowaWizyta(Wizyta wizyta)
        {
            var nextId = wizyty.Max(z => z.Id) + 1;
            wizyta.Id = nextId;

            wizyty.Add(wizyta);

            return CreatedAtAction(nameof(GetListaWizytZwierzecia), new { Zwierze = wizyta.zwierzeNaWizycie.Imie }, wizyta);

        }
    }
}
