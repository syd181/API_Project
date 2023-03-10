using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Population.Data;
using API_Population.Models;

namespace API_Population.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaysController : ControllerBase
    {
        private readonly API_PopulationContext _context;

        public PaysController(API_PopulationContext context)
        {
            _context = context;
        }

        // GET : api/Pays 
        // Cette methode GET va nous permettre d'afficher tout les pays de la BDD donc elle retourne une liste de Pays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pays>>> GetPaysWithPopulations()
        {
            var paysWithPopulations = await _context.Pays
                .Include(p => p.Populations)
                .ToListAsync();

            if (paysWithPopulations == null)
            {
                return NotFound();
            }

            return paysWithPopulations;
        }


        
        // GET: api/Pays/5
        // On va pouvoir avec cette méthode récupérer juste un seul pays en fournissant son id 
        [HttpGet("{id}")]
        public async Task<ActionResult<Pays>> GetPays(int id)
        {
            var pays = await _context.Pays.Include(p => p.Populations).Where(p => p.Id == id).FirstOrDefaultAsync();

            if (pays == null)
            {
                return NotFound();
            }

            return pays;
        }

        // On pourra creer des pays en donnant dans postman le nom et le continent et les informations sur la population
        [HttpPost]
        public async Task<IActionResult> PostPays(IEnumerable<Pays> paysList)
        {
            if (paysList == null || !paysList.Any())
            {
                return BadRequest("La liste des pays ne peut pas être vide.");
            }

            foreach (var pays in paysList)
            {
                if (pays.Populations == null || !pays.Populations.Any())
                {
                    return BadRequest("La liste des populations pour le pays " + pays.Country + " ne peut pas être vide.");
                }

                foreach (var population in pays.Populations)
                {
                    population.PaysId = pays.Id; // pour affecter l'Id du pays à chaque population correspondante
                }
            }

            _context.Pays.AddRange(paysList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPays), new { }, paysList);
            NoContent();
        }












        /*

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Pays>>> PostPays(IEnumerable<Pays> paysList)
        {
            if (paysList == null || !paysList.Any())
            {
                return BadRequest("La liste des pays ne peut pas être vide.");
            }

            var paysEntities = new List<Pays>();

            foreach (var pays in paysList)
            {
                var paysEntity = new Pays
                {
                    Country = pays.Country,
                    Continent = pays.Continent,
                    Populations = null
                };

                paysEntities.Add(paysEntity);
            }

            _context.Pays.AddRange(paysEntities);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPays), new { }, paysEntities);
        }
        */

        /*
        // POST: api/Pays
        // On pourra creer des pays en donnant dans postman le nom et le continent 
        [HttpPost]
        // La methode prend une liste de pays en param et vérifie si cette liste n'est pas vide 
        public async Task<ActionResult<IEnumerable<Pays>>> PostPays(IEnumerable<Pays> paysList, IEnumerable<Population> populationList)
        {
            if (paysList == null || !paysList.Any())
            {
                return BadRequest("La liste de pays ne peut pas etre vide.");
            }
            // Si la liste est valide, elle ajoute les pays à la BDD 
            _context.Pays.AddRange(paysList);
            _context.Population.AddRange(populationList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPays), new {}, paysList);
        }

        */



        /*
        // POST: api/Pays
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Pays>>> PostPays(IEnumerable<Pays> paysList)
        {
            if (paysList == null || !paysList.Any())
            {
                return BadRequest("La liste des pays ne peut pas être vide.");
            }

            foreach (var pays in paysList)
            {
                if (pays.Populations == null || !pays.Populations.Any())
                {
                    return BadRequest("La liste des populations pour le pays " + pays.Country + " ne peut pas être vide.");
                }
            }

            _context.Pays.AddRange(paysList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPays), new { }, paysList);
        }
        */

        /*

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Pays>>> PostPays(IEnumerable<Pays> paysList)
        {
            if (paysList == null || !paysList.Any())
            {
                return BadRequest("La liste des pays ne peut pas être vide.");
            }

            foreach (var pays in paysList)
            {
                if (pays.Populations == null || !pays.Populations.Any())
                {
                    return BadRequest("La liste des populations pour le pays " + pays.Country + " ne peut pas être vide.");
                }
                
            }

            _context.Pays.AddRange(paysList);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPays), new { }, paysList);
        }

        */


        /*
         * Frist

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id)
            {
                return BadRequest("L'id du pays n'ai pas correct");
            }

            var existingPays = await _context.Pays.Include(p => p.Populations)
                                                  .FirstOrDefaultAsync(p => p.Id == id);
            if (existingPays == null)
            {
                return NotFound();
            }

            existingPays.Country = pays.Country;
            existingPays.Continent = pays.Continent;

            if (pays.Populations != null)
            {
                if (existingPays.Populations== null)
                {
                    existingPays.Populations = new List<Population>();
                }

                foreach (var population in pays.Populations)
                {
                    var existingPopulation = existingPays.Populations.FirstOrDefault(p => p.Id == population.Id);
                    if (existingPopulation == null)
                    {
                        existingPays.Populations.Add(population);
                    }
                    else
                    {
                            existingPopulation.Annee = population.Annee;
                            existingPopulation.NbrHabitants = population.NbrHabitants;
                    }
                    
                }
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
        */







        [HttpPut("{id}")]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id)
            {
                return BadRequest("L'id du pays n'ai pas correct");
            }

            var existingPays = await _context.Pays.Include(p => p.Populations)
                                                  .FirstOrDefaultAsync(p => p.Id == id);
            if (existingPays == null)
            {
                return NotFound();
            }

            existingPays.Country = pays.Country;
            existingPays.Continent = pays.Continent;

            if (pays.Populations != null && pays.Populations.Any())
            {
                foreach (var population in pays.Populations)
                {
                    var existingPopulation = existingPays.Populations.FirstOrDefault(p => p.Id == population.Id);
                    if (existingPopulation == null)
                    {
                        var newPopulation = new Population { Annee = population.Annee, NbrHabitants = population.NbrHabitants };
                        existingPays.Populations.Add(newPopulation);
                    }
                    else
                    {

                       
                        
                            existingPopulation.Annee = population.Annee;
                            existingPopulation.NbrHabitants = population.NbrHabitants;
                        

                    }

                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }















        /*

        // PUT: api/Pays/5
        // On pourras changer un pays c'est à dire UPDATE mettre à jour les informations ( soit le nom, ou le continent )
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id)
            {
                return BadRequest();
            }

            _context.Entry(pays).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaysExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */


        // DELETE: api/Pays/5
        // Methode qui va supprimer de la table un pays grace à son id 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePays(int id)
        {
            if (_context.Pays == null)
            {
                return NotFound();
            }
            var pays = await _context.Pays.FindAsync(id);
            if (pays == null)
            {
                return NotFound();
            }

            _context.Pays.Remove(pays);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool PaysExists(int id)
        {
            return (_context.Pays?.Any(e => e.Id == id)).GetValueOrDefault();
        }

      
        /* 
         * Conclusion : Le CRUD est bon pour L'API Pays 
         * Test réalisé le 04/03/23
         * Sydney A.
         */

        
    }
}
