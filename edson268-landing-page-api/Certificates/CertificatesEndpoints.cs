using Asp.Versioning.Builder;
using edson268_landing_page_api.Common;
using edson268_landing_page_api.Core.DTOs;
using edson268_landing_page_api.Core.Entities;
using edson268_landing_page_api.Data;
using Microsoft.EntityFrameworkCore;

namespace edson268_landing_page_api.Certificates
{
    public class CertificatesEndpoints : IEndpoints
    {
        public static IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder, ApiVersionSet versions)
        {
            var group = builder.MapGroup("/api/v1/certificates")
                            .WithTags("Certificates")
                            .WithApiVersionSet(versions)
                            .MapToApiVersion(1);

            group.MapGet("/", async (LandingPageDbContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var certificates = await context.Certificates
                        .OrderByDescending(c => c.CreatedAt)
                        .ToListAsync();

                    var response = certificates.Select(c => new CertificateResponse
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Institution = c.Institution,
                        Image = c.ImageUrl,
                        Url = c.CredentialUrl,
                        Type = c.Type
                    });

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error getting certificates");
                    return Results.Problem(
                        title: "Internal Server Error",
                        detail: "An error occurred while processing your request",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithSummary("Get all certificates")
            .WithDescription("Returns a list of all certificates")
            .Produces<List<CertificateResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapGet("/{id:guid}", async (Guid id, LandingPageDbContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var certificate = await context.Certificates.FindAsync(id);

                    if (certificate is null)
                    {
                        return Results.NotFound(new { message = $"Certificate with ID {id} not found" });
                    }

                    var response = new CertificateResponse
                    {
                        Id = certificate.Id.ToString(),
                        Name = certificate.Name,
                        Institution = certificate.Institution,
                        Image = certificate.ImageUrl,
                        Url = certificate.CredentialUrl,
                        Type = certificate.Type
                    };

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error getting certificate {Id}", id);
                    return Results.Problem(
                        title: "Internal Server Error",
                        detail: "An error occurred while processing your request",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithSummary("Get certificate by ID")
            .WithDescription("Returns a specific certificate by its ID")
            .Produces<CertificateResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPost("/", async (CreateCertificateRequest request, LandingPageDbContext context, ILogger<Program> logger) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Name))
                    {
                        return Results.BadRequest(new { message = "Name is required" });
                    }

                    var certificate = new Certificate
                    {
                        Name = request.Name,
                        Institution = request.Institution,
                        ImageUrl = request.ImageUrl,
                        CredentialUrl = request.CredentialUrl,
                        Type = request.Type
                    };

                    context.Certificates.Add(certificate);
                    await context.SaveChangesAsync();

                    var response = new CertificateResponse
                    {
                        Id = certificate.Id.ToString(),
                        Name = certificate.Name,
                        Institution = certificate.Institution,
                        Image = certificate.ImageUrl,
                        Url = certificate.CredentialUrl,
                        Type = certificate.Type
                    };

                    return Results.Created($"/api/certificates/{certificate.Id}", response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error creating certificate");
                    return Results.Problem(
                        title: "Internal Server Error",
                        detail: "An error occurred while creating the certificate",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .Accepts<CreateCertificateRequest>("application/json")
            .WithSummary("Create a new certificate")
            .WithDescription("Creates a new certificate and returns the created resource")
            .Produces<CertificateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdateCertificateRequest request, LandingPageDbContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var certificate = await context.Certificates.FindAsync(id);

                    if (certificate is null)
                    {
                        return Results.NotFound(new { message = $"Certificate with ID {id} not found" });
                    }

                    certificate.Name = request.Name ?? certificate.Name;
                    certificate.Institution = request.Institution ?? certificate.Institution;
                    certificate.ImageUrl = request.ImageUrl ?? certificate.ImageUrl;
                    certificate.CredentialUrl = request.CredentialUrl ?? certificate.CredentialUrl;

                    certificate.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error updating certificate {Id}", id);
                    return Results.Problem(
                        title: "Internal Server Error",
                        detail: "An error occurred while updating the certificate",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .Accepts<UpdateCertificateRequest>("application/json")
            .WithSummary("Update a certificate")
            .WithDescription("Updates an existing certificate by its ID")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapDelete("/{id:guid}", async (Guid id, LandingPageDbContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var certificate = await context.Certificates.FindAsync(id);

                    if (certificate is null)
                    {
                        return Results.NotFound(new { message = $"Certificate with ID {id} not found" });
                    }

                    context.Certificates.Remove(certificate);
                    await context.SaveChangesAsync();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error deleting certificate {Id}", id);
                    return Results.Problem(
                        title: "Internal Server Error",
                        detail: "An error occurred while deleting the certificate",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithSummary("Delete a certificate")
            .WithDescription("Deletes a specific certificate by its ID")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            group.MapPost("/seed", async (LandingPageDbContext context, ILogger<Program> logger) =>
            {
                try
                {
                    if (await context.Certificates.AnyAsync())
                    {
                        return Results.Conflict(new { message = "Database already contains certificates" });
                    }

                    var certificates = new List<Certificate>
                    {
                        new()
                        {
                            Name = "Java Standard Edition - 2015",
                            Institution = "CENTEC",
                            ImageUrl = "assets/img/certificates/centec.jpg",
                            CredentialUrl = "https://drive.google.com/file/d/12TrSTfCw8E9uE5gEfoSfw8aUQQhpdewk/view?usp=sharing",
                            Type = "CER",
                        }
                    };

                    await context.Certificates.AddRangeAsync(certificates);
                    await context.SaveChangesAsync();

                    return Results.Ok(new
                    {
                        message = "Certificates seeded successfully",
                        count = certificates.Count
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error seeding certificates");
                    return Results.Problem(
                        title: "Internal Server Error",
                        detail: "An error occurred while seeding certificates",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithSummary("Seed database with sample certificates")
            .WithDescription("Initializes the database with sample certificate data")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

            return builder;
        }
    }
}
