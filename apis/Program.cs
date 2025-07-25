using Microsoft.AspNetCore.Builder;
using RestVSoapDemo.Services;
using RestVSoapDemo.Soap;
using SoapCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning; 


var builder = WebApplication.CreateBuilder(args);

// Configurar URLs explícitamente
builder.WebHost.UseUrls("https://localhost:5001", "http://localhost:5000");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "REST vs SOAP Demo API",
        Version = "v1",
        Description = "Comparación entre API REST y SOAP en el mismo proyecto"
    });
});

builder.Services.AddSoapCore();

builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductSoapService, ProductSoapService>();
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0); //Establecer versión de la API
    opt.AssumeDefaultVersionWhenUnspecified = true; //Backward compatibility
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UriSegmentApiVersionReader(), //Obtener Version de la api
        new QueryStringApiVersionReader("version"), //extraer version desde request
        new HeaderApiVersionReader("X-Version")
        );


});

var app = builder.Build();


app.UseRouting();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST vs SOAP Demo API v1");
        c.RoutePrefix = "swagger"; // Swagger estará en /swagger
    });
}



app.UseRouting();
app.MapControllers();

app.UseSoapEndpoint<IProductSoapService>("/soap/ProductService.asmx", new SoapEncoderOptions()
{
    MessageVersion = System.ServiceModel.Channels.MessageVersion.Soap11,
    WriteEncoding = System.Text.Encoding.UTF8,
    ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max
});

app.MapGet("/", () => Results.Content(@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>REST vs SOAP Demo</title>
    <style>
        body { font-family: Arial; margin: 40px; background: #f5f5f5; }
        .container { max-width: 800px; background: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
        .api-section { margin: 20px 0; padding: 20px; border-left: 4px solid #007acc; background: #f9f9f9; border-radius: 4px; }
        .soap-section { border-left-color: #ff6b6b; }
        h1 { color: #333; text-align: center; }
        h2 { color: #007acc; }
        .soap-section h2 { color: #ff6b6b; }
        code { background: #e8e8e8; padding: 2px 6px; border-radius: 3px; font-family: 'Courier New', monospace; }
        .endpoint { margin: 10px 0; padding: 8px; background: white; border-radius: 3px; }
        .test-buttons { margin: 15px 0; }
        .btn { padding: 8px 16px; margin: 5px; background: #007acc; color: white; text-decoration: none; border-radius: 4px; display: inline-block; }
        .btn-soap { background: #ff6b6b; }
        .btn:hover { opacity: 0.8; }
    </style>
</head>
<body>
    <div class='container'>
        <h1> REST vs SOAP API Demo</h1>
        <p style='text-align: center; color: #666; margin-bottom: 30px;'>
            Este proyecto demuestra las diferencias clave entre APIs REST y SOAP implementadas en el mismo proyecto.
        </p>
        
        <div class='api-section'>
            <h2>REST API Endpoints</h2>
            <div class='endpoint'><strong>GET</strong> <code>/api/products</code> - Obtener todos los productos</div>
            <div class='endpoint'><strong>GET</strong> <code>/api/products/{id}</code> - Obtener producto por ID</div>
            <div class='endpoint'><strong>POST</strong> <code>/api/products</code> - Crear producto</div>
            <div class='endpoint'><strong>PUT</strong> <code>/api/products/{id}</code> - Actualizar producto</div>
            <div class='endpoint'><strong>DELETE</strong> <code>/api/products/{id}</code> - Eliminar producto</div>
            
            <div class='test-buttons'>
                <a href='/swagger' class='btn'> Ver Documentación Swagger</a>
                <a href='/api/products' class='btn'> Probar GET Products</a>
            </div>
            
            <p><strong> Formato:</strong> JSON | <strong> Protocolo:</strong> HTTP | <strong> Cacheable:</strong> Sí | <strong> Estados:</strong> Sin estado</p>
        </div>

        <div class='api-section soap-section'>
            <h2> SOAP API Endpoint</h2>
            <div class='endpoint'><strong>POST</strong> <code>/soap/ProductService.asmx</code> - Todas las operaciones via SOAP</div>
            
            <div class='test-buttons'>
                <a href='/soap/ProductService.asmx?wsdl' class='btn btn-soap'> Ver WSDL</a>
            </div>
            
            <h3>Operaciones disponibles:</h3>
            <ul>
                <li><code>GetAll()</code> - Obtener todos los productos</li>
                <li><code>GetById(int id)</code> - Obtener producto por ID</li>
                <li><code>Create(Product)</code> - Crear producto</li>
                <li><code>Update(int, Product)</code> - Actualizar producto</li>
                <li><code>Delete(int id)</code> - Eliminar producto</li>
            </ul>
            
            <p><strong> Formato:</strong> XML (SOAP Envelope) | <strong> Protocolo:</strong> SOAP/HTTP | <strong> Cacheable:</strong> No | <strong> Estados:</strong> Puede mantener estado</p>
        </div>

        <h2> Diferencias Clave</h2>
        <table style='width: 100%; border-collapse: collapse; margin: 20px 0; border: 1px solid #ddd;'>
            <tr style='background: #f0f0f0;'>
                <th style='padding: 12px; border: 1px solid #ddd;'>Aspecto</th>
                <th style='padding: 12px; border: 1px solid #ddd; color: #007acc;'>REST API</th>
                <th style='padding: 12px; border: 1px solid #ddd; color: #ff6b6b;'>SOAP API</th>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>URLs</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>Múltiples endpoints semánticos (/api/products, /api/products/1)</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>Un solo endpoint (/soap/ProductService.asmx)</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Formato</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>JSON (ligero, legible)</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>XML con SOAP Envelope (verbose)</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Verbos HTTP</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>GET, POST, PUT, DELETE, PATCH</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>Solo POST</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Manejo de Errores</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>Códigos HTTP (200, 404, 500, etc.)</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>SOAP Faults (XML)</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Documentación</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>OpenAPI/Swagger (interactiva)</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>WSDL (definición formal)</td>
            </tr>
            <tr>
                <td style='padding: 10px; border: 1px solid #ddd;'><strong>Facilidad de uso</strong></td>
                <td style='padding: 10px; border: 1px solid #ddd;'>Simple, directo con herramientas web</td>
                <td style='padding: 10px; border: 1px solid #ddd;'>Requiere cliente SOAP especializado</td>
            </tr>
        </table>

        <div style='margin-top: 30px; padding: 20px; background: #e8f4f8; border-radius: 8px;'>
            <h3> Cómo probar:</h3>
            <p><strong>REST:</strong> Usa el navegador, Postman, curl, o la interfaz Swagger</p>
            <p><strong>SOAP:</strong> Usa SoapUI, herramientas .NET, o un cliente SOAP personalizado</p>
        </div>
    </div>
</body>
</html>
", "text/html"));

Console.WriteLine("REST vs SOAP Demo iniciado!");
Console.WriteLine("REST API Swagger: https://localhost:5001/swagger");
Console.WriteLine("SOAP WSDL: https://localhost:5001/soap/ProductService.asmx?wsdl");
Console.WriteLine("Página principal: https://localhost:5001/");

app.Run();
