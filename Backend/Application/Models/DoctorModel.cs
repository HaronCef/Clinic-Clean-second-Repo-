﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class DoctorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ESpecializations Specialization { get; set; }
    }
}
