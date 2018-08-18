﻿using System.Threading.Tasks;

namespace Climb.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}