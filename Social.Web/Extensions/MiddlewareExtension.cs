using Social.Web.Middlewares;

namespace Social.Web.Extensions;

public static class MiddlewareExtension
{
    public static void AddMiddlewares(this WebApplication app)
    {
        app.UseCors(policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
        
        app.UseSwagger(c =>
        {
            c.SerializeAsV2 = true;
        });
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseExceptionHandler();
     
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static void UseExceptionHandler(this IApplicationBuilder builder)=>
        builder.UseMiddleware<ExceptionMiddleware>();
}