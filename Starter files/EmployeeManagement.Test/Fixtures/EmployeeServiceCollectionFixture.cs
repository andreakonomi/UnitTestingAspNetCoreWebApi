namespace EmployeeManagement.Test.Fixtures;

/// <summary>
/// Wraps the class fixture of EmployeeServiceFixture to make it a collection fixtures that all test classes can share it
/// </summary>
/// Attribute gives it a unique name that allows us to refer to it? A bit unclears
[CollectionDefinition("EmployeeServiceCollection")]
public class EmployeeServiceCollectionFixture : ICollectionFixture<EmployeeServiceFixture>
{
}