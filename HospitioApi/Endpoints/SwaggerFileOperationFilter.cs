using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HospitioApi.Endpoints;
public class SwaggerFileOperationFilter : IOperationFilter
{
    List<string> listOfFileUploadUrl = new List<string>();
    public SwaggerFileOperationFilter()
    {
        // Adding url to List
        listOfFileUploadUrl.Add("api/client-portal/files/upload-document");
        listOfFileUploadUrl.Add("api/hospitio-customer/file/upload-communicationfile");

    }
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath is not null && (context.ApiDescription.HttpMethod!.ToLower() == "post" || context.ApiDescription.HttpMethod!.ToLower() == "patch")
            && listOfFileUploadUrl.Contains(context.ApiDescription.RelativePath))
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Description = "upload file ",
                Content = new Dictionary<String, OpenApiMediaType>
                {
                    {
                        "multipart/form-data", new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Required = new HashSet<String>{ "file" },
                                Properties = new Dictionary<String, OpenApiSchema>
                                {
                                    {
                                        "file", new OpenApiSchema()
                                        {
                                            Type = "string",
                                            Format = "binary"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

    }


}

