﻿using Amazon.S3.Model;
using Auth.WebAPI.Helper.EmailHelper;
using Notification.API.Domain.Dto.Common;
using Notification.API.Helper.Configuration;
using Notification.API.Manager.Interfaces;

namespace Notification.API 
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();

            #region Options Config
            services.AddOptions<EmailSetting>().BindConfiguration(nameof(EmailSetting)).ValidateDataAnnotations();
            services.AddOptions<EmailSettingRetry>().BindConfiguration(nameof(EmailSettingRetry)).ValidateDataAnnotations();
            services.AddOptions<RabbitMqSettings>().BindConfiguration(nameof(RabbitMqSettings)).ValidateDataAnnotations();
            #endregion
        }
    }
}
