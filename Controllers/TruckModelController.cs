using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Truck_Registration_Control.Data;
using Truck_Registration_Control.Domain;

namespace Truck_Registration_Control.Controllers
{
    [Route("v1/truckmodel")]
    public class TruckModelController : ControllerBase
    {
        [HttpGet]
        public  async Task<ActionResult<List<TruckModel>>> Get([FromServices]DataContext context)
        {
            var truckModels = await context.TruckModels.AsNoTracking().ToListAsync();
            
            if(truckModels != null) 
                return Ok(truckModels);

            return NotFound(new { message = "Registros não encontrados."} );
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<ActionResult<TruckModel>> GetById(int Id, [FromServices] DataContext context)
        {
            var truckModels = await context.TruckModels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
            
            if(truckModels != null) 
                return Ok(truckModels);

            return NotFound(new { message = "Registro não encontrado."} );
        }

        [HttpPost]
        public async Task<ActionResult<TruckModel>> Post([FromBody] TruckModel truckmodel, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.TruckModels.Add(truckmodel);
                await context.SaveChangesAsync();
                
                return Ok(truckmodel);
            }
            catch
            {
                return BadRequest( new { message = "Não foi possível salvar o modelo de caminhão."});
            }
        }

        [HttpPut]
        [Route("{Id:int}")]
        public async Task<ActionResult<TruckModel>> Put(int Id, 
                                                        [FromBody] TruckModel truckmodel,
                                                        [FromServices] DataContext context)
        {
            if(Id != truckmodel.Id)
                return NotFound( new { message = "Modelo de caminhão não encontrado."});

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<TruckModel>(truckmodel).State = EntityState.Modified;
                await context.SaveChangesAsync();
                
                return Ok(truckmodel);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest( new { message = "Este registro já foi atualizado."});
            }
            catch (Exception)
            {
                return BadRequest( new { message = "Não foi possível atualizar o modelo de caminhão."});
            }
        }

        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<ActionResult<TruckModel>> Delete(int Id, [FromServices] DataContext context)
        {

            var truckModel = await context.TruckModels.FirstOrDefaultAsync(x => x.Id == Id);

            if (truckModel == null)
                return NotFound(new { message = "Modelo de caminhão não encontrado." });

            try
            {
                context.TruckModels.Remove(truckModel);
                await context.SaveChangesAsync();
                
                return Ok(new { message = "Moelo de caminhão removido com sucesso." });
            }
            catch
            {
                return NotFound(new { message = "Modelo de caminhão não encontrado." });
            }
        }
    }
}