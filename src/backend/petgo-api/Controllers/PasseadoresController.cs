using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using petgo.api.Data;

namespace petgo.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasseadoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PasseadoresController(AppDbContext context)
        {
            _context = context;
        }

        
    }
}