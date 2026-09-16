// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

using System.Diagnostics;
using Bodoconsult.Web.Mail.Helpers;
using NUnit.Framework;

// ReSharper disable InconsistentNaming

namespace BodoWebMailer.Test;

[TestFixture]
public class PasswordHandlerTests
{
    [Test]
    public void Decrypt_ValidPassword_PasswordDecrypted()
    {
        const string password = "Blubb";

        var encryptedPassword = PasswordHandler.Encrypt(password);

        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password==decryptedPassword);
    }

    [Test]
    public void Decrypt2_ValidPassword_PasswordDecrypted()
    {
        const string password = "Blubb";

        var encryptedPassword = PasswordHandler.Encrypt2(password);

        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt2(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password == decryptedPassword);
    }

    [Test]
    public void Decrypt3_ValidPassword_PasswordDecrypted()
    {
        const string password = "Blubb";

        var encryptedPassword = PasswordHandler.Encrypt3(password);

        Debug.Print(encryptedPassword);

        var decryptedPassword = PasswordHandler.Decrypt3(encryptedPassword);

        Assert.That(!string.IsNullOrEmpty(encryptedPassword));
        Assert.That(password == decryptedPassword);
    }

    [Test]
    public void CreateHash_ValidPassword_HashCreated()
    {
        const string password = "Test";

        const int saltBytes = 128;
        const int hashBytes = 128;
        const int iterations = 10000;

        var salt = PasswordHandler.CreateSalt(saltBytes);

        var hashedPassword = PasswordHandler.CreateHash(password, salt, hashBytes, iterations);

        Debug.Print(hashedPassword);

        Assert.That(hashedPassword.Length>0);

        var erg = PasswordHandler.ValidateHash(password, salt, hashedPassword, iterations);
        Assert.That(erg);
    }

    [Test]
    public void CreateHash_ValidPasswordLong_HashCreated()
    {
        const string password = "Test";

        const int saltBytes = 512;
        const int hashBytes = 512;
        const int iterations = 10000;

        var salt = PasswordHandler.CreateSalt(saltBytes);

        var hashedPassword = PasswordHandler.CreateHash(password, salt, hashBytes, iterations);

        Debug.Print(hashedPassword);

        Assert.That(hashedPassword.Length > 0);

        var erg = PasswordHandler.ValidateHash(password, salt, hashedPassword, iterations);
        Assert.That(erg);
    }


    [Test]
    public void CreateHash_ValidPassword_ValdidateRealSuccessful()
    {
        const string password = "Test123";
        const int iterations = 10000;

        var salt = "yO+U3TLU/4xb6a5drs6u8IJGDkGk5pzpw1PRD4wvhpPF564Rwu/em8u9/Oi/fdA7yEAjuJ7jJCH6aIe1+Ixc+qK9qOpy1TYRYwXrzj2sq/h8W6NaBMRm2oya2RSShAi0e9EaR5ugNx29iZiG927A9V03TYx/rhfy6Be5VsGzl3/A5jebh9C2LN9YpU0NisU5nU0sxM/IwzJXYHWUz5CbvEqAfMSeIHuqyxOXIQ0HqeP/rWMNrrVX38i5ChL63FCqOAjDzLlP+AUZXSUlMHM04faBbJaB7YRZQ7FC3LbCQB8malPGOXFzDhDCVD4nYfpVOZUVBcCQGuyh/qnHR5rSZoIRVt3IBzOunvA2horU10zxMEqrtHFl2eEz4BjPxkLUOrhs7z0JqxWhUWxpvkOYCytDA1P/PpkuEa3GD4ve4/BDgpdYqEYLLPDsz7VGlRUE6mZtjKXVjH0YMBwGbCmagrj2dq9n5HTObppFvUuuVX9uVgjlNuCNaDM5k+WE0vDGHxIgPBd/bdlmTq5C7bx8RE7drtNhCQYd0RdT/ncZNDix8bhkToY0p0Hk79Ql+iyBlpWH3F9xCEv1OFgX2wi7cfWpGxF8xgnDmx5y5ZoOqhJiQ9KtUWu44wLXYF6qqGBnqPgrh5v2dVFAgc8+iIgQ9W6uTwoA3sECOVNUH+KwNHU=";

        var hashedPassword1 = PasswordHandler.CreateHash(password, salt, 512, iterations);

        Debug.Print(hashedPassword1);

        var hashedPassword = "n7MLCKXs2AXITuX+QKvmawoD6fKVK4M5dF7JX0H+vGHhDBd4tzAutIfLeHi3EKTZRMyi+1MhXuUL9XcprmoLqlI08kRseumKyvaE5g5fAki3QDQpwslP8QuxesBkbciGC8vNh3DcBhW/Jdxxytm5Qj0tT8QLE/eEhC/djetNw1A97bj/06GTEksy8mKfDIwv+AJF/21QtbWiYkpANQLQ5jQfXnRGXxQj0jlJcYR+YNFqpHbquWK4HDIOCAPPeUQeEJwJnX4PtPoxrYj6oJGDh2gp6TxQZGRLcFK9rhvGhVzl6agt9t4GKpt/gQnq7Za7ZBO/g1bvUR00NRH/cRNi0rXE/FlTSFv5JkgLk28FG9hoZV+uoLLh8AEFjSXYDzaYO/eTJa3SVqVQ5xSWrguc+CiZAdV4Neqmbyi/4XZcNb2mSVgIDeKjp4CFF6b5254TJFQTwdjOkOclUQmE6h2hdVTNoY7LDu2MZ+/3jz0k/rmZeYAAMa0sx8P9UXGDaX71wf8brFf4zmXbGFEiU1eFzb+oYKlYQXgK/QDG6BVrCmhizHIE6qHWHlZMDjiPgGQxle0OZLTjZtcu9RjUks4GgzTXl6TLhJmKx8Xs5ShMcDZFgYg9flUG2YGiRXc+EytcnzPCaNwJvolJdMhB5lqeXxMiRe6dLYw4eNsb8JgCJ1Y=";
        Debug.Print(hashedPassword);

        Assert.That(hashedPassword, Is.EqualTo(hashedPassword1));

        var erg = PasswordHandler.ValidateHash(password, salt, hashedPassword, iterations);

        Assert.That(erg);
    }
}