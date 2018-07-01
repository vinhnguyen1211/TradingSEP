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
    public class RegisterTest
    {
        [TestMethod]
        public void ValidateRegisterCredential_WithValidCredential_ExpectValidCredential()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var user = new USERMetadata
                {
                    username = "vocongminh256",
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
                Assert.AreEqual(0, validationResults.Count);
            }
        }
        [TestMethod]
        public void ValidateRegisterCredential_WithInValidEmailCredential_ExpectInvalidEmailCredential()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var user = new USERMetadata
                {
                    username = "vocongminh256",
                    password = "Team04",
                    confirmpassword = "Team04",
                    email = "vocongminh256@gmail.com",
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
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("It must contains only @vanlanguni.vn or @vanlanguni.edu.vn and not just all number"));

            }
        }
        [TestMethod]
        public void ValidateRegisterCredential_WithInValidConfirmPassword_ExpectInValidConfirmPassword()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var user = new USERMetadata
                {
                    username = "vocongminh256",
                    password = "Team04",
                    confirmpassword = "Team05",
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
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("Password Mismatched. Re-enter your password"));
            }
        }
        [TestMethod]
        public void ValidateRegisterCredential_WithInValidUsername_ExpectInValidUsername()
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
        [TestMethod]
        public void ValidateLoginCredential_WithInValidName_ExpectInvalidName()
        {
            //Arrange

            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var user = new USERMetadata
                {
                    username = "vocongminh256",
                    password = "Team04",
                    confirmpassword = "Team04",
                    email = "vocongminh256@vanlanguni.vn",
                    name = "123minh",
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
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("It must contains only characters and one space between each one"));
            }
        }
        [TestMethod]
        public void ValidateLoginCredential_WithInValidAnswer_ExpectInvalidAnswer()
        {
            //Arrange
            using (vlutrading3545Entities db = new vlutrading3545Entities())
            {
                var controller = new UserController();
                var user = new USERMetadata
                {
                    username = "vocongminh256",
                    password = "Team04",
                    confirmpassword = "Team04",
                    email = "vocongminh256@vanlanguni.vn",
                    name = "vocongminh",
                    id_security_question = 1,
                    answer_security_question = "tr",
                };
                var validationResults = TestModelHelper.ValidateModel(controller, user);
                //Act
                var viewResult = controller.register(user) as ViewResult;
                //Assert
                Assert.IsNotNull(viewResult);
                Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
                Assert.AreEqual(1, validationResults.Count);
                Assert.IsTrue(validationResults[0].ErrorMessage.Equals("The Answer must be at least 6 characters."));
            }
        }
    }
}

       