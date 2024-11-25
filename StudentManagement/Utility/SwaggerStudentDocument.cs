using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StudentManagement.Utility
{
    public class SwaggerStudentDocument: IConfigureOptions<SwaggerGenOptions>
    {
        public SwaggerStudentDocument()
        {
            
        }
        public void Configure(SwaggerGenOptions options) {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { 
            Description = "Enter the authentication token and start it with Bearer\n"+
            "For example: Bearer skd;fk9230uej39djt54hvowtcfon4938dsadlsahfsdlajfj",
            Name="Authorization",
            In=ParameterLocation.Header,
            Type=SecuritySchemeType.ApiKey,
            Scheme="Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme(){
                    Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    },
                    Name="Bearer",
                    In=ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
            var pathComment = Path.Combine(AppContext.BaseDirectory, "SwaggerComment.xml");
            options.IncludeXmlComments(pathComment);
        }
    }
}
