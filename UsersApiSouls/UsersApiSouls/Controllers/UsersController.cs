using UsersApiSouls.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace UsersApiSouls.Controllers
{
    public class UsersController
    {
    }


public static class UserEndpoints
{
	public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));

        group.MapGet("/", async (UserssoulsContext db) =>
        {
            return await db.Users.ToListAsync();
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<User>, NotFound>> (int userid, UserssoulsContext db) =>
        {
            return await db.Users.AsNoTracking()
                .FirstOrDefaultAsync(model => model.UserId == userid)
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserById")
        .WithOpenApi();


            group.MapPut("/{userid}/updateCoins", async Task<Results<Ok<User>, NotFound>> (int userid, int newCoinAmount, UserssoulsContext db) =>
            {
                // Buscar al usuario por su ID
                var user = await db.Users.FirstOrDefaultAsync(u => u.UserId == userid);

                // Si el usuario no existe, devolver NotFound
                if (user == null)
                {
                    return TypedResults.NotFound();
                }

                // Actualizar solo las monedas (soulsCoin)
                user.SoulsCoin = newCoinAmount;

                // Guardar los cambios en la base de datos
                await db.SaveChangesAsync();

                // Devolver respuesta exitosa con el usuario actualizado
                return TypedResults.Ok(user);  // Devuelve el usuario actualizado
            })
 .WithName("UpdateUserCoins")
 .WithOpenApi();





            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int userid, User user, UserssoulsContext db) =>
        {
            var affected = await db.Users
                .Where(model => model.UserId == userid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.UserId, user.UserId)
                  .SetProperty(m => m.UserName, user.UserName)
                  .SetProperty(m => m.Email, user.Email)
                  .SetProperty(m => m.Password, user.Password)
                  .SetProperty(m => m.ProfilePicture, user.ProfilePicture)
                  .SetProperty(m => m.Description, user.Description)
                  .SetProperty(m => m.Birthday, user.Birthday)
                  .SetProperty(m => m.ProfileBorder, user.ProfileBorder)
                  .SetProperty(m => m.SoulsCoin, user.SoulsCoin)
                  .SetProperty(m => m.ProfileBackground, user.ProfileBackground)
                  .SetProperty(m => m.RankNum, user.RankNum)
                  .SetProperty(m => m.RankName, user.RankName)
                  .SetProperty(m => m.Nationality, user.Nationality)
                  .SetProperty(m => m.Gender, user.Gender)
                  .SetProperty(m => m.Race, user.Race)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapPost("/", async (User user, UserssoulsContext db) =>
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/User/{user.UserId}",user);
        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int userid, UserssoulsContext db) =>
        {
            var affected = await db.Users
                .Where(model => model.UserId == userid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}}
