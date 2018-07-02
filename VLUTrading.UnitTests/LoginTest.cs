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
    public class LoginTest
    {
        [TestMethod]
        public void ValidateLoginCredential_WithValidCredential_ExpectValidCredential()
        {
            {
                //Arrange
                using (vlutrading3545Entities db = new vlutrading3545Entities())
                {
                    var controller = new UserController();
                    var userlogin = new USERMetadata
                    {
                        username = "vocongminh256",
                        password = "Team04",
                        confirmpassword = "Team04",
                        email = "vocongminh256@vanlanguni.vn",
                        name = "vocongminh",
                        id_security_question = 1,
                        answer_security_question = "truyen kinh di",
                    };
                    var validationResults = TestModelHelper.ValidateModel(controller, userlogin);
                    //Act
                    var viewResult = controller.register(userlogin) as ViewResult;
                    //Assert
                    Assert.IsNotNull(viewResult);
                    Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
                    Assert.AreEqual(0, validationResults.Count);
                }
            }
        }
        [TestMethod]
        public void ValidateLoginCredential_WithInValidUsername_ExpectInValidUsername()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var user = new USERMetadata
                {
                    username = "123minh",
                    password = "Team04",
                    confirmpassword = "Team04",
                    email = "vocongminh256@vanlanguni.vn",
                    name = "vocongminh",
                    id_security_question = 1,
                    answer_security_question = "truyen kinh di",
                };
                var validationResults = TestModelHelper.ValidateModel(controller, user);
                //Act
                var viewResult = controller.register(user) as ViewResult;
                //Assert
                Assert.IsNotNull(viewResult);
                Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("First character must be a character, special character is not allowed. Length 6-16 characters"));
            }
        }
        [TestMethod]
        public void ValidateLoginCredential_WithInValidPassword_ExpectInvalidPassword()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var user = new USERMetadata
                {
                    username = "vocongminh256",
                    password = new string('A', 101),
                    confirmpassword = new string('A', 101),
                    email = "vocongminh256@vanlanguni.vn",
                    name = "vocongminh",
                    id_security_question = 1,
                    answer_security_question = "truyen kinh di",
                };
                var validationResults = TestModelHelper.ValidateModel(controller, user);
                //Act
                var viewResult = controller.register(user) as ViewResult;
                //Assert
                Assert.IsNotNull(viewResult);
                Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("Minimum four characters and maximun twenty characters, at least one uppercase letter, one lowercase letter and one number."));
            }
        }
    }
}