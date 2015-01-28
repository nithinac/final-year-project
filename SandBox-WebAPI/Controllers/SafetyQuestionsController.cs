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
using SandBox_WebAPI.Utilities;
using System.Collections;

namespace SandBox_WebAPI.Controllers
{
    public class SafetyQuestionsController : ApiController
    {
        private SandBoxContext db = new SandBoxContext();

        // GET: api/SafetyQuestions
        public WebApiResponseList<SafetyQuestion> GetSafetyQuestions()
        {
            WebApiResponseList<SafetyQuestion> response = new WebApiResponseList<SafetyQuestion>();
            try
            {
                response.RequestUrl = Request.RequestUri.ToString();
                response.Version = WebApi.Version;
                response.Exception = null;
                response.StatusCode = "200";
                response.List = db.SafetyQuestions.ToList();
            }
            catch (Exception e)
            {
                response.Exception = e;
                response.StatusCode = "500";
            }
            return response;
            //return db.SafetyQuestions.Include("Options");
        }

        // GET: api/SafetyQuestions/5
        [ResponseType(typeof(WebApiResponse<SafetyQuestion>))]
        public async Task<IHttpActionResult> GetSafetyQuestion(int id)
        {
            WebApiResponse<SafetyQuestion> response = new WebApiResponse<SafetyQuestion>();
            try
            {
                response.RequestUrl = Request.RequestUri.ToString();
                response.Version = WebApi.Version;
                response.Data = await db.SafetyQuestions.FindAsync(id);
                response.Exception = null;
                response.StatusCode = "200";
                if (response.Data == null)
                {
                    return NotFound();
                }

                response.Includes = new Dictionary<string, string>();
                foreach (var property in typeof(SafetyQuestion).GetProperties())
                {
                    Type propertyType = property.PropertyType;
                    if (!(propertyType.IsPrimitive || propertyType == typeof(string) || propertyType == typeof(DateTime)))
                    {                        
                        response.Includes.Add(property.Name, response.RequestUrl + "/" + property.Name);
                    }

                }
            }
            catch (Exception e)
            {
                response.Exception = e;
                response.StatusCode = "500";
            }
            return Ok(response);
        }

        public async Task<IHttpActionResult> GetSafetyQuestion(int id, string property)
        {                        
            WebApiResponseList<dynamic> response = new WebApiResponseList<dynamic>();
            try
            {
                SafetyQuestion safetyQuestion = await db.SafetyQuestions.Include(property).FirstOrDefaultAsync(d => d.ID == id);

                if (safetyQuestion == null)
                {
                    return NotFound();
                }

                response.RequestUrl = Request.RequestUri.ToString();
                response.Version = WebApi.Version;
                response.Exception = null;
                response.StatusCode = "200";                                
                response.List = safetyQuestion.GetType().GetProperty(property).GetValue(safetyQuestion, null); ;
                if (response.List == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                response.Exception = e;
                response.StatusCode = "500";
            }
            return Ok(response);

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