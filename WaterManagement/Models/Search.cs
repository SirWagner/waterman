using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WaterManagement.Models
    {
        public class Search
        {


            public string Name { get; set; }

            public string Meter { get; set; }
            public string Bill { get; set; }

            public string State { get; set; }



            public DateTime? DataInicio { get; set; }
            public DateTime? DataFim { get; set; }

        }
    }


