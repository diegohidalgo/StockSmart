using NetArchTest.Rules;
using NUnit.Framework;
using StockSmart.Architecture.Tests.Constants;

namespace StockSmart.Architecture.Tests
{
    public class ArchitectureTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Infrastructure_When_Used_Should_Not_Have_Dependencies()
        {
            var infrastructureAssembly = typeof(Infrastructure.AssemblyReference).Assembly;

            var projects = new[]
            {
                ArchitectureConstants.StockSmartPresentation,
                ArchitectureConstants.StockSmartWebApp
            };

            var result = Types.InAssembly(infrastructureAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(projects)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void Presentation_When_Used_Should_Not_Have_Dependencies()
        {
            var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

            var projects = new[]
            {
                ArchitectureConstants.StockSmartInfrastructure,
                ArchitectureConstants.StockSmartWebApp
            };

            var result = Types.InAssembly(presentationAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(projects)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void Application_When_Used_Should_Not_Have_Dependencies()
        {
            var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

            var projects = new[]
            {
                ArchitectureConstants.StockSmartPresentation,
                ArchitectureConstants.StockSmartInfrastructure,
                ArchitectureConstants.StockSmartWebApp
            };

            var result = Types.InAssembly(applicationAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(projects)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void Domain_When_Used_Should_Not_Have_Dependencies()
        {
            var domainAssembly = typeof(Domain.AssemblyReference).Assembly;

            var projects = new[]
            {
                ArchitectureConstants.StockSmartPresentation,
                ArchitectureConstants.StockSmartApplication,
                ArchitectureConstants.StockSmartInfrastructure,
                ArchitectureConstants.StockSmartWebApp
            };

            var result = Types.InAssembly(domainAssembly)
                .ShouldNot()
                .HaveDependencyOnAll(projects)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void Presentation_Controllers_When_Used_Should_Have_Dependency_With_Mediatr()
        {
            var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

            var result = Types
                .InAssembly(presentationAssembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }

        [Test]
        public void Application_Handlers_When_Used_Should_Have_Dependency_With_Domain()
        {
            var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

            var result = Types
                .InAssembly(applicationAssembly)
                .That()
                .HaveNameEndingWith("Handler")
                .Should()
                .HaveDependencyOn(ArchitectureConstants.StockSmartDomain)
                .GetResult();

            Assert.IsTrue(result.IsSuccessful);
        }
    }
}
