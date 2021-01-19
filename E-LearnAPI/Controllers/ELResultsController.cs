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
        [ResponseType(typeof(IEnumerable<ResultDTO>))]
        public async Task<IHttpActionResult> GetELResults(string search = null, bool? processed = null)
        {
            var result = db.ESRs.AsQueryable();

            //if (search != null)
            //{
            //    result = result.Where(r => r.PersonName.Contains(search));
            //}

            if (processed != null)
            {
                result = result.Where(r => r.Processed == processed);
            }

            var resultList = await result.OrderByDescending(r => r.CompletionDate).Take(200).ToListAsync();

            return Ok(resultList.Select(e => new ResultDTO(e)));
        }

        /// <summary>
        /// Gets a single E-Learning result from the database.
        /// </summary>
        /// <param name="id">The ID from the database of the E-Learning Result.</param>
        /// <returns>The matching E-Learning result or Not Found (404)</returns>
        [Authorize]
        // GET: api/ELResults/5
        [ResponseType(typeof(ResultDTO))]
        public async Task<IHttpActionResult> GetELResult(int id)
        {
            var eLResult = await db.ESRs.FindAsync(id);
            if (eLResult == null)
            {
                return NotFound();
            }

            return Ok(new ResultDTO(eLResult));
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
            var eLResult = await db.ESRs.FindAsync(id);
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
        [ResponseType(typeof(ESRModules))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> PostELResult(ESRModules eLResult)
        {
            //eLResult.Received = DateTime.Now;
            //eLResult.FromADAcc = User.Identity.Name;
            eLResult.Processed = false;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ESRs.Add(eLResult);
            await db.SaveChangesAsync();

            await ProcessResultAsync(eLResult);

            return CreatedAtRoute("DefaultApi", new { id = eLResult.ID }, eLResult);
        }

        /// <summary>
        /// Deletes a E-Learning result from the database.
        /// </summary>
        /// <param name="id">The ID of the E-Learning result to be deleted.</param>
        /// <returns>The deleted E-Learning result/</returns>
        [Authorize]
        // DELETE: api/ELResults/5
        [ResponseType(typeof(ESRModules))]
        public async Task<IHttpActionResult> DeleteELResult(int id)
        {
            var eLResult = await db.ESRs.FindAsync(id);
            if (eLResult == null)
            {
                return NotFound();
            }

            db.ESRs.Remove(eLResult);
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
        private async Task ProcessResultAsync(ESRModules eLResult)
        {
            if (eLResult.StaffID == null)
            {
                if (eLResult.Employee > 0)
                {
                    //Find Person
                    var person = await db.People.SingleOrDefaultAsync(p => p.ESRID == eLResult.Employee);
                    if (person == null)
                    {
                        eLResult.Comments = "Unable to find staff member with matching employee number";
                        await db.SaveChangesAsync();
                        return;
                    }
                    eLResult.StaffID = person.ID;
                }
            }
            
            if (eLResult.CourseID == null)
            {
                //Find Course
                var course = await db.Courses.SingleOrDefaultAsync(c => c.CourseName == eLResult.ModuleName);
                if (course == null)
                {
                    eLResult.Comments = $"Unable to find course matching {eLResult.ModuleName}";
                    await db.SaveChangesAsync();
                    return;
                }
                eLResult.CourseID = course.ID;
            }
            
            
            //Find Requirement
            var req = await db.Requirements.SingleOrDefaultAsync(r => r.Staff == eLResult.StaffID && r.Course == eLResult.CourseID);
            if (req == null)
            {
                eLResult.Comments = "Staff member does not have requirement for course.";
                await db.SaveChangesAsync();
                return;
            }

            //Update Requirement
            if (eLResult.CompletionDate != null)
            {
                req.Status = (short)5;

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
                eLResult.Comments = "No Completion Date included in message";
                await db.SaveChangesAsync();
                return;
            }
            //Update Result
            eLResult.Processed = true;
            await db.SaveChangesAsync();
        }

        private bool ELResultExists(int id)
        {
            return db.ESRs.Count(e => e.ID == id) > 0;
        }
    }
}