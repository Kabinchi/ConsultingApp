﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsultingApp.Models
{
    public partial class ProjectService
    {
        [Key]
        public int ProjectService_ID { get; set; }
        public int Project_ID { get; set; }
        public int Service_ID { get; set; }
        public int Quantity { get; set; }

        public virtual Project Project { get; set; }
        public virtual Service Service { get; set; }
    }
}