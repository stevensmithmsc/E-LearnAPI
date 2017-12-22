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
using System.Web.Http.Cors;
using E_LearnAPI.Models;
using E_LearnAPI.DTOs;

namespace E_LearnAPI.Controllers
{
    /// <summary>
    /// Handles messages regarding E-Laerning Results
    /// </summary>
    public class ELResultsController : ApiController
    {
        private TrainingDatabase db = new TrainingDatabase();

        /// <summary>
        /// Gets a list of E-Learning Results from the database.
        /// </summary>
        /// <param name="search">Optional String Parameter, will search Person Name for any name containing parameter, if included.</param>
        /// <param name="processed">Optional Boolean Parameter, will search the Processed field for matching results, if included.</param>
        /// <returns>List of upto 200 E-Laerning Results.</returns>
        [Authorize]
        // GET: api/ELResults
        public IQueryable<ELResult> GetELResults(string search = null, bool? processed = null)
        {
            var result = (IQueryable<ELResult>)db.ELResults;

            if (search != null)
            {
                result = result.Where(r => r.PersonName.Contains(search));
            }

            if (processed != null)
            {
                result = result.Where(r => r.Processed == processed);
            }

            return result.Take(200);
        }

        /// <summary>
        /// Gets a single E-Learning result from the database.
        /// </summary>
        /// <param name="id">The ID from the database of the E-Learning Result.</param>
        /// <returns>The matching E-Learning result or Not Found (404)</returns>
        [Authorize]
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

        /// <summary>
        /// Updates the processed and comments fields of an E-Learning result record, then
        /// attempts to process the result if processed field is false.
        /// </summary>
        /// <param name="id">This is the ID field of the E-Learning Result from the database.</param>
        /// <param name="resultDTO">Contains the Comments and Processed field of the E-Learning Result</param>
        /// <returns>A http code (204 if successful)</returns>
        [Authorize]
        // PUT: api/ELResults/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutELResult(int id, ResultUpdateDto resultDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != resultDTO.Id)
            {
                return BadRequest();
            }

            //db.Entry(eLResult).State = EntityState.Modified;
            ELResult eLResult = await db.ELResults.FindAsync(id);
            if (eLResult == null)
            {
                return NotFound();
            }

            eLResult.Processed = resultDTO.Processed;
            eLResult.Comments = resultDTO.Comments;

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

            if (!eLResult.Processed)
            {
                await ProcessResultAsync(eLResult);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Receives an E-Learning Result, will populate Received, FromADAcc and Processed fields
        /// and then attempt to process the E-Learning result before saving it to the database.
        /// Uses CORS, will accept messages from anywhere.
        /// </summary>
        /// <param name="eLResult">This is the E-Learning Result</param>
        /// <returns>The e-laerning result with additional fields populated.</returns>
        // POST: api/ELResults
        [ResponseType(typeof(ELResult))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
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

            await ProcessResultAsync(eLResult);

            return CreatedAtRoute("DefaultApi", new { id = eLResult.Id }, eLResult);
        }

        /// <summary>
        /// Deletes a E-Learning result from the database.
        /// </summary>
        /// <param name="id">The ID of the E-Learning result to be deleted.</param>
        /// <returns>The deleted E-Learning result/</returns>
        [Authorize]
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

        /// <summary>
        /// Attempts to process the E-learning result and update the associated requirement if found.
        /// Updated to processed field to true if sucessful, otherwise will update the comment field
        /// to reflect the reason the processing failed.
        /// </summary>
        /// <param name="eLResult">The E-Learning Result</param>
        /// <returns>nothing</returns>
        private async Task ProcessResultAsync(ELResult eLResult)
        {
            //Find Person
            var person = await db.People.SingleOrDefaultAsync(p => p.ESRID == eLResult.PersonId);
            if (person == null)
            {
                eLResult.Comments = "Unable to find staff member with matching ID";
                await db.SaveChangesAsync();
                return;
            }
            //Find Course
            var course = await db.Courses.SingleOrDefaultAsync(c => c.ID == eLResult.CourseId);
            if (course == null)
            {
                eLResult.Comments = "Unable to find course with matching ID";
                await db.SaveChangesAsync();
                return;
            }
            //Find Requirement
            var req = await db.Requirements.SingleOrDefaultAsync(r => r.Staff == person.ID && r.Course == eLResult.CourseId);
            if (req == null)
            {
                eLResult.Comments = "Staff member does not have requirement for course.";
                await db.SaveChangesAsync();
                return;
            }
            //Update Requirement
            if (eLResult.PassFail != null)
            {
                req.Status = (bool)(eLResult.PassFail) ? (short)5 : (short)2;

                //Puts additional information in requirement comment.
                //if ((bool)(eLResult.PassFail))
                //{
                //    req.Status = (short)5;
                //    req.Comments = "Passed: " + eLResult.Received.ToShortDateString();
                //}
                //else
                //{
                //    int attempts = await db.ELResults.Where(r => r.PersonId == person.ESRID && r.CourseId == course.ID && r.PassFail == false).CountAsync();
                //    req.Status = (short)2;
                //    req.Comments = "Number of Attempts: " + attempts.ToString();
                //}
            }
            else
            {
                eLResult.Comments = "No Pass or Fail included in message";
                await db.SaveChangesAsync();
                return;
            }
            //Update Result
            eLResult.Processed = true;
            await db.SaveChangesAsync();
        }

        private bool ELResultExists(int id)
        {
            return db.ELResults.Count(e => e.Id == id) > 0;
        }
    }
}