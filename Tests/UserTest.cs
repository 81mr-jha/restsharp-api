
using apiautomation.Pages;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace apiautomation.Tests
{
    [TestFixture]
    public class UsersTests
    {
        private UsersApi _usersApi;

        [SetUp]
        public void Setup()
        {
            _usersApi = new UsersApi();
        }

        [Test]
        public async Task GetUsers_ShouldReturnSuccess()
        {
            var response = await _usersApi.GetUsers();

            TestContext.WriteLine("Response Content: " + response.Content);
            TestContext.WriteLine("Status Code: " + response.StatusCode);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task CreateUser_AndVerifyWithGet_ShouldSucceed()
        {
            // Step 1: Create a new user
            var createResponse = await _usersApi.CreateUser("Nitin kumar", "Accountant");
            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            // Extract the user ID from the response
            var createdUser = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
            string userId = createdUser?.id;
            Assert.That(userId, Is.Not.Null.And.Not.Empty, "User ID should be returned after creation");

            TestContext.WriteLine("New User ID: " + userId);

            // Step 2: Fetch the user using the extracted ID
            var getResponse = await _usersApi.GetUser(userId);
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            TestContext.WriteLine("User found: " + getResponse.Content);

            // Step 3: Verify that the user details match
            var fetchedUser = JsonConvert.DeserializeObject<dynamic>(getResponse.Content);
            Assert.That((string)fetchedUser.name, Is.EqualTo("Nitin kumar"));
            Assert.That((string)fetchedUser.job, Is.EqualTo("Accountant"));
        }


        // ✅ Update User - Positive
        [Test]
        public async Task UpdateUser_ShouldReturn200()
        {
            var response = await _usersApi.UpdateUser("55", "Nitin kumar", "Qa Engineer");
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        // ✅ Delete User - Positive
        [Test]
        public async Task DeleteUser_ShouldReturn200Or404()
        {
            var response = await _usersApi.DeleteUser("56bd");
            TestContext.WriteLine(response.Content);

            // Some APIs return 404 if user does not exist, so check for both
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.NotFound));
        }

        // ❌ Negative Test Cases ❌

        // ✅ Fetching a non-existent user
        [Test]
        public async Task GetUser_NonExistent_ShouldReturnNotFound()
        {
            var response = await _usersApi.GetUser("2001252 "); // Assuming 99999 does not exist
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        // ✅ Creating a user with missing required fields
        [Test]
        public async Task CreateUser_MissingFields_ShouldReturn400Or422()
        {
            var response = await _usersApi.CreateUser("", ""); // Empty fields
            TestContext.WriteLine(response.Content);

            // Some APIs return 400, others return 422 Unprocessable Entity
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest).Or.EqualTo(HttpStatusCode.UnprocessableEntity));
        }

        // ✅ Creating a user with an invalid ID format (Fixed)
        [Test]
        public async Task CreateUser_InvalidId_ShouldReturnBadRequest()
        {
            var response = await _usersApi.CreateUserWithInvalidId("abc", "Test User", "Developer"); // ID should be numeric
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        // ✅ Updating a non-existent user
        [Test]
        public async Task UpdateUser_NonExistent_ShouldReturnNotFound()
        {
            var response = await _usersApi.UpdateUser("99999", "Nonexistent User", "Unknown");
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        // ✅ Deleting a non-existent user
        [Test]
        public async Task DeleteUser_NonExistent_ShouldReturnNotFound()
        {
            var response = await _usersApi.DeleteUser("99999");
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
