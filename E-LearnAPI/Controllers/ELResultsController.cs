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

            if (!string.IsNullOrWhiteSpace(search))
            {
                var names = search.Split(' ');
                if (names.Length == 1)
                {
                    result = result.Where(r => r.Staff.Fname.Contains(search) || r.Staff.Sname.Contains(search));
                }
                else if (names.Length == 2)
                {
                    string searchF = names[0];
                    string searchS = names[1];
                    result = result.Where(r => (r.Staff.Fname.Contains(searchF) && r.Staff.Sname.Contains(searchS)));
                }
                else
                {
                    return BadRequest("Too Many Spaces in search term!");
                }

            }

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
        [ResponseType(typeof(ResultDTO))]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> PostELResult(NewResultDTO eLResult)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ESRModules result = new ESRModules()
            {
                Employee = eLResult.Employee??-1,
                UserName = eLResult.UserName,
                ModuleName = eLResult.ModuleName,
                CompletionDate = eLResult.CompletionDate,
                Source = eLResult.Source,
                Processed = false
            };

            if (string.IsNullOrWhiteSpace(result.Source))
            {
                result.Source = User.Identity.Name;
            }

            db.ESRs.Add(result);
            await db.SaveChangesAsync();

            await ProcessResultAsync(result);

            //Make sure Staff and Course details are in memory
            if (result.StaffID != null)
            {
                var person = await db.People.SingleOrDefaultAsync(p => p.ID == result.StaffID);
            }

            if (result.CourseID != null)
            {
                var person = await db.Courses.SingleOrDefaultAsync(c => c.ID == result.CourseID);
            }

            return CreatedAtRoute("DefaultApi", new { id = result.ID }, new ResultDTO(result));
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
                List<string> errors = new List<string>();
                if (eLResult.Employee > 0)
                {
                    //Find Person
                    var person = await db.People.SingleOrDefaultAsync(p => p.ESRID == eLResult.Employee);
                    if (person == null)
                    {
                        errors.Add($"Unable to find staff member matching employee number {eLResult.Employee}");
                    }
                    else
                    {
                        eLResult.StaffID = person.ID;
                    }                   
                }

                if (!string.IsNullOrWhiteSpace(eLResult.UserName) && eLResult.StaffID == null)
                {
                    try
                    {
                        var person = await db.People.SingleOrDefaultAsync(p => p.EMail == eLResult.UserName);
                        if (person == null)
                        {
                            errors.Add($"Unable to find staff member with E-Mail: {eLResult.UserName}");
                        }
                        else
                        {
                            eLResult.StaffID = person.ID;
                        }
                    }
                    catch(InvalidOperationException)
                    {
                        errors.Add($"Multiple Staff records have E-Mail address: {eLResult.UserName}");
                    }
                }

                if (string.IsNullOrWhiteSpace(eLResult.UserName) && eLResult.Employee <= 0)
                {
                    errors.Add("No data items to identify staff member present in data!");
                }

                if (eLResult.StaffID == null)
                {
                    eLResult.Comments = string.Join(", ", errors);
                    await db.SaveChangesAsync();
                    return;
                }
            }
            
            if (eLResult.CourseID == null)
            {
                //Find Course
                var map = await db.CourseMaps.SingleOrDefaultAsync(c => c.ModuleName == eLResult.ModuleName);
                if (map == null)
                {
                    var course = await db.Courses.SingleOrDefaultAsync(c => c.CourseName == eLResult.ModuleName);
                    if (course == null)
                    {
                        eLResult.Comments = $"Unable to find course matching {eLResult.ModuleName}";
                        await db.SaveChangesAsync();
                        return;
                    }
                    eLResult.CourseID = course.ID;
                }
                else
                {
                    eLResult.CourseID = map.CourseID;
                }                
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
                req.Comments = "Course completed via e-learning.";
                
            }
            else
            {
                eLResult.Comments = "No Completion Date included in message";
                await db.SaveChangesAsync();
                return;
            }
            //Update Result
            eLResult.Processed = true;
            eLResult.Comments += string.Format(" Processed: {0:d}", DateTime.Now);
            await db.SaveChangesAsync();
        }

        private bool ELResultExists(int id)
        {
            return db.ESRs.Count(e => e.ID == id) > 0;
        }
    }
}