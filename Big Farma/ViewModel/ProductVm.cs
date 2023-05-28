﻿using Big_Farma.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Big_Farma.ViewModel
{
    public class ProductVm
    {
        // This class represents the ViewModel for the Product entity.
        public Product product { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryLists { get; set; }


    }
}
