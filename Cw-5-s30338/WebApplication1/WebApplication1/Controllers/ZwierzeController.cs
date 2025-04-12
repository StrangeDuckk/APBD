using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("zwierze")]
public class ZwierzeController : ControllerBase
{
    public static List<Zwierze> Zwierzeta = [
        new Zwierze(){
            Id = 1,
            Imie = "Kropka",
            Kategoria = "Pies",
            Masa = 13,
            KolorSiersci = "Łaciata, czarno-biala"
        }
    ];

    //pobeiranie listy zwierzat
    [HttpGet]//zwierzeta
    public IActionResult GetAllZwierzetas()
    {
        return Ok(Zwierzeta);
    }

    //pobieranie danych konkretnego zwierzecia po id
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetZwierzeById(int id)
    {
        var zwierze = Zwierzeta.FirstOrDefault(z => z.Id == id);

        if (zwierze == null)
        {
            return NotFound();
        }
        return Ok(zwierze);
    }

    //dodawanie zwierzecia
    [HttpPost]
    public IActionResult AddZwierze(Zwierze zwierze)
    {
        var nextId = Zwierzeta.Max(z => z.Id) + 1;

        zwierze.Id = nextId;
        Zwierzeta.Add(zwierze);

        return CreatedAtAction(nameof(GetZwierzeById), new { id = zwierze.Id }, zwierze);
    }

    //edycja danych zwierzecia
    [HttpPost]
    [Route("{id}")]
    public IActionResult ReplaceZwierze(int id, Zwierze zwierze)
    {
        var zwierzeUpdate = Zwierzeta.FirstOrDefault(z=>z.Id == id);

        if (zwierzeUpdate == null)
        {
            return NotFound($"Nie znaleziono zwierzecia o id: {id}");
        }

        zwierzeUpdate.Imie = zwierze.Imie;
        zwierzeUpdate.KolorSiersci = zwierze.KolorSiersci;
        zwierzeUpdate.Kategoria= zwierze.Kategoria;
        zwierzeUpdate.Masa = zwierze.Masa;
        return NoContent();
    }

    //usuwanie zwierzecia
    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteZwierzeById(int id)
    {
        var zwierze = Zwierzeta.FirstOrDefault(z => z.Id == id);

        if(zwierze == null)
        {
            return NotFound($"Nie znaleziono zwierzecia o id: {id}");
        }

        Zwierzeta.Remove(zwierze);
        return NoContent();
    }

    //wyszukiwania wszystkich zwierzat na postawie imienia
    [HttpGet]
    [Route("imie/{imie}")]
    public IActionResult GetZwierzeByImie(string imie)
    {
        var zwierze = Zwierzeta.FirstOrDefault(z => z.Imie == imie);

        if (zwierze == null)
        {
            return NotFound();
        }
        return Ok(zwierze);
    }
}
