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
using SandBox_WebAPI.Models;

namespace SandBox_WebAPI.Controllers
{
    public class SafetyQuestionsController : ApiController
    {
        private SandBoxContext db = new SandBoxContext();

        // GET: api/SafetyQuestions
        public IQueryable<SafetyQuestion> GetSafetyQuestions()
        {
            return db.SafetyQuestions.Include("Options");
        }

        // GET: api/SafetyQuestions/5
        [ResponseType(typeof(SafetyQuestion))]
        public async Task<IHttpActionResult> GetSafetyQuestion(int id)
        {
            SafetyQuestion safetyQuestion = await db.SafetyQuestions.Include("Options").FirstOrDefaultAsync(s=>s.ID==id);
            if (safetyQuestion == null)
            {
                return NotFound();
            }

            return Ok(safetyQuestion);
        }

        // PUT: api/SafetyQuestions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSafetyQuestion(int id, SafetyQuestion safetyQuestion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != safetyQuestion.ID)
            {
                return BadRequest();
            }
            db.Entry(safetyQuestion).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SafetyQuestionExists(id))
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

        // POST: api/SafetyQuestions
        [ResponseType(typeof(SafetyQuestion))]
        public async Task<IHttpActionResult> PostSafetyQuestion(SafetyQuestionViewModel safetyQuestionView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SafetyQuestion safetyQuestion = new SafetyQuestion();
            safetyQuestion.Question = safetyQuestionView.Question;
            List<Option> Options = new List<Option>();
            foreach(String option in safetyQuestionView.Options)
            {
                Options.Add(new Option(option));
            }
            safetyQuestion.Options = Options;
            safetyQuestion.Answer = safetyQuestionView.AnswerId;

            db.SafetyQuestions.Add(safetyQuestion);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = safetyQuestion.ID }, safetyQuestion);
        }

        // DELETE: api/SafetyQuestions/5
        [ResponseType(typeof(SafetyQuestion))]
        public async Task<IHttpActionResult> DeleteSafetyQuestion(int id)
        {
            SafetyQuestion safetyQuestion = await db.SafetyQuestions.FindAsync(id);
            if (safetyQuestion == null)
            {
                return NotFound();
            }

            db.SafetyQuestions.Remove(safetyQuestion);
            await db.SaveChangesAsync();

            return Ok(safetyQuestion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SafetyQuestionExists(int id)
        {
            return db.SafetyQuestions.Count(e => e.ID == id) > 0;
        }
    }
}