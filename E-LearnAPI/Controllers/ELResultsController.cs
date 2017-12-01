using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using E_LearnAPI.Models;

namespace E_LearnAPI.Controllers
{
    public class ELResultsController : ApiController
    {
        private TrainingDatabase db = new TrainingDatabase();

        // GET: api/ELResults
        public IQueryable<ELResult> GetELResults()
        {
            return db.ELResults;
        }

        // GET: api/ELResults/5
        [ResponseType(typeof(ELResult))]
        public async Task<IHttpActionResult> GetELResult(int id)
        {
            ELResult eLResult = await db.ELResults.FindAsync(id);
            if (eLResult == null)
            {
                return NotFound();
            }

            return Ok(eLResult);
        }

        // PUT: api/ELResults/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutELResult(int id, ELResult eLResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eLResult.Id)
            {
                return BadRequest();
            }

            db.Entry(eLResult).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ELResultExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ELResults
        [ResponseType(typeof(ELResult))]
        public async Task<IHttpActionResult> PostELResult(ELResult eLResult)
        {
            eLResult.Received = DateTime.Now;
            eLResult.FromADAcc = User.Identity.Name;
            eLResult.Processed = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ELResults.Add(eLResult);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = eLResult.Id }, eLResult);
        }

        // DELETE: api/ELResults/5
        [ResponseType(typeof(ELResult))]
        public async Task<IHttpActionResult> DeleteELResult(int id)
        {
            ELResult eLResult = await db.ELResults.FindAsync(id);
            if (eLResult == null)
            {
                return NotFound();
            }

            db.ELResults.Remove(eLResult);
            await db.SaveChangesAsync();

            return Ok(eLResult);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void ProcessResult(ELResult eLResult)
        {
            //Find Person

            //Find Course

            //Find Requirement

            //Update Requirement

            //Update Result
        }

        private bool ELResultExists(int id)
        {
            return db.ELResults.Count(e => e.Id == id) > 0;
        }
    }
}