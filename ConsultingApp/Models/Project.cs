﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsultingApp.Models
{
    public partial class Project
    {
        public Project()
        {
            Invoices = new HashSet<Invoice>();
            ProjectEmployees = new HashSet<ProjectEmployee>();
            ProjectServices = new HashSet<ProjectService>();
        }
        [Key]
        public int Project_ID { get; set; }
        public int Client_ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
        public virtual ICollection<ProjectService> ProjectServices { get; set; }
    }
}