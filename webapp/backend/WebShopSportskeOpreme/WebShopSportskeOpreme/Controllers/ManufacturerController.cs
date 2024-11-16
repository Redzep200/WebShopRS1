using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Services;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;
        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService; 
        }
        [HttpGet]

        public IActionResult GetAllManufacturers()
        {
            var manufacturers = _manufacturerService.GetAllManufacturers();
            return Ok(manufacturers);
        }
        [HttpGet]
        public IActionResult GetManufacturerById(int id)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(id);
            if (manufacturer == null)
                return NotFound("Proizvođač nije pronađen!");
            return Ok(manufacturer);
        }
        [HttpGet]
        public IActionResult GetManufacturerByName(string name)
        {
            var manufacturer = _manufacturerService.GetManufacturerByName(name);
            if (manufacturer == null) return NotFound("Proizvođač nije pronađen!");
                    return Ok(manufacturer);
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPost]
        public IActionResult AddManufacturer(AddManufacturerVM model)
        {
            if (ModelState.IsValid)
            {
                var manufacturer = _manufacturerService.CreateManufacturer(new Manufacturer
                {
                    Name = model.Name,
                    Address = model.Address,
                    Email = model.Email
                });

                if (!manufacturer)
                    return BadRequest("Greška u dodavanju proizvođača!");

                return Ok(new { success = true, message = "Uspješno dodan proizvođač" });
            }

            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpDelete]
        public IActionResult DeleteManufacturer(int id)
        {
            var status = _manufacturerService.DeleteManufacturer(id);
            if (status == false) return NotFound("Proizvođač sa ID " + id + " ne postoji!");
            return Ok(new { success = true, message = "Uspjesno obrisan proizvođač" });
        }

        [Authorize(Roles = "Web shop admin, SistemAdmin")]
        [HttpPut]
        public IActionResult UpdateManufacturer(int id, string name, string address, string email)
        {
            var status = _manufacturerService.UpdateManufacturer(id, new Models.Manufacturer { Name = name, Address = address,Email= email });
            if (status == false) return NotFound("Proizvođač nije pronađen!");
            return Ok(new { success = true, message = "Uspjesno editovan proizvođač" });
        }
    }
}
