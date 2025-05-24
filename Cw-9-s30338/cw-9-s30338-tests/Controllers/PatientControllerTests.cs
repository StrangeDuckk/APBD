using Moq;
using Cw_9_s30338.Controllers;
using Cw_9_s30338.DTOs;
using Cw_9_s30338.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Assert = Xunit.Assert;


namespace cw_9_s30338_tests.Controllers;

public class PatientControllerTests
{
    private readonly Mock<IDbService> _mockDbService;
    private readonly PatientController _patientController;

    public PatientControllerTests()
    {
        _mockDbService = new Mock<IDbService>();
        _patientController = new PatientController(_mockDbService.Object);
    }

    [Fact]
    public async Task GetPatientWithId_ReturnsPatient()
    {
        var patient = new GetPatientDTO()
        {
            IdPatient = 1,
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateTime(1990, 1, 1)
        };
        
        _mockDbService.Setup(s => s.GetPatientByIdAsync(1)).ReturnsAsync(patient);

        var result = await _patientController.GetPatientAsync(1);
        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<GetPatientDTO>(okResult.Value);
        Assert.Equal("John", returned.FirstName);
    }
}