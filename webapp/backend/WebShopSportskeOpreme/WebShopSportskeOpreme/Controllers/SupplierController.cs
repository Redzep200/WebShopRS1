using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShopSportskeOpreme.Interfaces;
using WebShopSportskeOpreme.Models;
using WebShopSportskeOpreme.Viewmodels;

namespace WebShopSportskeOpreme.Controllers
{
    [Authorize(Roles = "Web shop admin, SistemAdmin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet]
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _supplierService.GetAllSuppliers();
            return Ok(suppliers);
        }
        [HttpGet]
        public IActionResult GetSuppliersById(int id) 
        { 
         var supplier = _supplierService.GetSupplierById(id);
            if (supplier == null)
                return NotFound("Ovaj dobavljač ne postoji!");
            return Ok(supplier);
        }

        [HttpGet]
        public IActionResult GetSuppliersByCity(int cityId)
        {
            var suppliers = _supplierService.GetSuppliersByCity(cityId);
            if (suppliers == null || !suppliers.Any())
                return NotFound("Nema dobavljača za ovaj grad.");
            return Ok(suppliers);
        }

        [HttpPost]
        public IActionResult CreateSupplier(AddSuppliersVM model)
        {
            if (ModelState.IsValid)
            {
                var supplier = _supplierService.CreateSupplier(new Supplier
                {
                    CityId = model.CityId,
                    Name = model.Name,
                    Adress = model.Adress,
                    ContactPhone = model.ContactPhone,
                    Email = model.Email
                });

                if(!supplier)
                    return BadRequest("Greška u dodavanju dobavljača!");

                return Ok(new { success = true, message = "Uspješno dodana dobavljač" });
            }
            return BadRequest(ModelState);
        }
        [HttpDelete]
        public IActionResult DeleteSupplier(int id) 
        {
           var status = _supplierService.DeleteSupplier(id);
            if (status == false) return NotFound("Ne postoji dobavljač kojeg želite obrisati!");
            return Ok(new { success = true, message = "Uspjesno obrisan dobavljač" });
        }
        [HttpPut]
        public IActionResult UpdateSupplier(int id,int cityId, string supplierName, string adress, string phone, string email)
        {
            var status = _supplierService.UpdateSupplier(id, new Supplier { 
            Adress= adress,
            ContactPhone = phone,
            Email = email,
            Name = supplierName,
            CityId = cityId
            });
            if (status == false) return BadRequest("Ne postoji dobavljač kojeg želite ažurirati!");
            return Ok(new { success = true, message = "Uspjesno editovan dobavljač" });
        }
    }
}
