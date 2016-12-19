    [HttpPost]
        [Route("api/EmailSentsModel/SentEmail")]
        [AllowAnonymous]
        public IHttpActionResult SentEmail(EmailSent emailSent)
        {
            string jsonString = string.Empty;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("Ashdeep.qservices@gmail.com");
                mail.To.Add(emailSent.ToRecipient);
                mail.Subject = emailSent.Subject;
                mail.Body = emailSent.Details;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("Ashdeep.qservices@gmail.com", "arbxvkedpqndzidm");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                jsonString = "Sent Successful";
            }
            catch (Exception)
            {
                jsonString = "Failed To Sent";
            }
            return Ok(jsonString);
        }
