using _1_mimicApi_study_test.V1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.Data
{
    public class MimicContext: DbContext
    {

        public DbSet<Palavra> Palavras { get; set; }

        public MimicContext(DbContextOptions<MimicContext> options):base(options)
        {

        }
    }
}
