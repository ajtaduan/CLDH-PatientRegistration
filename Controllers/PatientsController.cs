using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CLDH.PatientRegistration.Data;
using CLDH.PatientRegistration.Models;

namespace CLDH.PatientRegistration.Controllers
{
    [ApiController]
    [Route("api/patients")]
    [Authorize] // whole controller requires login, no patient data without auth
    public class PatientsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PatientsController(AppDbContext db)
        {
            _db = db;
        }

        // GET /api/patients - list all patients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _db.Patients.ToListAsync();
            return Ok(patients);
        }

        // GET /api/patients/5 - get one patient by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _db.Patients.FindAsync(id);
            if (patient == null) return NotFound(new { message = "Patient not found" });
            return Ok(patient);
        }

        // POST /api/patients - create a new patient
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Patient patient)
        {
            // ModelState checks [Required]/[MaxLength] rules I put on the Patient model
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _db.Patients.Add(patient);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }

        // PUT /api/patient/5 - update an existing patient
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Patient updated)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var patient = await _db.Patients.FindAsync(id);
            if (patient == null) return NotFound(new { message = "Patient not found" });

            // Copy over the bew values onto the existing entity
            patient.FullName = updated.FullName;
            patient.BirthDate = updated.BirthDate;
            patient.Gender = updated.Gender;
            patient.ContactNumber = updated.ContactNumber;
            patient.Address = updated.Address;

            await _db.SaveChangesAsync();
            return Ok(patient);
        }

        // DELETE /api/patient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _db.Patients.FindAsync(id);
            if (patient == null) return NotFound(new { message = "Patient not found" });

            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Patient deleted" });
        }
    }
}