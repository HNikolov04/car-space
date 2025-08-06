using CarSpace.Data.Repositories.Interfaces;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Contracts.About.Request;
using CarSpace.Services.Core.Contracts.About.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;

namespace CarSpace.Services.Core.Services;

public class AboutUsService : IAboutUsService
{
    private readonly IAboutUsRepository _aboutUsRepository;

    public AboutUsService(IAboutUsRepository aboutUsRepository)
    {
        _aboutUsRepository = aboutUsRepository;
    }

    public async Task<GetAboutUsResponse> GetAsync()
    {
        var about = await _aboutUsRepository.GetAsync();

        if (about is null)
        {
            throw new NotFoundException(ExceptionMessages.AboutSectionNotFound);
        }

        return new GetAboutUsResponse(about.Title, about.Message);
    }

    public async Task UpdateAsync(UpdateAboutUsRequest request)
    {
        ValidateRequest(request);

        var about = await _aboutUsRepository.GetAsync();

        if (about is null)
        {
            throw new NotFoundException(ExceptionMessages.AboutSectionNotFound);
        }

        about.Title = request.Title;
        about.Message = request.Message;

        await _aboutUsRepository.UpdateAsync(about);
    }

    private static void ValidateRequest(UpdateAboutUsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Message))
        {
            throw new ValidationAppException(ExceptionMessages.InvalidAboutData);
        }
    }
}

