﻿namespace AngularProject.DTO
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public bool Result { get; set; }
        public List<string> Errors { get; set; }

        public string UserName { get; set; } = string.Empty;
    }
}
