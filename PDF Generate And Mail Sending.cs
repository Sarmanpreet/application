-----------------Generating PDF
public ActionResult FinalInvoicePDF()
        {
            string row = null;
            JobPersonLogicLayer objPerson = null;
            JobNoLogicLayer objJobNo = null;
            int UserId = 0;
            int jobNo = Convert.ToInt32(Session["JobId"]);
            InVoiceViewModel invoiceVM = null;
            objJobNo = new JobNoLogicLayer();
            objPerson = new JobPersonLogicLayer();
            invoiceVM = new InVoiceViewModel();
            invoiceVM.JobNoDetails = objJobNo.GetJobNoById(jobNo);
            UserId = invoiceVM.JobNoDetails.UserId;
            invoiceVM.JobPersonDetails = objPerson.getPersonById(UserId);
            invoiceVM.JobQuantityDetails = objJobQuantity.GetJobbyJobId(jobNo);
            
            FileResult fileResult = null;
            var generator = new NReco.PdfGenerator.HtmlToPdfConverter();
            string FileName = invoiceVM.JobNoDetails.JobNo + ".pdf";
            FileName = FileName.Replace('/', '_');
            FileName = FileName.Replace(':', '_');
            FileName = FileName.Replace(' ','_');
            generator.Orientation = NReco.PdfGenerator.PageOrientation.Portrait;
            string htmlContent = new WebClient().DownloadString("http://localhost:50000/JobQuantityDetails/FinalInvoice/"+ jobNo);
            //string htmlContent = new WebClient().DownloadString("http://jood.qservicesit.com/JobQuantityDetails/FinalInvoice/" + jobNo);
            var pdfBytes = generator.GeneratePdf(htmlContent.ToString());
            HttpContext.Response.ContentType = "application/pdf";
            HttpContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Response.AppendHeader("content-disposition", string.Format("attachment; filename={0}", FileName));
            //fileResult=new FileStreamResult()
           // fileResult = new FileContentResult(pdfBytes, "application/pdf");
            //fileResult.FileDownloadName = FileName;
            //string path = System.Web.HttpContext.Current.Server.MapPath(@"~\Saved Files\" + FileName);
            string path = Path.Combine(Server.MapPath("~/SavedFiles"), FileName);
            System.IO.File.WriteAllBytes(path, pdfBytes);
           
            if(Mailing.Sendmail(invoiceVM,path,FileName, pdfBytes))
            {
                return RedirectToAction("Index", "JobPersonDetails");
            }
            else
            {
                return RedirectToAction("FinalInvoice", jobNo);
            }
            //return View(invoiceVM);
        }
---------------------------Sending Mail With Attaching PDF
using JobBook.ViewModel;
using SendGrid;
//using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web;
namespace JobBook.ExtraMethod
{
    public class Mailing
    {
        public static bool Sendmail(InVoiceViewModel InvoiceVm,string path,string filename,byte[] image)
        {
            bool result = false;
            
            try
            {
                
                string Body = "<html><body><div style='border:3px solid #e9e9e9;' >";
    Body += @"<span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    @<h1 style='background-color:#0e9aef; width:500px; color:white;'>Invoice Of Job Books</h1>
                                            <p>Hi"+InvoiceVm.JobNoDetails.JobPersonDetails.Name+@"</p>
                                                <p>Here I'm sending you the attachment of Your Invoice Kindly check it."
                                            
                                    ;
                         Body += "</div></body></html>";
                var client = new HttpClient();
                
                MultipartFormDataContent form = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(image);
                imageContent.Headers.ContentType =
                    MediaTypeHeaderValue.Parse("application/pdf");
                form.Add(new StringContent("qservicesinc"), "api_user");
                form.Add(new StringContent("qservices@2015"), "api_key");
                form.Add(new StringContent(InvoiceVm.JobNoDetails.JobPersonDetails.Email), "to[]");
                form.Add(new StringContent(InvoiceVm.JobNoDetails.JobPersonDetails.Email), "toname[]");
                form.Add(new StringContent("Regarding Your Invoice"), "subject");
                form.Add(new StringContent(Body), "html");
                form.Add(new StringContent("do-not-reply@ddmauna.com"), "from");
                form.Add(imageContent, "files["+filename+"]");
           
                var response = client.PostAsync("https://api.sendgrid.com/api/mail.send.json", form).Result;
                result = true;
            }
                    catch (Exception ex)
                    {
                result = false;
                    }
                return result;
            }
}
}
