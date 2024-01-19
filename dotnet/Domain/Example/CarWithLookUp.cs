using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain.Example
{
    public class CarWithLookUp
    {
        public int Id { get; set; }

        public LookUp CarType { get; set; }
        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

    }
}
