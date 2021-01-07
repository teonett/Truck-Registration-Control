using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Truck_Registration_Control.Domain;
using Truck_Registration_Control.Data;

namespace Truck_Registration_Control.Controllers
{
    [Route("v1/truck")]
    public class TruckController : ControllerBase
    {
        [HttpGet]
        public  async Task<ActionResult<List<Truck>>> Get([FromServices]DataContext context)
        {
            var trucks = await context
                .Trucks
                .Include(x => x.TruckModel)
                .AsNoTracking()
                .ToListAsync();
            
            if(trucks != null) 
                return Ok(trucks);

            return NotFound(new { message = "Registros não encontrados."} );
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<ActionResult<Truck>> GetById(int Id, [FromServices] DataContext context)
        {
            var trucks = await context
                .Trucks
                .Include(x => x.TruckModel)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id);
            
            if(trucks != null) 
                return Ok(trucks);

            return NotFound(new { message = "Registro não encontrado."} );
        }

        [HttpGet]
        [Route("truckmodel/{Id:int}")]
        public async Task<ActionResult<List<Truck>>> GetByTruckModelId(int Id, [FromServices] DataContext context)
        {
            var trucks = await context
                .Trucks
                .Include(x => x.TruckModel)
                .AsNoTracking().Where(x => x.TruckModel.Id == Id)
                .ToListAsync();
            
            if(trucks != null) 
                return Ok(trucks);

            return NotFound(new { message = "Não existe nenhum caminhão cadastrado com o modelo indicado."} );
        }

        [HttpPost]
        public async Task<ActionResult<Truck>> Post([FromBody] Truck truck, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var truckModel = context.TruckModels.FirstOrDefaultAsync(x => x.Id == truck.TruckModelId);
                
                if (truckModel == null)
                    return NotFound(new { message = "Modelo de caminhão não encontrado." });

                context.Trucks.Add(truck);
                await context.SaveChangesAsync();
                
                return Ok(truck);
            }
            catch
            {
                return BadRequest( new { message = "Não foi possível salvar o registro do caminhão."});
            }
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<ActionResult<Truck>> Put(int Id, [FromBody] Truck truck, [FromServices] DataContext context)
        {
            if(Id != truck.Id)
                return NotFound( new { message = "Registro do caminhão não encontrado."});

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Truck>(truck).State = EntityState.Modified;
                await context.SaveChangesAsync();
                
                return Ok(truck);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest( new { message = "Este registro já foi atualizado."});
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível atualizar o registro do Caminhão."});
            }
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<ActionResult<Truck>> Delete(int Id, [FromServices] DataContext context)
        {

            var truck = await context.Trucks.FirstOrDefaultAsync(x => x.Id == Id);

            if (truck == null)
                return NotFound(new { message = "Registro do caminhão não encontrado." });

            try
            {
                context.Trucks.Remove(truck);
                await context.SaveChangesAsync();
                
                return Ok(new { message = "Registro de caminhão removido com sucesso." });
            }
            catch
            {
                return NotFound(new { message = "Registro do caminhão não encontrado." });
            }
        }
    }
}