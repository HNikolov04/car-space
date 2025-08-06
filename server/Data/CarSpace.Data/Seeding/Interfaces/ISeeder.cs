namespace CarSpace.Data.Seeding.Interfaces;

public interface ISeeder
{
    Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}
