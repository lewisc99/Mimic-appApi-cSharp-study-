using _1_mimicApi_study_test.Data;
using _1_mimicApi_study_test.Helpers;
using _1_mimicApi_study_test.V1.Models;
using _1_mimicApi_study_test.V1.Models.DTO;
using _1_mimicApi_study_test.V1.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.V1.Controllers
{


    [ApiController]
   
    [Route("api/v{version:apiVersion}/palavras")]
    //[Route("api/[controller]")]
    [ApiVersion("1.0",Deprecated = true)]
    [ApiVersion("1.1")]


    public class PalavrasController : ControllerBase
    {


        private readonly  IPalavraRepository _repository;
        private readonly IMapper _mapper;

        public PalavrasController(IPalavraRepository context, IMapper mapper)
        {
            _repository = context;
            _mapper = mapper;

        }



        /// <summary>
        /// Operação que realiza o cadastro da Palavra
        /// </summary>
        /// <param name="palavra">Um Objeto Palavra</param>
        /// <returns>Retorna o objeto cadastrado</returns>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody] Palavra palavra)
        {


            if (palavra == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }



            palavra.Ativo = true;
            palavra.Criado = DateTime.Now;
            _repository.Cadastrar(palavra);


            _repository.Cadastrar(palavra);

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);



            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }),
                "GET"));


            return Created($"/api/palavras/{palavra.Id}", palavraDTO);





        }




        /// <summary>
        /// Operação que pega do banco de dados todas as palavras existentes
        /// </summary>
        /// <param name="query">Filtros de pesquisa</param>
        /// <returns>Listagem de Palavras</returns>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpGet("",Name = "ObterTodas")]
        public ActionResult ObterTodas([FromQuery] PalavraUrlQuery? query)
        {



            PaginationList<Palavra> lista = _repository.ObterPalavras(query);




            if (lista.Results.Count == 0)
            {
                return NotFound();
            }

         

    


            PaginationList<PalavraDTO> listaDTO = _mapper.Map<PaginationList<Palavra>,PaginationList<PalavraDTO>>(lista);


         

            foreach (var palavra in listaDTO.Results)
            {
                palavra.Links = new List<LinkDTO>();

                palavra.Links.Add(new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavra.Id }), "GET"));

            }
            listaDTO.links.Add(new LinkDTO("self", Url.Link("ObterTodas", query), "GET"));



            if (lista.Paginacao != null)
            {
                Response.Headers.Add("X -  Pagination ", JsonConvert.SerializeObject(lista.Paginacao));


                if (query.PagNumero + 1 <= lista.Paginacao.TotalPaginas)
                {
                    var queryString = new PalavraUrlQuery()
                    {
                        PagNumero = query.PagNumero + 1,
                        PagRegistro = query.PagRegistro,
                        Data = query.Data
                    };


                    listaDTO.links.Add(new LinkDTO("next", Url.Link("ObterTodas", queryString),
                        "GET"));

                }

                if (query.PagNumero - 1 > 0)
                {
                    var queryString = new PalavraUrlQuery()
                    {
                        PagNumero = query.PagNumero - 1,
                        PagRegistro = query.PagRegistro,
                        Data = query.Data
                    };

                    listaDTO.links.Add(new LinkDTO("previous", Url.Link("ObterTodas", queryString), "GET"));

                }


            }


         


            return Ok(listaDTO);


        }





        /// <summary>
        /// Operação que pega uma unica palavra da base de dados
        /// </summary>
        /// <param name="id">Identificar da palavra</param>
        /// <returns>Um Objeto de Palavra</returns>

        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpGet("{id}", Name = "ObterPalavra")]
        public ActionResult<Palavra> obter(int id)
        {

            Palavra palavraFound = _repository.Obter(id);

           
            if (palavraFound == null)
            {
                return NotFound();
            }


            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavraFound);


         //   palavraDTO.Links = new List<LinkDTO>();

            palavraDTO.Links.Add(
                new LinkDTO("self",Url.Link("ObterPalavra", new { id = palavraDTO.Id}), "GET"));


            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("AtualizarPalavra",new { id = palavraDTO.Id}), "PUT"));


            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("ExcluirPalavra", new { id = palavraDTO.Id }),"DELETE"));
            
            return Ok(palavraDTO);

        }



        /// <summary>
        /// Operação que realiza a substituição de dados de uma Palavra especifica
        /// </summary>
        /// <param name="id">Codigo identificador da Palavra a ser alterado</param>
        /// <param name="palavra">Objeto Palavra com dados para alteração</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpPut("{id}", Name = "AtualizarPalavra")]
        public ActionResult Atualizar([FromRoute] int id, [FromBody] Palavra palavra)
        {

            Palavra palavraId = _repository.Obter(id);
            

            if  (palavraId == null)
            {
                return NotFound();
            }

           else if (palavra == null)
            {
                return BadRequest();
            }

            else if(!ModelState.IsValid)
            {

                return UnprocessableEntity(ModelState);
            }

            else
            {
                palavra.Id = palavraId.Id;
                palavra.Ativo = palavraId.Ativo;
                palavra.Criado = palavraId.Criado;
                palavra.Atualizado = DateTime.Now;

                _repository.Atualizar(palavra);


                PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
                palavraDTO.Links.Add(
                    new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }), "GET"));


            }




           

            return Ok();


        }


        /// <summary>
        /// Operação que exclui uma Palavra do sistema
        /// </summary>
        /// <param name="id">Código de identificação da Palavra</param>
        /// <returns></returns>
        [MapToApiVersion("1.1")]
        [HttpDelete("{id}", Name = "ExcluirPalavra")]
        public ActionResult Deletar(int id)
        {

            Palavra palavraId = _repository.Obter(id);



            if (palavraId == null)
            {
                return NotFound();
            }
            else
            {
                
                _repository.Deletar(id);
            }



            return NoContent();



        }

    }
}

