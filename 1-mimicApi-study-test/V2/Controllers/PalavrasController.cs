using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.V2.Controllers
{



    [Route("api/v{version:apiVersion}/[controller]")]
  //  [Route("api/[controller]")]

    [ApiController]
    [ApiVersion("2.0")]
    public class PalavrasController: ControllerBase
    {


        [HttpGet("",Name = "ObterTodas")]
        [MapToApiVersion("2.0")]
        public string ObterTodas()
        {
            return "Versão 2.0";
        }


    }
}
