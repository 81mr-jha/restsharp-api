
using apiautomation.Pages;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace apiautomation.Tests
{
    [TestFixture]
    public class UsersTests
    {
        private UsersApi _usersApi;
        private static ExtentReports extent;
        private ExtentTest test;
        private static ExtentHtmlReporter _htmlReporter;

        [OneTimeSetUp]
        public void SetupReport()
        {
            string reportPath = "C:\\TestReports\\ExtentReport.html";
            _htmlReporter = new ExtentHtmlReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(_htmlReporter);
        }

        [SetUp]
        public void Setup()
        {
            _usersApi = new UsersApi();
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [Test]
        public async Task GetUsers_ShouldReturnSuccess()
        {
            //var response = await _usersApi.GetUsers();

            //TestContext.WriteLine("Response Content: " + response.Content);
            //TestContext.WriteLine("Status Code: " + response.StatusCode);

            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //**
            //test.Log(Status.Info, "Starting test: GetUsers_ShouldReturnSuccess");

            //var response = await _usersApi.GetUsers();
            //test.Log(Status.Info, "Response received: " + response.Content);

            //Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //test.Log(Status.Pass, "API responded with 200 OK");

            test.Log(Status.Info, "Starting test: GetUsers_ShouldReturnSuccess");

            try
            {
                var response = await _usersApi.GetUsers();
                test.Log(Status.Info, "Response received: " + response.Content);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                test.Log(Status.Pass, "API responded with 200 OK ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw; // Rethrow so NUnit also marks it as failed
            }
        }

        //[Test]
        //public async Task CreateUser_AndVerifyWithGet_ShouldSucceed()
        //{
        //    test.Log(Status.Info, "creating user: CreateUser_AndVerifyWithGet_ShouldSucceed");

        //    // Step 1: Create a new user
        //    var createResponse = await _usersApi.CreateUser("Mike Ross", "Lawer");
        //    Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        //    // Extract the user ID from the response
        //    var createdUser = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
        //    string userId = createdUser?.id;
        //    Assert.That(userId, Is.Not.Null.And.Not.Empty, "User ID should be returned after creation");

        //    TestContext.WriteLine("New User ID: " + userId);

        //    // Step 2: Fetch the user using the extracted ID
        //    var getResponse = await _usersApi.GetUser(userId);
        //    Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //    TestContext.WriteLine("User found: " + getResponse.Content);

        //    // Step 3: Verify that the user details match
        //    var fetchedUser = JsonConvert.DeserializeObject<dynamic>(getResponse.Content);
        //    Assert.That((string)fetchedUser.name, Is.EqualTo("Nitin kumar"));
        //    Assert.That((string)fetchedUser.job, Is.EqualTo("Accountant"));
        //}

        [Test]
        public async Task CreateUser_AndVerifyWithGet_ShouldSucceed()
        {
            test.Log(Status.Info, "Starting test: CreateUser_AndVerifyWithGet_ShouldSucceed");

            try
            {
                var createResponse = await _usersApi.CreateUser("Nitin kumar", "Accountant");
                Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                test.Log(Status.Pass, "User created successfully ✅");

                var createdUser = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
                string userId = createdUser?.id;
                Assert.That(userId, Is.Not.Null.And.Not.Empty, "User ID should be returned after creation");

                var getResponse = await _usersApi.GetUser(userId);
                Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                test.Log(Status.Pass, "User retrieved successfully ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw;
            }
        }



        // ✅ Update User - Positive
        [Test]
        public async Task UpdateUser_ShouldReturn200()
        {
            test.Log(Status.Info, "Updating test: UpdateUser_ShouldReturn200");

            try
            {
                var response = await _usersApi.UpdateUser("55", "Mike Ross", "QA Engineer");
                test.Log(Status.Info, "Response received: " + response.Content);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                test.Log(Status.Pass, "User updated successfully ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw; // Rethrow to ensure NUnit marks it as failed
            }
        }

        // ✅ Delete User - Positive
        [Test]
        public async Task DeleteUser_ShouldReturn200Or404()
        {
            test.Log(Status.Info, "Deleting user: DeleteUser_ShouldReturn200Or404");

            try
            {
                var response = await _usersApi.DeleteUser("56bd");
                test.Log(Status.Info, "Response received: " + response.Content);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.NotFound));
                test.Log(Status.Pass, "User deleted or not found as expected ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw; // Rethrow to ensure NUnit marks it as failed
            }
        }

        //// ❌ Negative Test Cases ❌

        //// ✅ Fetching a non-existent user
        //[Test]
        //public async Task GetUser_NonExistent_ShouldReturnNotFound()
        //{
        //    test.Log(Status.Info, "Checking whether user exist or not: GetUser_NonExistent_ShouldReturnNotFound");

        //    var response = await _usersApi.GetUser("2001252 "); // Assuming 99999 does not exist
        //    TestContext.WriteLine(response.Content);
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        //}

        //// ✅ Creating a user with missing required fields
        //[Test]
        //public async Task CreateUser_MissingFields_ShouldReturn400Or422()
        //{
        //    test.Log(Status.Info, "Creating user with missing data: CreateUser_MissingFields_ShouldReturn400Or422");
        //    var response = await _usersApi.CreateUser("", ""); // Empty fields
        //    TestContext.WriteLine(response.Content);

        //    // Some APIs return 400, others return 422 Unprocessable Entity
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest).Or.EqualTo(HttpStatusCode.UnprocessableEntity));
        //}

        // ❌ Fetching a non-existent user
        [Test]
        public async Task GetUser_NonExistent_ShouldReturnNotFound()
        {
            test.Log(Status.Info, "Checking if the user exists: GetUser_NonExistent_ShouldReturnNotFound");

            try
            {
                var response = await _usersApi.GetUser("2001252"); // Assuming user does not exist
                test.Log(Status.Info, "Response received: " + response.Content);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                test.Log(Status.Pass, "User not found as expected ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw; // Ensure NUnit marks it as failed
            }
        }
        // ❌ Creating a user with missing required fields
        [Test]
        public async Task CreateUser_MissingFields_ShouldReturn400Or422()
        {
            test.Log(Status.Info, "Creating user with missing data: CreateUser_MissingFields_ShouldReturn400Or422");

            try
            {
                var response = await _usersApi.CreateUser("", ""); // Empty fields
                test.Log(Status.Info, "Response received: " + response.Content);

                // Some APIs return 400, others return 422 Unprocessable Entity
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest).Or.EqualTo(HttpStatusCode.UnprocessableEntity));
                test.Log(Status.Pass, "API returned expected error for missing fields ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw; // Ensure NUnit marks it as failed
            }
        }

        //// ✅ Creating a user with an invalid ID format (Fixed)
        //[Test]
        //public async Task CreateUser_InvalidId_ShouldReturnBadRequest()
        //{
        //    var response = await _usersApi.CreateUserWithInvalidId("abc", "Test User", "Developer"); // ID should be numeric
        //    TestContext.WriteLine(response.Content);
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        //}

        // ✅ Updating a non-existent user
        [Test]
        public async Task UpdateUser_NonExistent_ShouldReturnNotFound()
        {
            test.Log(Status.Info, "Updating a non-existent user: UpdateUser_NonExistent_ShouldReturnNotFound");

            try
            {
                var response = await _usersApi.UpdateUser("99999", "Nonexistent User", "Unknown");
                test.Log(Status.Info, "Response received: " + response.Content);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                test.Log(Status.Pass, "API returned 404 as expected ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw; // Ensure NUnit marks it as failed
            }
        }

        // ✅ Deleting a non-existent user
        [Test]
        public async Task DeleteUser_NonExistent_ShouldReturnNotFound()
        {
            test.Log(Status.Info, "Deleting a non-existent user: DeleteUser_NonExistent_ShouldReturnNotFound");

            try
            {
                var response = await _usersApi.DeleteUser("99999");
                test.Log(Status.Info, "Response received: " + response.Content);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                test.Log(Status.Pass, "API returned 404 as expected ✅");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed ❌: " + ex.Message);
                throw; // Ensure NUnit marks it as failed
            }
        }

        [OneTimeTearDown] // ✅ Runs once after all tests
        public void TearDownReport()
        {
            extent.Flush(); // ✅ Generates the report
        }
        //[TearDown]
        //public void TearDown()
        //{
        //    var status = TestContext.CurrentContext.Result.Outcome.Status;
        //    var message = TestContext.CurrentContext.Result.Message;

        //    if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        //    {
        //        test.Log(Status.Fail, "Test failed ❌: " + message);
        //    }
        //    else
        //    {
        //        test.Log(Status.Pass, "Test passed ✅");
        //    }
        //}

    }
}
