﻿namespace CodeGolf.Recaptcha
{
    using System.Net;
    using System.Threading.Tasks;

    public interface IRecaptchaVerifier
    {
        Task<bool> IsValid(string response, IPAddress ip);
    }
}
