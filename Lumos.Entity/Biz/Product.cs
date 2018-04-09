﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Product")]
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Enumeration.ProductType Type { get; set; }

        public int ProductCategoryId { get; set; }

        [MaxLength(256)]
        public string ProductCategory { get; set; }

        public int SupplierId { get; set; }

        [MaxLength(256)]
        public string Supplier { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string MainImg { get; set; }

        [MaxLength(2048)]
        public string DispalyImgs { get; set; }

        [MaxLength(512)]
        public string BriefIntro { get; set; }

        public string Details { get; set; }

        [MaxLength(2048)]
        public string SpecsJson { get; set; }

        public string ServiceDesc { get; set; }

        public Enumeration.ProductStatus Status { get; set; }

        public bool IsMultiSpec { get; set; }

        [MaxLength(512)]
        public string ProductKindIds { get; set; }

        [MaxLength(512)]
        public string ProductKindNames { get; set; }

        public bool IsHot { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

    }
}
