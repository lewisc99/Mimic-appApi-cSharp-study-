﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test.V1.Models.DTO
{
    public abstract class BaseDTO
    {

        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();

    }
}
