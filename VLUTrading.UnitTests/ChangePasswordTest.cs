using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradingVLU.Controllers;
using TradingVLU.Models;
using VLUTrading.UnitTests.Support;
using System.Web.Mvc;

namespace VLUTrading.UnitTests
{
    [TestClass]
    public class ChangePasswordTest
    {
        [TestMethod]
        public void ValidateChangePasswordCredential_WithValidCredential_ExpectValidCredential()
        {
            {
                //Arrange
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    var controller = new UserController();
                    var userchangepassword = new ChangePasswordViewModel
                    {
                        Password = "Team04",
                        NewPassword = "Team05",
                        ReConfirmPassword = "Team05",
                    };
                    var validationResults = TestModelHelper.ValidateModel(controller, userchangepassword);
                    //Act
                    var viewResult = controller.ChangePassword(userchangepassword) as ViewResult;
                    //Assert
                    Assert.IsNotNull(viewResult);
                    Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
                    Assert.AreEqual(0, validationResults.Count);
                }
            }
        }
        [TestMethod]
        public void ValidateChangePasswordCredential_WithInValidNewPassword_ExpectInvalidPassword()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var userchangepassword = new ChangePasswordViewModel
                {
                    Password = "Team04",
                    NewPassword = new string('A', 101),
                    ReConfirmPassword = new string('A', 101),
                };
                var validationResults = TestModelHelper.ValidateModel(controller, userchangepassword);
                //Act
                var viewResult = controller.ChangePassword(userchangepassword) as ViewResult;
                //Assert
                Assert.IsNotNull(viewResult);
                Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
                Assert.AreEqual(2, validationResults.Count);
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("Minimum four characters and maximun twenty characters, at least one uppercase letter, one lowercase letter and one number."));
            }
        }
        [TestMethod]
        public void ValidateChangePasswordCredential_WithInValidReConfirmPassword_ExpectInvalidPassword()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var userchangepassword = new ChangePasswordViewModel
                {
                    Password = "Team04",
                    NewPassword = "Team05",
                    ReConfirmPassword = "Team06",
                };
                var validationResults = TestModelHelper.ValidateModel(controller, userchangepassword);
                //Act
                var viewResult = controller.ChangePassword(userchangepassword) as ViewResult;
                //Assert
                Assert.IsNotNull(viewResult);
                Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("'Confirm password' and 'New Password' do not match."));
            }
        }
    }
}
