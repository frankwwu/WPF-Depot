using Bogus;
using Kanban.Models;

namespace Kanban.Services;

/// <summary>Generates a demo board with fake people and work items.</summary>
public static class SampleData
{
    public static BoardSnapshot Create(int userCount = 5, int itemCount = 12)
    {
        var faker = new Faker("en");

        var users = Enumerable.Range(1, userCount)
            .Select(id => new User(id, faker.Name.FirstName(), faker.Name.LastName()))
            .ToList();

        var statuses = Enum.GetValues<WorkItemStatus>();

        var items = Enumerable.Range(1, itemCount)
            .Select(id => new WorkItemSnapshot(
                Id: id,
                Title: faker.Hacker.Phrase(),
                Status: statuses[Random.Shared.Next(statuses.Length)],
                AssigneeId: users[Random.Shared.Next(users.Count)].Id))
            .ToList();

        return new BoardSnapshot { Users = users, Items = items };
    }
}
