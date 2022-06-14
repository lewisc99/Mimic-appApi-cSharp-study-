using _1_mimicApi_study_test.Data;
using _1_mimicApi_study_test.Helpers;
using _1_mimicApi_study_test.V1.Models;
using _1_mimicApi_study_test.V1.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.V1.Repositories
{
    public class PalavraRepository : IPalavraRepository
    {


        private readonly MimicContext _context;

        public PalavraRepository(MimicContext context)
        {
            _context = context;
        }


        public void Cadastrar(Palavra palavra)
        {

            palavra.Id = 0;

            _context.Palavras.Add(palavra);

            _context.SaveChanges();
        }


        public PaginationList<Palavra> ObterPalavras(PalavraUrlQuery query)
        {

            var lista = new PaginationList<Palavra>();

            var item = _context.Palavras.AsNoTracking().AsQueryable();


            if (query.Data.HasValue)
            {
                item = item.Where(a => a.Criado > query.Data.Value);
            }

            if (query.PagNumero.HasValue  )
            {
                var quantidadeTotalRegistros = item.Count();


                item = item.Skip((query.PagNumero.Value - 1) * query.PagRegistro.Value).Take(query.PagRegistro.Value);



                Paginacao paginacao = new Paginacao();

                paginacao.NumeroPagina = query.PagNumero.Value;
                paginacao.RegistroPorPagina = query.PagRegistro.Value;
                paginacao.TotalRegistros = quantidadeTotalRegistros;

                paginacao.TotalPaginas = (int)Math.Ceiling((double)quantidadeTotalRegistros / query.PagRegistro.Value);


                lista.Paginacao = paginacao;

            }


            lista.Results.AddRange(item.ToList());
            
            return lista;

        }

        public Palavra Obter(int id)
        {
            Palavra palavra = _context.Palavras.FirstOrDefault(p => p.Id == id);


            return palavra;


        }

        public void Atualizar(Palavra palavra)
        {

            

            _context.Palavras.Update(palavra);
            _context.SaveChanges();
        }

        

        public void Deletar(int id)
        {

            Palavra palavra =  Obter(id);

            _context.Palavras.Remove(palavra);

            _context.SaveChanges();

        }

      

      
    }
}
