﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entity
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Purchase { get; set; }
        public string Sale { get; set; }
        public Department Department { get; set; }
    }
}
