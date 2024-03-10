using Newtonsoft.Json;
using SunriseNotifier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SunriseNotifier.Services;

public class EmailService
{
	private readonly SmtpClient _smtpClient;
	private readonly string emailAddress = Environment.GetEnvironmentVariable("NoReplyEmail", EnvironmentVariableTarget.Process);	
	private readonly string emailPassword = Environment.GetEnvironmentVariable("NoReplyPassword", EnvironmentVariableTarget.Process);
	private readonly string SmtpHost = Environment.GetEnvironmentVariable("SmtpHost", EnvironmentVariableTarget.Process);
	private readonly string SmtpPort = Environment.GetEnvironmentVariable("SmtpPort", EnvironmentVariableTarget.Process);
	public EmailService(SmtpClient smtpclient)
	{
		smtpclient.Host = SmtpHost;	
		smtpclient.Port = int.Parse(SmtpPort);
		smtpclient.Credentials = new NetworkCredential(emailAddress, emailPassword);
		_smtpClient = smtpclient;
	}

	
	public async Task SendVerdictEmail(SunrisePrediction prediction, string receiverEmail) {
		// Prepare the request to the email API.

		string htmlTemplate = @"
				<p>Hej 👋,</p>
				<p>Her er din solopgangsprognose for i morgen:</p>
				<ul>
					<li>Solen står op: <strong>{0}</strong> 🌅</li>
					<li>Temperatur: <strong>{1} °C</strong> 🌡️</li>
					<li>Vind: <strong>{2} m/s</strong> 🌬️</li>
				</ul>
				<p>Hav en fantastisk dag!</p>
				<p>Knus, møs og kram,<br>Den meget meget pålidelige solopgangsprognose-mand 🧙‍♂️</p>";
		var emailData = new
		{
			to = $"{receiverEmail}",
			subject = "🌄 Der er sku solopgang i morgen 🌅",
			html = string.Format(htmlTemplate, prediction.SunriseTime, prediction.DegreesCelcius, prediction.Wind)
		};

		// Create Mailmessage
		var mm = new MailMessage(emailAddress, emailData.to);
		mm.Subject = emailData.subject;
		mm.IsBodyHtml = true;
		mm.Body = emailData.html;

		// Send the request
		await _smtpClient.SendMailAsync(mm);
	}

	
}
