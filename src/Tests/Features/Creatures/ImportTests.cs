﻿using DMPowerTools.Core.Features.Creatures;
using DMPowerTools.Core.Infrastructure;
using DMPowerTools.Core.Models;

namespace DMPowerTools.Tests.Features.Creatures;

public class ImportTests : IntegrationTestBase
{
    [Fact]
    public async Task GivenAnyCreature_WhenItIsAccepted_ThenIsSaved()
    {
        var mockCreature = CreateFakeCreature();

        await SendAsync(new AcceptCreatureCommand { Creature = mockCreature });

        var creature = await FirstOrDefaultAsync<Creature>();
        creature.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenAnyCreatures_WhenTheyAreAllAccepted_ThenAllAreSaved()
    {
        var mockCreatures = new[] { CreateFakeCreature(), CreateFakeCreature() };

        await SendAsync(new AcceptAllCreaturesCommand { Creatures = mockCreatures });

        var dbContext = GetRequiredService<ApplicationDbContext>();
        dbContext.Creatures.Should().HaveCount(2);
    }

    [Fact]
    public async Task GivenAnyCreatureName_WhenItHasAlreadyBeenImported_ThenIsADuplicate()
    {
        var mockCreature = CreateFakeCreature();
        await AddAsync(mockCreature);

        var result = await SendAsync(new IsDuplicateCreatureQuery { Name = mockCreature.Name });

        result.Should().BeTrue();
    }

    [Fact]
    public async Task GivenAnyCreatureName_WhenItHasNotBeenImported_ThenIsNotADuplicate()
    {
        var mockCreature = CreateFakeCreature();
        await AddAsync(mockCreature);

        var result = await SendAsync(new IsDuplicateCreatureQuery { Name = "Something Else" });

        result.Should().BeFalse();
    }
}
