namespace MachineService.Tests
{
    using AutoFixture;
    using AutoFixture.AutoFakeItEasy;
    using EventBus;
    using FakeItEasy;
    using FluentAssertions;
    using MachineService.Factories;
    using MachineService.Managers;
    using MachineService.Models;
    using Xunit;

    public class MachineManagerTests
    {
        private readonly IFixture _fixture;
        private readonly IEventBus _eventBus;
        private readonly IMachineFactory _machineFactory;

        public MachineManagerTests()
        {
            _fixture = new Fixture().Customize(new AutoFakeItEasyCustomization());

            _eventBus = _fixture.Freeze<IEventBus>();
            _machineFactory = _fixture.Freeze<IMachineFactory>();

            A.CallTo(() => _machineFactory.CreateMachine(A<string>._))
             .ReturnsLazily(a => new Machine(a.Arguments.Get<string>(0), _eventBus));
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            var sut = new MachineManager(_fixture.Create<IMachineFactory>(), 10);

            // Act
            sut.GetMachine("M1").MachineName.Should().Be("M1");
        }
    }
}
