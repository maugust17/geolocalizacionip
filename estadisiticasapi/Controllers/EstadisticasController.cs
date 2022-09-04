using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text;

namespace estadisticasapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstadisticasController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabase _cache;
        private readonly ILogger<EstadisticasController> _logger;

        public EstadisticasController(IDatabase cache, IConfiguration configuration, ILogger<EstadisticasController> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _cache = cache;
        }

        [HttpGet]
       
        public async Task<IActionResult> Get()
        {
            Estadisticas estadisticas = new Estadisticas();

            estadisticas.DistanciaMinima = await _cache.StringGetAsync("minimo");
            estadisticas.DistanciaMaxima = await _cache.StringGetAsync("maximo");
            estadisticas.DistanciaPromedio = await _cache.StringGetAsync("promedio");
                       
            return Ok(estadisticas);
        }
    }
}
