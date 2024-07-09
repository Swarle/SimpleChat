using AutoMapper;
using SimpleChat.BL.Helpers;

namespace UnitTests;

internal static class UnitTestHelper
{
    public static Mapper GetMapper()
    {
        var profile = new MapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));

        return new Mapper(configuration);
    }
}