using PromNotas.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PromNotas.Context
{
    public class NotasContext:DbContext
    {
        public NotasContext(): base("name=NotasContext")
        {

        }

        public DbSet<Nota> Notas { get; set; }
    }
}