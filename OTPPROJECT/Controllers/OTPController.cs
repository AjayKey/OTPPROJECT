using OTPPROJECT.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;


namespace OTPPROJECT.Controllers
{
    public class OTPController : Controller
    {
        private object phoneNumberOrEmail;

        public object SMSProvider { get; private set; }

        public ActionResult SendOTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendOTP(OTPModel model)
        {
            if (ModelState.IsValid)
            {
                string otp = GenerateOTP();
                SaveOTP(model.PhoneNumberOrEmail, otp);
                SendOTP(model.PhoneNumberOrEmail, otp);
                return RedirectToAction("VerifyOTP");
            }
            return View(model);
        }

        private string GenerateOTP()
        {
            // Generate a random OTP here (e.g., using Random class or a third-party library)
            // Return the generated OTP
            return "123456";
        }

        private void SaveOTP(string phoneNumberOrEmail, string otp)
        {
            string ConnectionString = "data source=DESKTOP-HR3BKPI;initial catalog=otp;integrated security=true;";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "INSERT INTO OTP (PhoneNumberOrEmail, OTP, CreatedAt) VALUES (@PhoneNumberOrEmail, @OTP, @CreatedAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PhoneNumberOrEmail", phoneNumberOrEmail);
                command.Parameters.AddWithValue("@OTP", otp);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


        private void SendOTP(string phoneNumberOrEmail, string otp)
        {
            // Implement the logic to send the OTP via SMS or email using a third-party service or API
            // This code snippet assumes sending the OTP via SMS using a hypothetical SMS service
            SmtpClient smtpClient = new SmtpClient("your-smtp-server");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("your-email", "your-password");
            smtpClient.EnableSsl = true;

            // Compose the email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("your-email");
            mailMessage.To.Add(new MailAddress(phoneNumberOrEmail));
            mailMessage.Subject = "OTP Verification";
            mailMessage.Body = "Your OTP is: " + otp;

            // Send the email
            smtpClient.Send(mailMessage);
        }

        // OTPController.cs
        public ActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOTP(string otp)
        {
            // Retrieve the stored OTP from the database based on the user's phone number or email
            string storedOTP;
            string ConnectionString = "data source=DESKTOP-HR3BKPI;initial catalog=otp;integrated security=true;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string query = "SELECT TOP 1 OTP FROM OTP WHERE PhoneNumberOrEmail = @PhoneNumberOrEmail ORDER BY CreatedAt DESC";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PhoneNumberOrEmail", phoneNumberOrEmail);

                connection.Open();
                storedOTP = command.ExecuteScalar() as string;
            }

            if (otp == storedOTP)
            {
                // OTP is valid
                // Implement your logic here for successful OTP verification
                return RedirectToAction("Success");
            }
            else
            {
                // Invalid OTP
                // Implement your logic here for failed OTP verification
                ModelState.AddModelError("OTP", "Invalid OTP");
                return View();
            }
        }


        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failure()
        {
            return View();
        }
    }
}