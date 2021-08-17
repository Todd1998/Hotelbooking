﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotelbooking_System.Models
{
    public class SendEmailViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Please enter an email address.")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter the contents")]
        public string Contents { get; set; }

        public HttpPostedFileBase AttachedFile { get; set; }
    }
}