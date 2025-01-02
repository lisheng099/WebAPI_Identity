﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class NewsFilesPutDto
    {
        public string Name { get; set; } = null!;
        public string Path { get; set; } = null!;
        public string Extension { get; set; } = null!;
    }
}
